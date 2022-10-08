using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pointer2 : MonoBehaviour {
	void Start(){Cursor.visible = false;}
	
	void Update () {
		this.gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
	}
}
