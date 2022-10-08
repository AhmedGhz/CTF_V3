using UnityEngine;
using System.Collections;

public class TestBox2 : MonoBehaviour {
	public float damage;
	
	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			contact.otherCollider.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);	
			contact.otherCollider.gameObject.SendMessage("PointHit", contact.point, SendMessageOptions.DontRequireReceiver);
        }
		Destroy(this.gameObject);
	}

}
