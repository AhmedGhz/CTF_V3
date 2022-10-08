// Fantasy Skybox version: 1.2.3
// Author: Gold Experience Team (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

#endregion

/***************
* FSPlayer class.
* 
* 	This class handles
* 		- Switches the Skybox
* 		- Updates Directional light
* 		- Updates Render Settings
* 		- Responds the Trigger events
* 		- Shows details on GUIs.
* 
* 	More info:
* 
* 		Skybox
* 		http://docs.unity3d.com/Documentation/Components/class-Skybox.html
* 		
* 		How do I Make a Skybox?
*		https://docs.unity3d.com/Documentation/Manual/HOWTO-UseSkybox.html
* 
* 		Render Settings
* 		http://docs.unity3d.com/Documentation/Components/class-RenderSettings.html
* 
* 		Directional light
* 		https://docs.unity3d.com/Documentation/Components/class-Light.html
* 
* 		Lights
* 		https://docs.unity3d.com/Documentation/Manual/Lights.html
* 
* 		Box Collider
* 		https://docs.unity3d.com/Documentation/Components/class-BoxCollider.html
* 
***************/

/***************
* Fog and Ambient colors reference.
* There are render settings and light preset pictures in Assets/Fantasy Skybox/Sunny/Presets folder

		"FS Night 01A" and "FS Night 01A noMoon"
			Fog Color: 30,50,100,255			Color(0.1176470588235294f,0.196078431372549f,0.392156862745098f,1f)
			Ambient Light: 35,35,35,255			Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f)

		"FS Night 01B" and "FS Night 01B noMoon"
			Fog Color: 20,40,100,255			Color(0.0784313725490196f,0.1568627450980392f,0.392156862745098f,1f)
			Ambient Light: 35,35,35,255			Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f)

		"FS Sunny 01A" and "FS Sunny 01A noSun"
			Fog Color: 110,180,255,255			Color(0.431372549f,0.705882353f,1f,1f)
			Ambient Light: 160,160,170,255		Color(0.62745098f,0.62745098f,0.666666667f,1f)

		"FS Sunny 01B" and "FS Sunny 01B noSun"
			Fog Color: 90,210,255,255			Color(0.352941176f,0.823529412f,1f,1f)
			Ambient Light: 160,180,180,255		Color(0.62745098f,0.705882353f,0.705882353f,1f)

		"FS Sunny 02A" and "FS Sunny 02A noSun"
			Fog Color: 190,230,230,255			Color(0.745098039f,0.901960784f,0.901960784f,1f)
			Ambient Light: 180,180,180,255		Color(0.705882353f,0.705882353f,0.705882353f,1f)

		"FS Sunny 02B" and "FS Sunny 02B noSun"
			Fog Color: 170,200,245,255			Color(0.666666667f,0.784313725f,0.960784314f,1f)
			Ambient Light: 180,180,180,255		Color(0.705882353f,0.705882353f,0.705882353f,1f)

		"FS Sunny 03A" and "FS Sunny 03A noSun"
			Fog Color: 125,200,245,255			Color(0.490196078f,0.784313725f,0.960784314f,1f)
			Ambient Light: 160,160,170,255		Color(0.62745098f,0.62745098f,0.666666667f,1f)

		"FS Sunny 03B" and "FS Sunny 03B noSun"
			Fog Color: 200,210,255,255			Color(0.784313725f,0.823529412f,1f,1f)
			Ambient Light: 140,150,180,255		Color(0.549019608f,0.588235294f,0.705882353f,1f)

		"FS Sunny 04A" and "FS Sunny 04A noSun"
			Fog Color: 230,255,255,255			Color(0.901960784f,1f,1f,1f)
			Ambient Light: 170,180,190,255		Color(0.666666667f,0.705882353f,0.745098039f,1f)

		"FS Sunny 04B" and "FS Sunny 04B noSun"
			Fog Color: 180,220,255,255			Color(0.705882353f,0.862745098f,1f,1f)
			Ambient Light: 190,190,200,255		Color(0.745098039f,0.745098039f,0.784313725f,1f)
					
		"FS Sunny 05A" and "FS Sunny 05A noSun"
			Fog Color: 160,230,255,255			Color(0.62745098f,0.901960784f,1f,1f)
			Ambient Light: 190,190,190,255		Color(0.745098039f,0.745098039f,0.745098039f,1f)

		"FS Sunny 05B" and "FS Sunny 05B noSun"
			Fog Color: 210,230,200,255			Color(0.823529412f,0.901960784f,0.784313725f,1f)
			Ambient Light: 190,190,170,255		Color(0.745098039f,0.745098039f,0.666666667f,1f) 

		"FS Sunny 06A" and "FS Sunny 06A noSun"
			Fog Color: 164,217,255,255			Color(0.62745098f,0.901960784f,1f,1f)
			Ambient Light: 130,130,130,255		Color(0.745098039f,0.745098039f,0.745098039f,1f)

		"FS Sunny 06B" and "FS Sunny 06B noSun"
			Fog Color: 164,217,255,255			Color(0.6431372549019608f,0.8509803921568627f,1f,1f)
			Ambient Light: 130,130,130,255		Color(0.5098039215686275f,0.5098039215686275f,0.5098039215686275f,1f)

* 
* ***************/

