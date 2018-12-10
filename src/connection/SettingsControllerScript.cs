using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Control panel for filtering data on the client.
/// </summary>
public class SettingsControllerScript : MonoBehaviour {

    [SerializeField]
    private CanvasScript canvas;
    [SerializeField]
    private Slider accelX;
    [SerializeField]
    private Slider accelY;
    [SerializeField]
    private Slider movmentThreshold;
    [SerializeField]
    private Slider gravityCompensation;
    [SerializeField]
    private Slider movementScale;
    [SerializeField]
    private Slider movmentRadius;
    [SerializeField]
    private Slider latency;

    private static float ax = 0;
    private static float ay = 0;
    private static float mt = 48;
    private static float gc = 25;
    private static float ms = 5;
    private static float mr = 2;
    private static float l = 0;

    public static float AX
    {
        get { return ax; }
    }
    public static float AY
    {
        get { return ay; }
    }
    public static float MT
    {
        get { return mt / 100f; }
    }
    public static float GC
    {
        get { return gc / 100f; }
    }
    public static float MS
    {
        get { return ms; }
    }
    public static float MR
    {
        get { return mr; }
    }
    public static int L
    {
        get { return (int)l; }
    }

    private void Start()
    {
        Save();
    }

    /// <summary>
    /// Cancels the current changes.
    /// </summary>
    public void Cancel()
    {
        accelX.value = ax;
        accelY.value = ay;
        movmentThreshold.value = mt;
        gravityCompensation.value = gc;
        movementScale.value = ms;
        movmentRadius.value = mr;
        latency.value = l;
        canvas.ToggleSettings();
    }

    /// <summary>
    /// Saves the current changes.
    /// </summary>
    public void Save()
    {
        ax = accelX.value;
        ay = accelY.value;
        mt = movmentThreshold.value;
        gc = gravityCompensation.value;
        ms = movementScale.value;
        mr = movmentRadius.value;
        l = latency.value;
        canvas.ToggleSettings();
    }
}
