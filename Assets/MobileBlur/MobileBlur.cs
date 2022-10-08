using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MobileBlur : MonoBehaviour
{
    [Range(1, 5)]
    public int NumberOfPasses = 3;
    [Range(2, 3)]
    public int KernelSize = 2;
    [Range(0, 3)]
    public float BlurAmount = 1;
    public Texture2D maskTexture; 
    private Texture2D previous;
    public Material material = null;

    static readonly string kernelKeyword = "KERNEL";
    static readonly int blurAmountString = Shader.PropertyToID("_BlurAmount");
    static readonly int blurTexString = Shader.PropertyToID("_BlurTex");
    static readonly int maskTexString = Shader.PropertyToID("_MaskTex");

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (KernelSize == 2)
            material.DisableKeyword(kernelKeyword);
        else
            material.EnableKeyword(kernelKeyword);

        material.SetFloat(blurAmountString, BlurAmount);

        if(maskTexture != null || previous != maskTexture)
        {
            previous = maskTexture;
            material.SetTexture(maskTexString, maskTexture);
        }

        RenderTexture blurTex = null;

        if (BlurAmount == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }
        else if (NumberOfPasses == 1)
        {
            blurTex = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2, 0, source.format);
            Graphics.Blit(source, blurTex, material, 0);
        }
        else if (NumberOfPasses == 2)
        {
            blurTex = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            Graphics.Blit(source, temp1, material, 0);
            Graphics.Blit(temp1, blurTex, material, 0);
            RenderTexture.ReleaseTemporary(temp1);
        }
        else if (NumberOfPasses == 3)
        {
            blurTex = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8, 0, source.format);
            Graphics.Blit(source, blurTex, material, 0);
            Graphics.Blit(blurTex, temp1, material, 0);
            Graphics.Blit(temp1, blurTex, material, 0);
            RenderTexture.ReleaseTemporary(temp1);
        }
        else if (NumberOfPasses == 4)
        {
            blurTex = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 16, Screen.height / 16, 0, source.format);
            var temp2 = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            Graphics.Blit(source, temp2, material, 0);
            Graphics.Blit(temp2, blurTex, material, 0);
            Graphics.Blit(blurTex, temp1, material, 0);
            Graphics.Blit(temp1, blurTex, material, 0);
            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
        }
        else if (NumberOfPasses == 5)
        {
            blurTex = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8, 0, source.format);
            var temp2 = RenderTexture.GetTemporary(Screen.width / 16, Screen.height / 16, 0, source.format);
            Graphics.Blit(source, blurTex, material, 0);
            Graphics.Blit(blurTex, temp1, material, 0);
            Graphics.Blit(temp1, temp2, material, 0);
            Graphics.Blit(temp2, temp1, material, 0);
            Graphics.Blit(temp1, blurTex, material, 0);
            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
        }

        material.SetTexture(blurTexString, blurTex);
        RenderTexture.ReleaseTemporary(blurTex);

        Graphics.Blit(source, destination, material, 1);
    }
}