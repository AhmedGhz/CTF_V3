using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Dreamteck Splines")]
    [Tooltip("Base class for handling spline tracers")]
    public abstract class SplineFollowerAction : SplineTracerAction
    {
        public SplineFollower follower
        {
            get { return _follower; }
        }

        private SplineFollower _follower = null;

        protected override bool OnOwnerUpdate()
        {
            if (base.OnOwnerUpdate())
            {
                if (user is SplineFollower)
                {
                    _follower = (SplineFollower)user;
                    return true;
                }
                else
                {
                    _follower = null;
                }
            }
            return false;
        }
    }
}
