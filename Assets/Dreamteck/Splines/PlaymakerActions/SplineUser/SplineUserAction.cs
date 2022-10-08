using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [Tooltip("Base action for handling SplineUsers")]
    public abstract class SplineUserAction : DreamteckSplineAction
    {
        public SplineUser user
        {
            get { return _user; }
        }

        private SplineUser _user = null;

        protected override bool OnOwnerUpdate()
        {
            if(gameObject != null)
            {
                _user = gameObject.GetComponent<SplineUser>();
                if (_user == null)
                {
                    Debug.LogError("Missing object reference for state action " + DisplayName + " in " + Fsm.Name + " Action will not work.");
                }
            }
            return _user != null;
        }

        protected override void RunAction()
        {
            updateMethod = (UpdateMethod)(int)_user.updateMethod;
            base.RunAction();
        }
    }
}
