using UnityEngine;
using System.Collections;

public class CamControl2 : MonoBehaviour{
    public Transform target;
	public float height;
	public float distanceMin;
    public float distanceMax;
	public float xSpeed;
    public float ySpeed;
	
	private float distance;
	private float xAngle;
    private float yAngle;     
	
	void Update(){
		 if (Input.GetMouseButton(1)){
			xAngle -= Input.GetAxis("Mouse Y") * ySpeed;
            yAngle += Input.GetAxis("Mouse X") * xSpeed;  
			
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }else{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
			
		distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*10, distanceMin, distanceMax);	
		
		if(target){
			transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
			transform.position = new Vector3(0, height, 0) + target.position + transform.rotation * new Vector3(0, 0, -distance);
		}
	}
}
