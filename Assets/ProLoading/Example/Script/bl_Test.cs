using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class bl_Test : MonoBehaviour {

    public bl_Loading Circle;
    public GUISkin Skin;
    public Texture2D Vignette;
    private float m_step ;
    private float Screeny;
    private float Screenx;
    private bool ErrorPostion = false;
    private string ErrorPos = "Label Can not display this size";
    public List<Texture2D> m_Textures = new List<Texture2D>();
    private Vector2 m_scroll = Vector2.zero;
    private string FileName = "Test.txt";

    void Start()
    {
        m_step = Circle.steps;
        Circle.anchor.x = Screen.width / 2;
        Circle.anchor.y = Screen.height / 2;
        Screeny = Screen.height;
        Screenx = Screen.width;
        bl_Loading.Loading = true;

    }

    void OnGUI()
    {
        GUI.skin = Skin;
        ErrorPostion = Circle.CanShowLabel;
        if (!ErrorPostion)
        {
            GUI.Label(new Rect(Screen.width - 250, Screen.height - 45, 245, 30), ErrorPos);
        }
        GUILayout.BeginArea(new Rect(15, 25, 300, Screen.height));
        GUILayout.Label("Circle Step");
        GUILayout.BeginHorizontal();
        m_step = GUILayout.HorizontalSlider(m_step, 0.0f, 100.0f);
        GUILayout.Box(m_step.ToString("000"),GUILayout.Width(40));
        GUILayout.EndHorizontal();
        GUILayout.Label("Circle Radius");
        GUILayout.BeginHorizontal();
        Circle.radius = GUILayout.HorizontalSlider(Circle.radius, 0.0f, 250.0f);
        GUILayout.Box(Circle.radius.ToString("000"), GUILayout.Width(40));
        GUILayout.EndHorizontal();
        if (!Circle.AutoRotation)
        {
            GUILayout.Label("Circle Rotation");
            GUILayout.BeginHorizontal();
            Circle.rotationOffset = GUILayout.HorizontalSlider(Circle.rotationOffset, 0.0f, 1000.0f);
            GUILayout.Box(Circle.rotationOffset.ToString("0000"), GUILayout.Width(40));
            GUILayout.EndHorizontal();
        }
        GUILayout.Label("Circle Position (X,Y)");
        GUILayout.BeginHorizontal();
        Circle.anchor.x = GUILayout.HorizontalSlider(Circle.anchor.x, 1.0f, Screenx);
        Circle.anchor.y = GUILayout.HorizontalSlider(Circle.anchor.y, 1.0f, Screeny);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("X = " + Circle.anchor.x.ToString("000"));
        GUILayout.Box("Y = " + Circle.anchor.y.ToString("000"));
        GUILayout.EndHorizontal();
        GUILayout.Label("Circle Size (X,Y)");
        GUILayout.BeginHorizontal();
        Circle.size.x = GUILayout.HorizontalSlider(Circle.size.x, 1.0f, 300.0f);
        Circle.size.y = GUILayout.HorizontalSlider(Circle.size.y, 1.0f, 300.0f);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("X = "+ Circle.size.x.ToString("000"));
        GUILayout.Box("Y = " + Circle.size.y.ToString("000"));
        GUILayout.EndHorizontal();
        Circle.AutoRotation = GUILayout.Toggle(Circle.AutoRotation, "Auto Rotation");
        if (Circle.AutoRotation)
        {
            GUILayout.Label("Auto Rotation Speed");
            GUILayout.BeginHorizontal();
            Circle.RotationSpeed = GUILayout.HorizontalSlider(Circle.RotationSpeed,0.5f,10000.0f);
            GUILayout.Box(Circle.RotationSpeed.ToString("00000"), GUILayout.Width(50));
            GUILayout.EndHorizontal();
        }
        GUILayout.Label("Label Position");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Center","box"))
        {
            Circle.m_Place = Place.Center;
        }
        if (GUILayout.Button("Top", "box"))
        {
            Circle.m_Place = Place.Top;
        }
        if (GUILayout.Button("Down", "box"))
        {
            Circle.m_Place = Place.Down;
        }
        if (GUILayout.Button("Left", "box"))
        {
            Circle.m_Place = Place.Left;
        }
        if (GUILayout.Button("Right", "box"))
        {
            Circle.m_Place = Place.Right;
        }
        GUILayout.EndHorizontal();

        if (Circle.m_Texture != null)
        {
            GUILayout.Label("Circle Color (RGBA)");
            GUILayout.BeginHorizontal();
            Circle.color.r = GUILayout.HorizontalSlider(Circle.color.r, 0.0f, 1.0f);
            Circle.color.g = GUILayout.HorizontalSlider(Circle.color.g, 0.0f, 1.0f);
            Circle.color.b = GUILayout.HorizontalSlider(Circle.color.b, 0.0f, 1.0f);
            Circle.color.a = GUILayout.HorizontalSlider(Circle.color.a, 0.0f, 1.0f);
            GUILayout.EndHorizontal();
        }
        //This For test in Unity Editor
        #if UNITY_EDITOR
        GUILayout.Label("Read Prefabs File");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Read Botton Left"))
        {
            FileName = "BottonLeft.txt";
            ReadFile();
        }

        if (GUILayout.Button("Read Botton Right"))
        {
            FileName = "BottonRight.txt";
            ReadFile();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Read Top Right"))
        {
            FileName = "TopRight.txt";
            ReadFile();
        }
        GUILayout.EndHorizontal();
        #endif

        m_scroll = GUILayout.BeginScrollView(m_scroll, "box");
       GUILayout.BeginHorizontal();
        for (int i = 0; i < m_Textures.Count; i++)
        {
           
                if (GUILayout.Button(m_Textures[i], GUILayout.Width(50), GUILayout.Height(50)))
                {
                    Circle.m_Texture = m_Textures[i];
                }
            
           
        }
        GUILayout.Label("Select a Texture and change color");
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),Vignette);
        if (!bl_Loading.Loading)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height - 30, 150, 30), "Loading [On]"))
            {
                bl_Loading.Loading = !bl_Loading.Loading;
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height - 30, 150, 30), "Loading [Off]"))
            {
                bl_Loading.Loading = !bl_Loading.Loading;
            }
        }
        Circle.steps = (int)m_step;
    }

    void ReadFile()
    {
        StreamReader read = new StreamReader(Application.dataPath + "/ProLoading/Example/Present/" + FileName);
        string sr = read.ReadToEnd();
        read.Close();
       string[] lines = sr.Split("\n"[0]);

       foreach (string line in lines)
       {
           string[] t_get = line.Split(":"[0]);
           if (t_get[0] == "step")
           {
              Circle.steps = int.Parse(t_get[1]);
           }
           if (t_get[0] == "radius")
           {
               Circle.radius = float.Parse(t_get[1]);
           }
           if (t_get[0] == "posx")
           {
               Circle.anchor.x = float.Parse(t_get[1]);
           }
           if (t_get[0] == "posy")
           {
               Circle.anchor.y = float.Parse(t_get[1]);
           }
           if (t_get[0] == "sizex")
           {
               Circle.size.x = float.Parse(t_get[1]);
           }
           if (t_get[0] == "sizey")
           {
               Circle.size.y = float.Parse(t_get[1]);
           }
           if (t_get[0] == "speed")
           {
               Circle.RotationSpeed = float.Parse(t_get[1]);
           }
           if (t_get[0] == "place")
           {
               if ( int.Parse(t_get[1]) == 0)
               {
                   Circle.m_Place = Place.Center;
               }
               if (int.Parse(t_get[1]) == 1)
               {
                   Circle.m_Place = Place.Top;
               }
               if (int.Parse(t_get[1]) == 2)
               {
                   Circle.m_Place = Place.Down;
               }
               if (int.Parse(t_get[1]) == 3)
               {
                   Circle.m_Place = Place.Left;
               }
               if (int.Parse(t_get[1]) == 4)
               {
                   Circle.m_Place = Place.Right;
               }
           }
       }
    }
    
}
