using System.Collections.Generic;
using UnityEngine;

public class TailAnimatorDemo_InsectMover : FIMSpace.Basics.FBasics_RigidbodyMover
{
    [FPD_Header("References")]
    public List<Transform> WheelsFront;
    public List<Transform> WheelsBack;

    protected override void UpdateMotor()
    {
        base.UpdateMotor();

        Vector3 flatVelo = rigbody.velocity; flatVelo.y = 0f;
        for (int i = 0; i < WheelsFront.Count; i++)
            WheelsFront[i].Rotate(flatVelo.magnitude * 1.4f, 0f, 0f);

        for (int i = 0; i < WheelsBack.Count; i++)
        {
            WheelsBack[i].Rotate(flatVelo.magnitude * 1.4f, 0f, 0f);
            WheelsBack[i].Rotate(rigbody.angularVelocity.sqrMagnitude * .6f, 0f, 0f);
        }
    }
}
