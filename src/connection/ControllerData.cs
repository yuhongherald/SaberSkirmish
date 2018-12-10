using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

/// <summary>
/// A common class between the server and client build. Interfaces data between
/// <see cref="Server"/> and <see cref="QrReader"/>.
/// </summary>
public class ControllerData
{
    public const int MAX_NUM_PLAYERS = 1;
    public const int TIMEOUT = 300;
    public const int DEFAULT_MESSAGE_TYPE = 888;
    public const int APPLICATION_PORT = 25000;
    private const char MESSAGE_SEPARATOR = '|';

    private static int timer = 0;
    private static bool reset = false;

    private static Player[] players;
    private static Vector3[] speed;
    private static Quaternion adjustment;

    private static GameObject redBall;
    private static GameObject greenBall;
    private static GameObject blueBall;
    private static GameObject blackBall;

    private enum ClientMessage
    {
        ACCELERATION = 0,
        GRAVITY,
        GYRO_ATTITUDE,
        MOVEMENT_THRESHOLD,
        GRAVITY_COMPENSATION,
        MOVEMENT_SCALE,
        MOVEMENT_RADIUS,
        TIME_LATENCY,
        RESET,
        TOTAL
    }

    private enum ServerMessage
    {
        VIBRATE = 0,
        TOTAL
    }

    /// <summary>
    /// Initializes class for server use.
    /// </summary>
    /// <param name="playerTemplate">Template to create player clones</param>
    public static void InitServer(Player playerTemplate)
    {
        adjustment = Quaternion.Euler(-90, 0, 0);
        InitPlayers(playerTemplate);
        InitBallMarkers();
    }

    /// <summary>
    /// Initializes class for client use.
    /// </summary>
    public static void InitClient()
    {
        reset = false;
        timer = 0;
        Input.gyro.enabled = true;
    }

    /// <summary>
    /// Sends a reset signal from <see cref="QrReader"/> to <see cref="Server"/>.
    /// </summary>
    public static void SetReset()
    {
        reset = true;
    }

    /// <summary>
    /// Packages data into a String.
    /// </summary>
    /// <param name="reset">Flag to specify for control reset.</param>
    /// <returns>Packed message.</returns>
    public static StringMessage CreateStringMessage(bool reset)
    {
        string[] rawMessage = new string[(int)ClientMessage.TOTAL];

        Vector3 acceleration = Input.acceleration;
        Vector3 gravity = Input.gyro.gravity;
        Quaternion rotation = Input.gyro.attitude;
        float movementThreshold = SettingsControllerScript.MT;
        float gravityCompensation = SettingsControllerScript.GC;
        float movementScale = SettingsControllerScript.MS;
        float movementRadius = SettingsControllerScript.MR;
        float timeLatency = Time.deltaTime * SettingsControllerScript.L;

        rawMessage[(int)ClientMessage.ACCELERATION] = Util.Vector3ToString(acceleration);
        rawMessage[(int)ClientMessage.GRAVITY] = Util.Vector3ToString(gravity);
        rawMessage[(int)ClientMessage.GYRO_ATTITUDE] = Util.QuaternionToString(rotation);
        rawMessage[(int)ClientMessage.MOVEMENT_THRESHOLD] = movementThreshold.ToString();
        rawMessage[(int)ClientMessage.GRAVITY_COMPENSATION] = gravityCompensation.ToString();
        rawMessage[(int)ClientMessage.MOVEMENT_SCALE] = movementScale.ToString();
        rawMessage[(int)ClientMessage.MOVEMENT_RADIUS] = movementRadius.ToString();
        rawMessage[(int)ClientMessage.RESET] = Util.BoolToString(reset);
        
        return new StringMessage(string.Join(MESSAGE_SEPARATOR.ToString(), rawMessage));
    }

