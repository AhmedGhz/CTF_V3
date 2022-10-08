function OnGUI() {
	if(GUI.Button(Rect(5, 5, 140, 55), "Example Demo")) {
		Application.LoadLevel(0);
	}
	
	if(GUI.Button(Rect(155, 5, 140, 55), "Wall Type Showcase")) {
		Application.LoadLevel(1);
	}
}