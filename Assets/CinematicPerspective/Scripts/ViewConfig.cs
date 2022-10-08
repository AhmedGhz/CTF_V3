using UnityEngine;

namespace CinematicPerspective
{
    public class ViewConfig : MonoBehaviour
    {
	    public Transform lookAtPoint;
	    public Transform driverView;

	    public float viewDistance = 10.0f;
	    public float viewHeight = 3.5f;
	    public float viewDamping = 3.0f;
	    public float viewMinDistance = 3.8f;
	    public float viewMinHeight = 0.0f;
    }
}