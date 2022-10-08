using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Base class for handling spline tracers")]
    public abstract class SplineTracerAction : SplineUserAction
    {
        public SplineTracer tracer
        {
            get { return _tracer; }
        }

        private SplineTracer _tracer = null;

        protected override bool OnOwnerUpdate()
        {
            if (base.OnOwnerUpdate())
            {
                if (user is SplineTracer)
                {
                    _tracer = (SplineTracer)user;
                    return true;
                }
                else
                {
                    _tracer = null;
                }
            }
            return false;
        }
    }
}
