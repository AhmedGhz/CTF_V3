using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline Computer")]
    [Tooltip("Gets the properties of a point from the spline computer")]
    public class SplineComputerGetPoint : SplineComputerAction
    {
        public FsmInt getPointIndex;
        public FsmVector3 position;
        public FsmVector3 tangent;
        public FsmVector3 tangent2;
        public FsmVector3 normal;
        public FsmColor color;
        public FsmFloat size;

        public override void Reset()
        {
            base.Reset();
            getPointIndex = null;
            position = null;
            tangent = null;
            tangent2 = null;
            normal = null;
            color = null;
            size = null;
        }

        protected override void RunAction()
        {
            base.RunAction();
            if(getPointIndex.Value < 0 || getPointIndex.Value >= spline.pointCount)
            {
                Debug.LogError("Get Point Index is out of Range. The referenced computer has " + spline.pointCount + " points. Accessing invalid index " + getPointIndex.Value);
                return;
            }
            SplinePoint point = spline.GetPoint(getPointIndex.Value);
            position.Value = point.position;
            tangent.Value = point.tangent;
            tangent2.Value = point.tangent2;
            normal.Value = point.normal;
            color.Value = point.color;
            size.Value = point.size;
        }
    }
}