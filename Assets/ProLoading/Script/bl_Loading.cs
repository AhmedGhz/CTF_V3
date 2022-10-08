using UnityEngine;
using System.Collections;

public class bl_Loading : MonoBehaviour {

    public int steps = 100; //Amount of textures to form the circle (higher = more rounded, but laggier).
    public float radius = 25f; //Radius of the circle (note that you may need to adjust size).
    public float rotationOffset = 180f; //Starts from top = 180, starts from bottom = 0.
    public Vector2 size = new Vector2(5f, 10f); //Size of the texture
    public Color color = new Color(255, 0, 0, 255); //Color of the circle/texture.
    public GUIStyle MidleTextFont;
    private int startSteps; //Cache the maximum value.
    public Vector2 anchor; //The anchoring/position of the circle
    public Texture2D m_Texture;
    private Texture2D tex; //Generated texture with 1x1 flat color.
    public bool AutoRotation = false;
    public float RotationSpeed = 10.0f;
    public float FadeSpeed = 3;
    private float DeafultFade;
    private static bl_Loading instance;
    public static bool Loading = false;//for default is not loading
    public bool ShowLabel = true;
    public bool CanShowLabel = true;
    public Place m_Place = Place.Center;
    public string MidleText = "Loading";
    private int InitialStep;


    public static bl_Loading Instance
    {

        get
        {
            return instance ?? (instance = GameObject.FindObjectOfType(typeof(bl_Loading)) as bl_Loading);
        }

    }

    void Start()
    {
        InitialStep = steps;
        steps = 100;
        startSteps = steps;
        DeafultFade = color.a;
        
        StartCoroutine(FixedStep());
        if (m_Texture == null)
        {
            tex = CreateTexture(color); //Never put it in loop
        }
    }

    //Moved calculations to the Update function rather than OnGUI function since it gets called multiple times per frame.
    void Update()
    {
        if (Loading)
        {
            steps = Mathf.Clamp(steps, 0, startSteps); //Make sure percentage stays within range (doesn't go past 100%).
            
            if (AutoRotation)
            {
                AutoRot();
            }
            if (color.a != 1.0f)
            {
                color.a = Mathf.Lerp(color.a, DeafultFade, Time.deltaTime * FadeSpeed);

            }
        }
        else
        {
            if (color.a > 0.0001)
            {
                color.a = Mathf.Lerp(color.a, 0.0f, Time.deltaTime * FadeSpeed);

            }
        }
    }


    void OnGUI()
    {
        if (Loading || color.a > 0.01)
        {
            if (ShowLabel )
            {
                GUI.color = new Color(MidleTextFont.normal.textColor.r, MidleTextFont.normal.textColor.g, MidleTextFont.normal.textColor.b, color.a);
                if (m_Place == Place.Center && radius > 35 )
                {
                    GUI.Label(new Rect(anchor.x - (MidleText.Length + 20), anchor.y, size.x, size.y), MidleText, MidleTextFont);
                    CanShowLabel = true;
                }
                else if (m_Place == Place.Right && size.x > 25)
                {
                    GUI.Label(new Rect(((anchor.x + size.x) + radius) - (MidleText.Length - size.x), anchor.y, size.x, size.y), MidleText, MidleTextFont);
                    CanShowLabel = true;
                }
                else if (m_Place == Place.Left && size.x > 25)
                {
                    GUI.Label(new Rect(((anchor.x - size.x) - radius) - ((MidleText.Length)* 5 + size.x), anchor.y, size.x, size.y), MidleText, MidleTextFont);
                    CanShowLabel = true;
                }
                else if (m_Place == Place.Top)
                {
                    GUI.Label(new Rect(anchor.x - (MidleText.Length + 20), ((anchor.y - size.x) - radius) - (size.y)/2, size.x, size.y), MidleText, MidleTextFont);
                    CanShowLabel = true;
                }
                else if (m_Place == Place.Down)
                {
                    GUI.Label(new Rect(anchor.x - (MidleText.Length + 20), ((anchor.y + size.x) + radius) + (size.y) / 2, size.x, size.y), MidleText, MidleTextFont);
                    CanShowLabel = true;
                }
                else
                {
                    CanShowLabel = false;
                }
            }
            GUIUtility.RotateAroundPivot(rotationOffset, anchor);

            for (int i = 0; i < steps; i++)
            {
                if (m_Texture == null)
                {
                    GUI.DrawTexture(new Rect(anchor.x - size.x, anchor.y - (size.y / 2) + radius, size.x, size.y), tex); //Draw texture
                }
                else
                {
                    GUI.color = color;
                    GUI.DrawTexture(new Rect(anchor.x - size.x, anchor.y - (size.y / 2) + radius, size.x, size.y), m_Texture); //Draw texture
                    GUI.color = Color.white;
                }
                GUIUtility.RotateAroundPivot((360f / startSteps), anchor); //Rotate to make it fit a circular shape
            }
            
        }
    }

    private Texture2D CreateTexture(Color32 col)
    {
        Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false); //Create simple texture for GUI.
        tex.SetPixel(0, 0, col); //Set flat color for texture
        tex.Apply(); //Apply texture 

        return tex;
     
    }
    void AutoRot()
    {
        rotationOffset += Time.deltaTime * RotationSpeed;
    }

    IEnumerator FixedStep()
    {
        yield return new WaitForSeconds(0.35f);
        steps = InitialStep;
    }
}
