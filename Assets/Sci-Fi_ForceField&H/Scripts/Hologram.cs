using UnityEngine;
using System.Collections;

public class Hologram : MonoBehaviour {
	public MeshRenderer[] gameObject;
	public Vector4 speedOffset = new Vector4(0,0,0,0);

	private Vector4 offset = new Vector4(0,0,0,0);
	private Material[] material;
	private float camDistance;
	
	void Start(){
		material =  new Material[gameObject.Length];
		int i = 0;
		foreach (MeshRenderer tGameObject in gameObject){
			material[i] = tGameObject.material;
			i++;
		}
	}
	
	void Update () {
		camDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
		
		offset.x += Time.deltaTime*speedOffset.x;
		offset.y += Time.deltaTime*speedOffset.y;
		offset.z += Time.deltaTime*speedOffset.z;
		offset.w += Time.deltaTime*speedOffset.w;
		foreach (Material tMaterial in material){
			tMaterial.SetTextureOffset("_MainTex", new Vector2(offset.x, offset.y));
			tMaterial.SetTextureOffset("_NormalMap", new Vector2(offset.z, offset.w));
			tMaterial.SetFloat("_CamDistance", camDistance);
		}
	}
}
