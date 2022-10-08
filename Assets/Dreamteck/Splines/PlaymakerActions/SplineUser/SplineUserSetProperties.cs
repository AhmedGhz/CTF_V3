using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Sets the properties of a SplineUser component")]
    public class SplineUserSetProperties : SplineUserAction
    {
        public FsmBool setClipFrom = false;
        [HasFloatSlider(0f, 1f)]
        public FsmFloat clipFrom;

        public FsmBool setClipTo = false;
        [HasFloatSlider(0f, 1f)]
        public FsmFloat clipTo;


        public FsmBool setSpline = false;
        [CheckForComponent(typeof(SplineComputer))]
        public FsmGameObject spline;

        public FsmBool setUpdateMethod = false;
        public SplineUser.UpdateMethod userUpdateMethod;



        public override void Reset()
        {
            base.Reset();
            clipFrom = new FsmFloat();
            clipFrom.Value = 0f;
            clipTo = new FsmFloat();
            clipTo.Value = 1f;
            spline = null;
            updateMethod = UpdateMethod.Update;
        }

        protected override void RunAction()
        {
            base.RunAction();
            if (setClipFrom.Value) user.clipFrom = clipFrom.Value;
            if (setClipTo.Value) user.clipTo = clipTo.Value;
            if (setSpline.Value)
            {
                if (spline.Value == null)
                {
                    user.spline = null;
                }
                else
                {
                    user.spline = spline.Value.GetComponent<SplineComputer>();
                }
            }
            if (setUpdateMethod.Value)
            {
                user.updateMethod = userUpdateMethod;
                updateMethod = (UpdateMethod)(int)user.updateMethod;
            }

        }
    }
}
