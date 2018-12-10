using System;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

using System.Net;
using System.Net.Sockets;

/// <summary>
/// A networking class for the desktop build.
/// </summary>
public class Server : MonoBehaviour {
    private static bool lockPlayerControls = false;

    [SerializeField]
    private Text debugText;
    [SerializeField]
    private Text IP;
    [SerializeField]
    private Text numberConnected;
    [SerializeField]
    private Player playerTemplate;
    [SerializeField]
    private GameObject title;

    private const string horizontalAxisName = "Horizontal"; 
	private const string verticalAxisName = "Vertical";

    private bool isShowingQr = false;
    private Texture2D QrCode;
    private int QrSize = 256;

    /// <summary>
    /// Sets the status on locking player controls.
    /// </summary>
    public static void LockPlayerControls(bool lockStatus)
    {
        lockPlayerControls = lockStatus;
    }

    /// <summary>
    /// Retrieves the status of QR being shown.
    /// </summary>
    /// <returns>If QR is shown.</returns>
    public bool IsShowingQr()
    {
        return isShowingQr;
    }

    /// <summary>
    /// Drops a player from the server.
    /// </summary>
    public void DropPlayer()
    {
        throw new System.NotImplementedException();
    }

    private Texture2D generateQR(string text)
    {
        Texture2D encoded = new Texture2D(QrSize, QrSize);
        Color32[] color32 = QrReader.Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    private string GetLocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    private void ServerReceiveMessage(NetworkMessage rawMessage)
	{
		string message = rawMessage.ReadMessage<StringMessage>().value;
        int id = rawMessage.conn.connectionId;
        Debug.Log("id " + id);

        if (!lockPlayerControls)
        {
            ControllerData.ApplyControllerDataServer(0, message);
        }
    }

    private void Start()
    {
        ControllerData.InitServer(playerTemplate);

        QrCode = generateQR(GetLocalIPAddress());
        isShowingQr = true;

        NetworkServer.Listen(ControllerData.APPLICATION_PORT);
        NetworkServer.RegisterHandler(ControllerData.DEFAULT_MESSAGE_TYPE, ServerReceiveMessage);
    }

    private void Update()
    {
        IP.text = GetLocalIPAddress();
        numberConnected.text = Math.Max(0, NetworkServer.connections.Count - 1).ToString();
        bool tempIsShowingQr = Math.Max(0, NetworkServer.connections.Count - 1) == 0;
        if (isShowingQr != tempIsShowingQr)
        {
            isShowingQr = tempIsShowingQr;
            title.SetActive(isShowingQr);
        }
    }

    private void OnGUI()
    {
        if (isShowingQr)
        {
            GUI.Box(new Rect((Screen.width - QrSize) / 2, (Screen.height - QrSize) / 2, QrSize, QrSize), QrCode);
        }
    }
}

