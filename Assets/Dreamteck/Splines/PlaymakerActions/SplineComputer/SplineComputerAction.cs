using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline Computer")]
    [Tooltip("Base action for handling SplineComputers")]
    public abstract class SplineComputerAction : DreamteckSplineAction
    {
        public SplineComputer spline
        {
            get { return _spline; }
        }

        private SplineComputer _spline = null;

        protected override bool OnOwnerUpdate()
        {
            if (gameObject != null)
            {
                _spline = gameObject.GetComponent<SplineComputer>();
                if (_spline == null)
                {
                    Debug.LogError("Missing object reference for state action " + DisplayName + " in " + Fsm.Name + " Action will not work.");
                }
            }
            return _spline != null;
        }

        protected override void RunAction()
        {
            updateMethod = (UpdateMethod)(int)_spline.updateMode;
            base.RunAction();
        }
    }
}
