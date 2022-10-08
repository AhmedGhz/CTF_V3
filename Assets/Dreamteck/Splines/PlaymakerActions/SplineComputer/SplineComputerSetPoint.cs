using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline Computer")]
    [Tooltip("Sets the properties of a point from the spline computer")]
    public class SplineComputerSetPoint : SplineComputerAction
    {
        public FsmInt setPointIndex;
        public FsmBool additive;
        public FsmVector3 position;
        public FsmVector3 tangent;
        public FsmVector3 tangent2;
        public FsmVector3 normal;
        public FsmColor color;
        public FsmFloat size;

        public override void Reset()
        {
            base.Reset();
            additive = new FsmBool();
            additive.Tooltip = "If true, than the values will be addet to the point's values";
            setPointIndex = null;
            position = null;
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
            if(setPointIndex.Value < 0 || setPointIndex.Value >= spline.pointCount)
            {
                Debug.LogError("Set Point Index is out of Range. The referenced computer has " + spline.pointCount + " points. Accessing invalid index " + setPointIndex.Value);
                return;
            }
            SplinePoint point = spline.GetPoint(setPointIndex.Value);
            if (additive.Value)
            {
                if (!position.IsNone) point.position += position.Value;
                Vector3 tangent2Value = point.tangent2;
                if (!tangent.IsNone) point.SetTangentPosition(point.tangent + tangent.Value);
                if (!tangent2.IsNone) point.SetTangent2Position(tangent2Value + tangent2.Value);
                if (!normal.IsNone)
                {
                    point.normal += normal.Value;
                    point.normal.Normalize();
                }
                if (!color.IsNone) point.color += color.Value;
                if (!size.IsNone) point.size += size.Value;
            }
            else
            {
                if (!position.IsNone) point.position = position.Value;
                if (!tangent.IsNone) point.SetTangentPosition(tangent.Value);
                if (!tangent2.IsNone) point.SetTangent2Position(tangent2.Value);
                if (!normal.IsNone)
                {
                    point.normal = normal.Value;
                    point.normal.Normalize();
                }
                if (!color.IsNone) point.color = color.Value;
                if (!size.IsNone) point.size = size.Value;
            }
            spline.SetPoint(setPointIndex.Value, point);
        }
    }
}