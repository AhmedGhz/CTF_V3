using UnityEngine;
using System.Collections;

/* Simple Physics Toolkit - Destroy Objects
 * Description: Acts as a killzone for colliders. Any collider which enters trigger is destroyed. 
 *  			Nice for scene cleanup if projectiles stray outside of bounds
 * Required Components: Collider (Trigger)
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2017-04-29
*/
public class DestroyObjects : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		GameObject currentItem = col.gameObject;
		Destroy (currentItem);
	}
}
