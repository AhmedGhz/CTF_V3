using UnityEngine;

/*
 * Based on Unity standard asset Image Effects/Glow/GlowEffect.cs
 * 
 * 20150610: updated script to remove redundent allocations 
 */

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Heathen/Image Effects/Selective Glow")]
public class SelectiveGlow : MonoBehaviour
{
	public enum debugModeType
	{
		Normal,
		Glow,
		Mask
	}
	[Tooltip("Switch output to various stages of the effect for debugging")]
	public debugModeType debugMode = debugModeType.Normal;
	[Header("Configuration")]
	[Tooltip("Switch to the optimized blur shader, this should not be toggled while rendering e.g. at run time")]
    public bool UseFastBlur = false;
    /// <summary>
    /// The blur resolution.
    /// </summary>
	[Tooltip("Number of times to half the source image before processing")]
    [Range(1, 32)]
    public int SampleDivision = 4;
    /// <summary>
    /// The blur iterations.
    /// </summary>
	[Tooltip("Number of blur iterations to run before combining")]
    [Range(1, 32)]
    public int iterations = 5;
    /// <summary>
    /// The blur spread.
    /// </summary>
	[Tooltip("Distance to spread the blur")]
    [Range(0.0f, 2)]
    public float blurSpread = 0.6f;
    /// <summary>
    /// The global intensity.
    /// </summary>
	[Tooltip("Global strength of the glow effect")]
    [Range(0.0f, 64)]
    public float Intensity = 4f;
	[Header("Referance Shaders")]
    /// <summary>
    /// The target composite shader.
    /// </summary>
	[Tooltip("Combine the resulting glow map and the scene image")]
    public Shader compositeShader;
    /// <summary>
    /// The target render glow shader.
    /// </summary>
	[Tooltip("Replaces SelectiveGlow render types on render")]
    public Shader renderGlowShader;
    /// <summary>
    /// The blur iteration shader.
    /// Basically it just takes n texture samples and averages them.
    /// By applying it repeatedly and spreading out sample locations
    /// we get a Gaussian blur approximation.
    /// </summary>
	[Tooltip("Stanadrd cone tap blur")]
    public Shader blurShader;
    /// <summary>
    /// The fast blur shader.
    /// </summary>
	[Tooltip("Optimized aproximation blur")]
    public Shader fastBlurShader;
    /// <summary>
    /// The m_material.
    /// </summary>
    private Material m_Material;
    private Material GetMaterial()
    {
        if (m_Material == null || (Application.isEditor && !Application.isPlaying))
        {
            if (UseFastBlur)
                m_Material = new Material(fastBlurShader);
            else
                m_Material = new Material(blurShader);
            m_Material.hideFlags = HideFlags.HideAndDontSave;
        }
        return m_Material;
    }
    /// <summary>
    /// The m_composite material.
    /// </summary>
    private Material m_CompositeMaterial;
    private Material GetCompositeMaterial()
    {
        if (m_CompositeMaterial == null)
        {
            m_CompositeMaterial = new Material(compositeShader);
            m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        return m_CompositeMaterial;
    }

    private RenderTexture renderTexture;
    private GameObject shaderCamera;
    private Camera shaderCameraPointer;
    private Camera thisCamera;

    void OnDisable()
    {
        if (m_Material)
        {
            //DestroyImmediate( m_Material.shader );
            DestroyImmediate(m_Material);
        }
        DestroyImmediate(m_CompositeMaterial);
        DestroyImmediate(shaderCamera);
        if (renderTexture != null)
        {
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = null;
        }
    }

    void Start()
    {
        thisCamera = GetComponent<Camera>();
        if(thisCamera == null)
        {
            enabled = false;
            Debug.LogWarning("A camera component must be assigned to the GameObject when Selective Glow component is used.");
            return;
        }

        if (compositeShader == null)
        {
            enabled = false;
            Debug.LogWarning("Composite Shader is not assigned");
            return;
        }

        if (renderGlowShader == null)
        {
            enabled = false;
            Debug.LogWarning("Render Glow Shader is not assigned");
            return;
        }

        if (blurShader == null)
        {
            enabled = false;
            Debug.LogWarning("Blur Shader is not assigned");
            return;
        }

        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            Debug.LogWarning("Image Effects are not supported");
            return;
        }
        // Disable if the blur shader can't run on the users graphics card
        if (!GetMaterial().shader.isSupported)
        {
            enabled = false;
            Debug.LogWarning("Blur shader can't run on the users graphics card");
            return;
        }
    }

