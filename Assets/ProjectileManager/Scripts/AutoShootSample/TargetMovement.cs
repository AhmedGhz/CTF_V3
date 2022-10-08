using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public Vector3 start = new Vector3(-15, 3, -20);
    public Vector3 goal = new Vector3(15, 3, -20);
    public float speed = 2f;

    Vector3 temp;
    float time;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(start, goal);
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.time - time;
        if (Vector3.Distance(transform.position, goal) < 0.1f)
        {
            temp = start;
            start = goal;
            goal = temp;

            time = 0;
        }

        float presentLoc = (time * speed) / distance;
        transform.position = Vector3.Lerp(start, goal, presentLoc);
    }
}
