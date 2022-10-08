using UnityEngine;
using System.Collections;

public class DetectHit : MonoBehaviour {
	[HideInInspector]public ForceField forceField;
	
	public void ApplyDamage(float damage){
		forceField.armor -= damage;
	}
	
	public void PointHit(Vector3 point){
		if(forceField.mesh!=null){
			foreach (MeshRenderer tMesh in forceField.mesh){
				tMesh.materials[forceField.i].SetVector("_Color", new Vector4(forceField.shieldColor.r, 
																				forceField.shieldColor.g, 
																				forceField.shieldColor.b, 
																				forceField.shieldColor.a+forceField.brightnessCollision));
			}
			foreach (MeshRenderer tMesh in forceField.mesh){
				tMesh.materials[forceField.i].SetVector("_Position", transform.InverseTransformPoint(point));
			}
		}
		forceField.mTime=1.0f;
		forceField.hit = true;
		forceField.hit2 = true;
	}
	
	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			if(forceField.mesh!=null){
	            foreach (MeshRenderer tMesh in forceField.mesh){
					tMesh.materials[forceField.i].SetVector("_Color", new Vector4(forceField.shieldColor.r, 
																					forceField.shieldColor.g, 
																					forceField.shieldColor.b, 
																					forceField.shieldColor.a+forceField.brightnessCollision));
				}
				foreach (MeshRenderer tMesh in forceField.mesh){
					tMesh.materials[forceField.i].SetVector("_Position", transform.InverseTransformPoint(contact.point));
				}
				forceField.mTime=1.0f;
				forceField.hit = true;
				forceField.hit2 = true;
			}
        }
	}
}