    /// <summary>
    /// Immediately sends the data from the client to the server through a <see cref="NetworkClient"/>.
    /// </summary>
    /// <param name="client">Communication channel to server.</param>
    public static void SendData(NetworkClient client)
    {
        timer++;
        if (timer >= SettingsControllerScript.L)
        {
            client.Send(DEFAULT_MESSAGE_TYPE, CreateStringMessage(reset));
            reset = false;
            timer = 0;
        }
    }

    /// <summary>
    /// Parses message from client into data and applies actions.
    /// </summary>
    /// <param name="message">String encoded message from client.</param>
    public static void ApplyControllerDataClient(string message)
    {
        string[] deltas = message.Split(MESSAGE_SEPARATOR);
        if (Util.StringToBool(deltas[(int)ServerMessage.VIBRATE])) {
            Handheld.Vibrate();
        }
    }

    /// <summary>
    /// Parses message from server into data and applies actions.
    /// </summary>
    /// <param name="playerNumber">The corresponding player to apply to.</param>
    /// <param name="message">String encoded message from server.</param>
    public static void ApplyControllerDataServer(int playerNumber, string message)
    {
        Player player = players[playerNumber];
        player.LastActive = Time.time;
        float deltaTime = Mathf.Min(player.LastActive - Time.time, 120 * Time.deltaTime);
        player.gameObject.SetActive(true);
        player.PlayerActiveTimer = TIMEOUT;

        string[] deltas = message.Split(MESSAGE_SEPARATOR);
        Quaternion preRotation = Util.StringToQuaternion(deltas[(int)ClientMessage.GYRO_ATTITUDE]);
        Quaternion rotation = adjustment * preRotation;
        Quaternion gravityRotation = preRotation;
        Vector3 acceleration = Util.StringToVector3(deltas[(int)ClientMessage.ACCELERATION]);
        Vector3 gravity = Util.StringToVector3(deltas[(int)ClientMessage.GRAVITY]);
        float movementRadius = float.Parse(deltas[(int)ClientMessage.MOVEMENT_RADIUS]);

        float gravityCompensation = float.Parse(deltas[(int)ClientMessage.GRAVITY_COMPENSATION]);
        float movementThreshold = float.Parse(deltas[(int)ClientMessage.MOVEMENT_THRESHOLD]);
        float movementScale = float.Parse(deltas[(int)ClientMessage.MOVEMENT_SCALE]);

        if (Util.StringToBool(deltas[(int)ClientMessage.RESET]))
        {
            player.OffsetRotation = Quaternion.Inverse(rotation);
            player.ResetWeapon();
        }
        Quaternion newRotation = Quaternion.Inverse(rotation * player.OffsetRotation);
        player.SetWeaponRotation(newRotation);
    }

    /// <summary>
    /// Checks for inactive players and drops them. Place in an appropriate update loop.
    /// </summary>
    public static void UpdatePlayerStatus()
    {
        for (int i = 0; i < MAX_NUM_PLAYERS; i++)
        {
            if (players[i].PlayerActiveTimer > 0)
            {
                players[i].PlayerActiveTimer--;
                if (players[i].PlayerActiveTimer <= 0)
                {
                    players[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private static void InitPlayers(Player playerTemplate)
    {
        players = new Player[MAX_NUM_PLAYERS];
        Player player;
        for (int i = 0; i < MAX_NUM_PLAYERS; i++)
        {
            player = playerTemplate;
            player.OffsetRotation = Quaternion.identity;
            player.gameObject.SetActive(true);
            player.transform.localRotation = Quaternion.identity;
            player.ResetWeapon();
            players[i] = player;
        }
    }

    private static void InitBallMarkers()
    {
        // balls for debugging
        redBall = InitBall(Color.red, 0.3f);
        greenBall = InitBall(Color.green, 0.3f);
        blueBall = InitBall(Color.blue, 0.3f);
        blackBall = InitBall(Color.black, 0.3f);
    }

    private static GameObject InitBall(Color color, float scale)
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.GetComponent<MeshRenderer>().material.color = color;
        ball.transform.localScale = Vector3.one * scale;
        ball.SetActive(false);
        return ball;
    }
}
