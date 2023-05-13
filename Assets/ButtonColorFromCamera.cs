using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorFromCamera : MonoBehaviour
{
    public Button button;
    public Camera mainCamera;

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.AddListener(GetColorFromCamera);
    }

    private void GetColorFromCamera()
    {
        // Capture screenshot from the main camera
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();

        // Convert screenshot to texture
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Get color from a specific pixel
        Color color = texture.GetPixel(Screen.width / 2, Screen.height / 2);

        // Apply color to the button's image component
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }

        // Clean up
        RenderTexture.active = null;
        mainCamera.targetTexture = null;
        Destroy(renderTexture);
        Destroy(texture);
    }
}
