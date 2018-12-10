using System;

using UnityEngine;
using UnityEngine.Networking;

using ZXing;
using ZXing.QrCode;

/// <summary>
/// Handles QR codes and also a networking class for the phone build.
/// </summary>
class QrReader : MonoBehaviour{
    private static NetworkClient client;

    private IBarcodeReader barcodeReader;
    private WebCamTexture camTexture;
    private Rect screenRect;
    private int delay;
    private bool isReadingQr = true;

    /// <summary>
    /// Encodes the given text into a QR code.
    /// </summary>
    public static Color32[] Encode(string textForEncoding,
    int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    /// <summary>
    /// Retrieves the device's camera.
    /// </summary>
    /// <returns>Camera associated with current device.</returns>
    private WebCamDevice GetCamera() {
        WebCamDevice selectedCamera = WebCamTexture.devices[0];
        foreach (WebCamDevice device in WebCamTexture.devices)
        {
            if (!device.isFrontFacing)
            {
                selectedCamera = device;
                break;
            }
        }
        return selectedCamera;
    }

    /// <summary>
    /// Sends a reset message.
    /// </summary>
    public void ResetController()
    {
        ControllerData.SetReset();
    }

    private void Start()
    {
        client = new NetworkClient();
        ControllerData.InitClient();
        barcodeReader = new BarcodeReader();
        delay = 0;

        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture(GetCamera().name)
        {
            requestedFPS = 30,
            requestedWidth = Screen.width,
            requestedHeight = Screen.height
        };
        if (camTexture != null)
        {
            camTexture.Play();
        }
    }

    private void OnGUI()
    {
        // do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
        if (!isReadingQr)
        {
            return;
        }
        // drawing the camera on screen
        Graphics.DrawTexture(screenRect, camTexture);
        if (delay < 15)
        {
            delay++;
            return;
        }
        delay = 0;
        try
        {
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(),
              camTexture.width, camTexture.height);
            if (result != null)
            {
                client.Connect(result.Text, ControllerData.APPLICATION_PORT);
            }
        }
        catch (Exception ex) {
            Debug.LogWarning(ex.Message);
        }
    }

    private void Update()
    {
        if (client.isConnected)
        {
            ControllerData.SendData(client);
            if (isReadingQr)
            {
                camTexture.Stop();
                isReadingQr = false;
            }
        }
        else
        {
        }
    }

}