[System.Serializable]
public class LightAndSky
{
	// Name
	public string m_Name;

	// Light
	public Light m_Light;

	// Skybox
	public Material m_Skybox;

	// Fog
	public Color m_FogColor;

	// Ambient
	public Color m_AmbientLight;
}

public class FSPlayer : MonoBehaviour
{

#region Variables
	
	// LightAndSky
	public LightAndSky[] m_LightAndSkyList;

	// Index to current Skybox
	int m_CurrentSkyBox = 0;
	
#endregion {Variables}
	
// ######################################################################
// MonoBehaviour Functions
// ######################################################################

#region Component Segments

	// Use this for initialization
	void Start ()
	{
		/*
		// Fog Night
		m_LightAndSkyList[0].m_FogColor =	new Color(0.1176470588235294f,0.196078431372549f,0.392156862745098f,1f);
		m_LightAndSkyList[1].m_FogColor =	new Color(0.1176470588235294f,0.196078431372549f,0.392156862745098f,1f);
		m_LightAndSkyList[2].m_FogColor =	new Color(0.0784313725490196f,0.1568627450980392f,0.392156862745098f,1f);
		m_LightAndSkyList[3].m_FogColor =	new Color(0.0784313725490196f,0.1568627450980392f,0.392156862745098f,1f);

		// Fog Sunny
		m_LightAndSkyList[4].m_FogColor =	new Color(0.431372549f,0.705882353f,1f,1f);
		m_LightAndSkyList[5].m_FogColor =	new Color(0.431372549f,0.705882353f,1f,1f);
		m_LightAndSkyList[6].m_FogColor =	new Color(0.352941176f,0.823529412f,1f,1f);
		m_LightAndSkyList[7].m_FogColor =	new Color(0.352941176f,0.823529412f,1f,1f);
		m_LightAndSkyList[8].m_FogColor =	new Color(0.745098039f,0.901960784f,0.901960784f,1f);
		m_LightAndSkyList[9].m_FogColor =	new Color(0.745098039f,0.901960784f,0.901960784f,1f);
		m_LightAndSkyList[10].m_FogColor =	new Color(0.666666667f,0.784313725f,0.960784314f,1f);
		m_LightAndSkyList[11].m_FogColor =	new Color(0.666666667f,0.784313725f,0.960784314f,1f);
		m_LightAndSkyList[12].m_FogColor =	new Color(0.490196078f,0.784313725f,0.960784314f,1f);
		m_LightAndSkyList[13].m_FogColor =	new Color(0.490196078f,0.784313725f,0.960784314f,1f);
		m_LightAndSkyList[14].m_FogColor =	new Color(0.784313725f,0.823529412f,1f,1f);
		m_LightAndSkyList[15].m_FogColor =	new Color(0.784313725f,0.823529412f,1f,1f);
		m_LightAndSkyList[16].m_FogColor =	new Color(0.901960784f,1f,1f,1f);
		m_LightAndSkyList[17].m_FogColor =	new Color(0.901960784f,1f,1f,1f);
		m_LightAndSkyList[18].m_FogColor =	new Color(0.705882353f,0.862745098f,1f,1f);
		m_LightAndSkyList[19].m_FogColor =	new Color(0.705882353f,0.862745098f,1f,1f);
		m_LightAndSkyList[20].m_FogColor =	new Color(0.62745098f,0.901960784f,1f,1f);
		m_LightAndSkyList[21].m_FogColor =	new Color(0.62745098f,0.901960784f,1f,1f);
		m_LightAndSkyList[22].m_FogColor =	new Color(0.823529412f,0.901960784f,0.784313725f,1f);
		m_LightAndSkyList[23].m_FogColor =	new Color(0.823529412f,0.901960784f,0.784313725f,1f);
		m_LightAndSkyList[24].m_FogColor =	new Color(0.62745098f,0.901960784f,1f,1f);
		m_LightAndSkyList[25].m_FogColor =	new Color(0.62745098f,0.901960784f,1f,1f);
		m_LightAndSkyList[26].m_FogColor =	new Color(0.6431372549019608f,0.8509803921568627f,1f,1f);
		m_LightAndSkyList[27].m_FogColor =	new Color(0.6431372549019608f,0.8509803921568627f,1f,1f);


		// Ambient Night
		m_LightAndSkyList[0].m_AmbientLight =	new Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f);
		m_LightAndSkyList[1].m_AmbientLight =	new Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f);
		m_LightAndSkyList[2].m_AmbientLight =	new Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f);
		m_LightAndSkyList[3].m_AmbientLight =	new Color(0.1372549019607843f,0.1372549019607843f,0.1372549019607843f,1f);

		// Ambient Sunny
		m_LightAndSkyList[4].m_AmbientLight =	new Color(0.62745098f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[5].m_AmbientLight =	new Color(0.62745098f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[6].m_AmbientLight =	new Color(0.705882353f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[7].m_AmbientLight =	new Color(0.705882353f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[8].m_AmbientLight =	new Color(0.705882353f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[9].m_AmbientLight =	new Color(0.705882353f,0.705882353f,0.705882353f,1f);
		m_LightAndSkyList[10].m_AmbientLight =	new Color(0.62745098f,0.62745098f,0.666666667f,1f);
		m_LightAndSkyList[11].m_AmbientLight =	new Color(0.62745098f,0.62745098f,0.666666667f,1f);
		m_LightAndSkyList[12].m_AmbientLight =	new Color(0.549019608f,0.588235294f,0.705882353f,1f);
		m_LightAndSkyList[13].m_AmbientLight =	new Color(0.549019608f,0.588235294f,0.705882353f,1f);
		m_LightAndSkyList[14].m_AmbientLight =	new Color(0.666666667f,0.705882353f,0.745098039f,1f);
		m_LightAndSkyList[15].m_AmbientLight =	new Color(0.666666667f,0.705882353f,0.745098039f,1f);
		m_LightAndSkyList[16].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.784313725f,1f);
		m_LightAndSkyList[17].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.784313725f,1f);
		m_LightAndSkyList[18].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.745098039f,1f);
		m_LightAndSkyList[19].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.745098039f,1f);
		m_LightAndSkyList[20].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.666666667f,1f);
		m_LightAndSkyList[21].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.666666667f,1f);
		m_LightAndSkyList[22].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.666666667f,1f);
		m_LightAndSkyList[23].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.666666667f,1f);
		m_LightAndSkyList[24].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.745098039f,1f);
		m_LightAndSkyList[25].m_AmbientLight =	new Color(0.745098039f,0.745098039f,0.745098039f,1f);
		m_LightAndSkyList[26].m_AmbientLight =	new Color(0.5098039215686275f,0.5098039215686275f,0.5098039215686275f,1f);
		m_LightAndSkyList[27].m_AmbientLight =	new Color(0.5098039215686275f,0.5098039215686275f,0.5098039215686275f,1f);
		*/
		SwitchSkyBox(0);

		UpdateDetailsText();
		UpdateHowToText();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// User press Q key
		if(Input.GetKeyUp(KeyCode.Q))
		{
			SwitchSkyBox(-1);
			UpdateDetailsText();
		}
		// User press E key
		if(Input.GetKeyUp(KeyCode.E))
		{
			SwitchSkyBox(+1);
			UpdateDetailsText();
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		Debug.Log("OnTriggerExit="+other.name);
		
		// Reset player position when user move it away from terrain
		this.transform.localPosition = new Vector3(0,1,0);
    }
	
#endregion Component Segments
	
// ######################################################################
// Functions Functions
// ######################################################################

#region Functions

	void SwitchSkyBox(int DiffNum)
	{
		// update m_CurrentSkyBox
		m_CurrentSkyBox += DiffNum;
		
		// rounds m_CurrentSkyBox
		if(m_CurrentSkyBox<0)
		{
			m_CurrentSkyBox = m_LightAndSkyList.Length-1;
		}
		if(m_CurrentSkyBox>=m_LightAndSkyList.Length)
		{
			m_CurrentSkyBox = 0;
		}
		
		// switch skybox
		RenderSettings.skybox = m_LightAndSkyList[m_CurrentSkyBox].m_Skybox;
		
		// Set active/deactive lights
		for(int i=0;i<m_LightAndSkyList.Length;i++)
		{
			m_LightAndSkyList[i].m_Light.gameObject.SetActive(false);
		}
		m_LightAndSkyList[m_CurrentSkyBox].m_Light.gameObject.SetActive(true);
		
		// Enable fog
		RenderSettings.fog = true;
		
		// Set the fog color
		if(m_CurrentSkyBox>=0 && m_CurrentSkyBox<m_LightAndSkyList.Length)
		{
			RenderSettings.fogColor = m_LightAndSkyList[m_CurrentSkyBox].m_FogColor;
		}
		else
		{
			RenderSettings.fogColor = Color.white;
		}
		
		// Set the ambient lighting
		if(m_CurrentSkyBox>=0 && m_CurrentSkyBox<m_LightAndSkyList.Length)
		{
			RenderSettings.ambientLight = m_LightAndSkyList[m_CurrentSkyBox].m_AmbientLight;
		}
		else
		{
			RenderSettings.ambientLight = Color.white;
		}
	}
	
	// Update How to text
	void UpdateDetailsText()
	{
		// Update ItemNum text
		GameObject Text_ItemNum = GameObject.Find("Text_ItemNum");
		if(Text_ItemNum!=null)
		{ 				
			Text pText = Text_ItemNum.GetComponent<Text>();
			pText.text = string.Format("{0:00} of {1:00}",m_CurrentSkyBox+1, m_LightAndSkyList.Length);
		}

		// Update Details text
		GameObject Text_Details = GameObject.Find("Text_Details");
		if(Text_Details!=null)
		{ 				
			Text pText = Text_Details.GetComponent<Text>();
			pText.text = string.Format(m_LightAndSkyList[m_CurrentSkyBox].m_Name);
		}
	}

	// Update How to text
	void UpdateHowToText()
	{
		// Update How to text
		GameObject Text_HowTo = GameObject.Find("Text_HowTo");
		if(Text_HowTo!=null)
		{ 				
			Text pText = Text_HowTo.GetComponent<Text>();
			pText.text = "AWSD = Move | Spacebar = Jump | Mouse = Turn | Q/E = Prev/Next Skybox";
		}
	}

#endregion Functions
	
}
