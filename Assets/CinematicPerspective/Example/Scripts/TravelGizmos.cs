using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelGizmos : MonoBehaviour {

    private void OnDrawGizmos()
    {
        for (int i = 1; i <= transform.childCount; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.GetChild(i - 1).position, transform.GetChild(i % transform.childCount).position);
        }
    }
}
