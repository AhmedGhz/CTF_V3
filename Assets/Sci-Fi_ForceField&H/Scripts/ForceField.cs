using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {
	public GameObject[] underShield;
	public Material materialShield;
	public float brightnessCollision = 5.0f;
	public float fadingGlow = 1.0f;
	public float armor = 100;
	public float speedOnOff = 10.0f;
	public Vector4 speedOffset = new Vector4(0,0,0,0);
	public bool sphere = false;
	public float sphereScale = 1.0f;
	public Vector3 spherePosition = new Vector3(0, 0, 0);
	
	[HideInInspector]public float mTime;	
	[HideInInspector]public Color shieldColor;
	[HideInInspector]public bool hit = false;
	[HideInInspector]public bool hit2 = false;	
	[HideInInspector]public MeshRenderer[] mesh;
	[HideInInspector]public int i = 0;
	private float shieldA = 0.0f;
	private GameObject sp;
	private bool activ = false;
	private Vector4 offset = new Vector4(0,0,0,0);
 
	void Start (){
		CreateForceField();
	}
	
	void Update (){
		if(armor>0){
			if(activ == false){CreateForceField();}
			if(shieldColor.a<shieldA){	
				if(mesh!=null){foreach (MeshRenderer tMesh in mesh){tMesh.materials[i].SetVector("_Color", shieldColor);}}
				shieldColor.a+=Time.deltaTime*speedOnOff;						
			}else if(shieldColor.a>shieldA){
				shieldColor.a = shieldA;
				if(mesh!=null){foreach (MeshRenderer tMesh in mesh){tMesh.materials[i].SetVector("_Color", shieldColor);}}
			}				
		}else{
			if(shieldColor.a>0.0f){
				if(mesh!=null){foreach (MeshRenderer tMesh in mesh){tMesh.materials[i].SetVector("_Color", shieldColor);}}
				shieldColor.a-=Time.deltaTime*speedOnOff;					
			}else if(shieldColor.a<0.0f){
				shieldColor.a = 0.0f;
				if(mesh!=null){foreach (MeshRenderer tMesh in mesh){tMesh.materials[i].SetVector("_Color", shieldColor);}}
				if(activ == true){DestroyForceField();}
			}				
		}
		
		if(mesh!=null){
			offset.x += Time.deltaTime*speedOffset.x;
			offset.y += Time.deltaTime*speedOffset.y;
			offset.z += Time.deltaTime*speedOffset.z;
			offset.w += Time.deltaTime*speedOffset.w;
			foreach (MeshRenderer tMesh in mesh){
				tMesh.materials[i].SetTextureOffset("_MainTex", new Vector2(offset.x, offset.y));
				tMesh.materials[i].SetTextureOffset("_NormalMap", new Vector2(offset.z, offset.w));
			}
		}
		
		if(hit == true && mTime < 0.9f){		
			if(mesh!=null){foreach (MeshRenderer tMesh in mesh){tMesh.materials[i].SetVector("_Color", shieldColor);}}											
			hit = false;

		}    
		
		if (hit2 == true){
			if(mTime<0.0f){
				mTime = 0.0f;
				if(mesh!=null){foreach (MeshRenderer tMesh in mesh){tMesh.materials[i].SetFloat("_EffectTime", mTime);}}
				hit2 = false;
			}else{
				mTime-=Time.deltaTime * fadingGlow;					
				if(mesh!=null){foreach (MeshRenderer tMesh in mesh){tMesh.materials[i].SetFloat("_EffectTime", mTime);}}
			}
		}
	}
	
	public void CreateForceField(){
		if (sphere==false){
			mesh = new MeshRenderer[underShield.Length];
			int j=0;
			foreach (GameObject tUnderShield in underShield){				
				mesh[j] = tUnderShield.GetComponent<MeshRenderer>();
				Material[] m = mesh[j].materials;
				Material[] m2 = new Material[m.Length+1];
				i=0;
				foreach (Material tM in m){
					m2[i] = tM;
					i++;
				}
				m2[i] = materialShield;
				mesh[j].materials = m2;
				j++;
				
				DetectHit detectHit = tUnderShield.AddComponent<DetectHit>();
				detectHit.forceField = this;
			}
		}else{
			sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sp.name = "ForceField";
			sp.transform.parent = this.transform;
        	sp.transform.localPosition = spherePosition;
			sp.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
			i=0;
			mesh = new MeshRenderer[1];
			mesh[0] = sp.GetComponent<MeshRenderer>();
			mesh[0].material = materialShield;
			mesh[0].material.SetFloat("_MeshOffset", 0.0f);
			
			DetectHit detectHit = sp.AddComponent<DetectHit>();
			detectHit.forceField = this;
		}
		
		mTime = 0.0f;
		foreach (MeshRenderer tMesh in mesh){shieldColor = tMesh.materials[i].GetVector("_Color");}
		shieldA = shieldColor.a;
		shieldColor.a = 0.0f;
		foreach (MeshRenderer tMesh in mesh){tMesh.materials[i].SetVector("_Color", shieldColor);}
		hit = false;
		hit2 = false;
		activ = true;
	}
	
	public void DestroyForceField(){
		if (sphere==false){
			int j=0;
			foreach (GameObject tUnderShield in underShield){
				Material[] m = mesh[j].materials;
				Material[] m2 = new Material[m.Length-1];
				i=0;
				foreach (Material tM in m){
					if(i<m2.Length){m2[i] = tM;}
					i++;
				}
				mesh[j].materials = m2;
				j++;
				
				GameObject.Destroy(tUnderShield.GetComponent<DetectHit>());				
			}
			mesh = null;
		}else{			
			Destroy(sp);
			mesh = null;
		}
		
		mTime = 0.0f;
		shieldA = 0.0f;
		shieldColor = new Color(0,0,0,0);
		hit = false;
		hit2 = false;
		activ = false;
	}
	
//	void OnCollisionEnter(Collision collision) {
//		foreach (ContactPoint contact in collision.contacts) {
//			if(forceField.mesh!=null){
//	            foreach (MeshRenderer tMesh in mesh){
//					tMesh.materials[i].SetVector("_Color", new Vector4(shieldColor.r, 
//													shieldColor.g, 
//													shieldColor.b, 
//													shieldColor.a+brightnessCollision));
//				}
//				foreach (MeshRenderer tMesh in mesh){
//					tMesh.materials[i].SetVector("_Position", transform.InverseTransformPoint(contact.point));
//				}
//			}
//			mTime=1.0f;
//			hit = true;
//			hit2 = true;
//        }
//	}
}
