using UnityEngine;
using System.Collections;

/* Simple Physics Toolkit - Wind
 * Description: Applies force to all rigidbodies within range
 * 				Rotate attached gameobject to change wind direction
 * 				Will not affect Unity WindZones
 * Required Components: Collider (All Types Supported) - Collider will control wind zone
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2016-01-16
*/
public class Wind : MonoBehaviour {
	
	public float lift = 30.0f;
	public bool onlyAffectInteractableItems = false;

	void Start(){
		if (GetComponent<Collider> ()) {
			GetComponent<Collider>().isTrigger = true; //Force Trigger
		}
		lift = lift / 100.0f;
	}
	
	void OnTriggerStay(Collider other){
		if(other.GetComponent<Rigidbody>()){
			if (onlyAffectInteractableItems) {
				if (other.GetComponent<InteractableItem> ()) {
					other.GetComponent<Rigidbody> ().AddForce (lift * transform.up, ForceMode.Impulse);
				}
			} else {
				other.GetComponent<Rigidbody> ().AddForce (lift * transform.up, ForceMode.Impulse);
			}
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine (transform.position, transform.position + transform.up * 1.0f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, 0.1f);
	}
}
