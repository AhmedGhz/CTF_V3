using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public Transform targetParent;    

    public int index;

    public float speed = 1f;

    public float changeDistance = 3f;

    private Rigidbody _rigid;

    [Header("Debug")]
    public Transform target;

    void RollToSelection(Transform target)
    {
        _rigid.AddTorque(Vector3.Cross((target.position - transform.position).normalized, Vector3.down)*speed * Time.deltaTime, ForceMode.Force);
    }

	// Use this for initialization
	void Start () {
        _rigid = _rigid ?? GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        DoMove();
	}

    void DoMove()
    {
        //Avoiding out of range exceptions
        index = index % targetParent.childCount;

        //current target to move
        target = targetParent.GetChild(index);

        if (target == null)
        {
            _rigid.velocity *= 0;
            return;
        }

        RollToSelection(target);

        if (Vector3.Distance(transform.position, target.position) < changeDistance) index++;
    }
}
