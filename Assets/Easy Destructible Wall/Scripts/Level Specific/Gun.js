var forceMultiplier : int = 500;

function FixedUpdate() {
	if (Input.GetButton("Fire1")) {
		//Converts the position of the mouse cursor into a 3D represention in the scene
		var ray = camera.ScreenPointToRay(Input.mousePosition);
		var hit: RaycastHit;
		
		if(Physics.Raycast (ray, hit, 50.0)) {
			var hitObject = hit.rigidbody;
			if (hit.rigidbody) {
				var direction : Vector3 = hitObject.transform.position - transform.position;
	    		hitObject.AddForceAtPosition(direction.normalized * forceMultiplier, hit.point);
			}
		}
	}
}