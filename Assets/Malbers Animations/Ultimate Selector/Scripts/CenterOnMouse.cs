using UnityEngine;

public class CenterOnMouse : MonoBehaviour {

	// Use this for initialization
	void Update () {

        MousePos();
    }

    void MousePos()
    {
        transform.position = Input.mousePosition;
    }
}