    void OnPreRender()
    {
        if (!enabled || !gameObject.activeSelf)
            return;

        //If in editor this may be null so get it from the gameObject
        if (thisCamera == null)
        {
            thisCamera = GetComponent<Camera>();
        }

        if (renderTexture != null)
        {
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = null;
        }

        renderTexture = RenderTexture.GetTemporary((int)(thisCamera.pixelWidth * (1 / thisCamera.rect.width)), (int)(thisCamera.pixelHeight * (1 / thisCamera.rect.height)), 16);

        if (!shaderCamera)
        {
            shaderCamera = new GameObject("ShaderCamera", typeof(Camera));
            shaderCamera.GetComponent<Camera>().enabled = false;
            shaderCamera.hideFlags = HideFlags.HideAndDontSave;
            shaderCamera.GetComponent<Camera>().rect = GetComponent<Camera>().rect;
        }

        if (shaderCameraPointer == null)
            shaderCameraPointer = shaderCamera.GetComponent<Camera>();

        //Refresh the shader camera values
        shaderCameraPointer.CopyFrom(thisCamera);
        shaderCameraPointer.rect = new Rect(0, 0, 1, 1);
        shaderCameraPointer.backgroundColor = new Color(0, 0, 0, 0);
        shaderCameraPointer.clearFlags = CameraClearFlags.SolidColor;
        shaderCameraPointer.targetTexture = renderTexture;
        shaderCameraPointer.RenderWithShader(renderGlowShader, "RenderType");
    }
    /// <summary>
    /// Performs 1 blur operation.
    /// </summary>
    /// <param name="source">Source.</param>
    /// <param name="dest">Destination.</param>
    /// <param name="iteration">Iteration.</param>
    private void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
        RenderTexture.active = dest; //BlitMultiTap does this for us
        source.SetGlobalShaderProperty("__RenderTex"); //For backwards support with manual blurs
        //Material mat = GetMaterial(); //Gets the blur cone shader to use

        if (UseFastBlur)
        {
            float widthMod = 1.0f / (1.0f * (1 << SampleDivision));
            float iterationOffs = (iteration * 1.0f);
            GetMaterial().SetVector("_Parameter", new Vector4(blurSpread * widthMod + iterationOffs, -blurSpread * widthMod - iterationOffs, 0.0f, 0.0f));
            RenderTexture rt2 = RenderTexture.GetTemporary(dest.width, dest.height, 0, dest.format);
            rt2.filterMode = FilterMode.Bilinear;
            dest.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, rt2, GetMaterial(), 1);
            Graphics.Blit(rt2, dest, GetMaterial(), 2);
            RenderTexture.ReleaseTemporary(rt2);
        }
        else
        {
            float off = 0.5f + (iteration * blurSpread);
            Graphics.BlitMultiTap(source, dest, GetMaterial(),
                               new Vector2(off, off), //Lower left
                               new Vector2(-off, off), //Lower right
                               new Vector2(off, -off), //Upper left
                               new Vector2(-off, -off)  //Upper right	
                               );
        }
    }

    /// <summary>
    /// Downsamples the texture to a n resolution.
    /// </summary>
    /// <param name="source">Source.</param>
    /// <param name="dest">Destination.</param>
    private void DownSample4x(RenderTexture source, RenderTexture dest)
    {
        RenderTexture.active = dest;
        source.SetGlobalShaderProperty("__RenderTex");
        //Material mat = GetMaterial();
        float off = 1.0f;
        if (UseFastBlur)
        {
            float widthMod = 1.0f / (1.0f * (1 << SampleDivision));
            GetMaterial().SetVector("_Parameter", new Vector4(blurSpread * widthMod, -blurSpread * widthMod, 0.0f, 0.0f));
            Graphics.Blit(source, dest, GetMaterial(), 0);
        }
        else
            Graphics.BlitMultiTap(source, dest, GetMaterial(),
                               new Vector2(off, off), //Lower left
                               new Vector2(-off, off), //Lower right
                               new Vector2(off, -off), //Upper left
                               new Vector2(-off, -off)  //Upper right	
                               );
    }

    // Called by the camera to apply the image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!enabled || !gameObject.activeSelf)
            return;
        int targetWidth, targetHeight;
        targetWidth = source.width / SampleDivision;
        targetHeight = source.height / SampleDivision;
        RenderTexture buffer = RenderTexture.GetTemporary(targetWidth, targetHeight, 0);

        //RenderTexture sharpBuffer = RenderTexture.GetTemporary(source.width, source.height, 0);				
        // Copy glow mask to the 4x4 smaller texture.
        DownSample4x(renderTexture, buffer);

        // Blur the small texture
        for (var i = 0; i < iterations; i++)
        {
            RenderTexture buffer2 = RenderTexture.GetTemporary(targetWidth, targetHeight, 0);
            FourTapCone(buffer, buffer2, i);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
        }

        //Material compositeMat = GetCompositeMaterial();
        GetCompositeMaterial().SetTexture("_BlurTex", buffer);
        GetCompositeMaterial().SetTexture("_BlurRamp", renderTexture);
        GetCompositeMaterial().SetFloat("_Outter", Intensity);
		GetCompositeMaterial ().SetFloat ("_Mode", debugMode == debugModeType.Normal ? 0f : debugMode == debugModeType.Glow ? 1f : 2f);

        //Updated for 4.2.3 Graphics call
        //ImageEffects.BlitWithMaterial(compositeMat, source, destination);
        Graphics.Blit(source, destination, GetCompositeMaterial());

        RenderTexture.ReleaseTemporary(buffer);

        if (renderTexture != null)
        {
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = null;
        }
    }
}
