using UnityEngine;
using System.Collections;

/* Simple Physics Toolkit - ZeroGravity
 * Description: Controls enabling and disabling gravitry on all rigidbidbody objects that enter trigger
 * Required Components: Collider (All Types Supported) - Collider will control zero gravity area
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2016-01-16
*/
public class ZeroGravity : MonoBehaviour {

	public bool onlyAffectInteractableItems = false;

	void Start(){
		if (GetComponent<Collider> ()) {
			GetComponent<Collider> ().isTrigger = true; //Force trigger
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<Rigidbody> ()) {
			if (onlyAffectInteractableItems) {
				if (other.GetComponent<InteractableItem> ()) {
					other.GetComponent<Rigidbody>().useGravity = false;
					other.GetComponent<Rigidbody>().drag = 0.5f; //Reset Drag
				}
			} else {
				other.GetComponent<Rigidbody>().useGravity = false;
				other.GetComponent<Rigidbody>().drag = 0.5f; //Reset Drag
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.GetComponent<Rigidbody> ()) {
			if (onlyAffectInteractableItems) {
				if (other.GetComponent<InteractableItem> ()) {
					other.GetComponent<Rigidbody>().useGravity = true;
					other.GetComponent<Rigidbody>().drag = 0.0f; //Reset Drag
				}
			} else {
				other.GetComponent<Rigidbody>().useGravity = true;
				other.GetComponent<Rigidbody>().drag = 0.0f; //Reset Drag
			}
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.cyan;
		if (GetComponent<BoxCollider> ()) {
			Gizmos.DrawWireCube (transform.position, GetComponent<BoxCollider>().bounds.size);
		} else if (GetComponent<SphereCollider> ()) {
			SphereCollider c = GetComponent<SphereCollider>();
			Gizmos.DrawWireSphere(transform.position, c.radius);
		}
	}
}
