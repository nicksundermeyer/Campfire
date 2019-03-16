using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCheck : MonoBehaviour
{
    public RenderTexture lightCheckTexture;
    public float LightLevel = 1000000000f;

    // Update is called once per frame
    void Update()
    {
        // copy rendertexture into temporary rendertexture for this frame
        RenderTexture tmpTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(lightCheckTexture, tmpTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpTexture;

        // copy rendertexture values into texture2D
        Texture2D tmp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);
        tmp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
        tmp2DTexture.Apply();

        // release temporary rendertexture
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpTexture);

        // get pixel values out of texture
        Color32[] colors = tmp2DTexture.GetPixels32();
        Destroy(tmp2DTexture);

        // calculate light level by adding up all pixel values
        LightLevel = 0;
        for(int i=0; i<colors.Length; i++)
        {
            // formula to calculate light level from rgb values
            LightLevel += ((0.2126f * colors[i].r) + (0.7152f * colors[i].g) + (0.0722f * colors[i].b));
        }

        if(LightLevel == 0f)
        {
            LightLevel = 5000000;
        }

        // Debug.Log("LightLevel: " + LightLevel);
    }
}
