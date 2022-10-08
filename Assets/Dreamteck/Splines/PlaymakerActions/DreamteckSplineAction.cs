using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [Tooltip("Base action for handling any kind of spline component")]
    public abstract class DreamteckSplineAction : FsmStateAction
    {
        public enum UpdateMethod { Update, FixedUpdate, LateUpdate, AllUpdate, None }

        public FsmOwnerDefault defaultOwner = null;
        public bool everyFrame = false;


        public GameObject gameObject
        {
            get { return _gameObject; }
        }

        private GameObject _gameObject = null;
        private FsmOwnerDefault _lastOwner = null;
        private bool validated = false;

        protected UpdateMethod updateMethod = UpdateMethod.Update;

        public override void Reset()
        {
            _gameObject = null;
            defaultOwner = null;
        }

        public override void Awake()
        {
            base.Awake();
            UpdateOwner();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            RunAction();
            if (!everyFrame)
            {
                Finish();
            }
        }

        private void UpdateOwner()
        {
            if (_lastOwner != defaultOwner)
            {
                if (defaultOwner != null)
                {
                    _gameObject = Fsm.GetOwnerDefaultTarget(defaultOwner);
                } 
                else
                {
                    _gameObject = null;
                }
                _lastOwner = defaultOwner;
                validated = OnOwnerUpdate();
            }
        }

        protected virtual bool OnOwnerUpdate()
        {
            return _gameObject != null;
        }

        protected virtual void RunAction()
        {
        }

        private void PerformUpdateLogic()
        {
            UpdateOwner();
            if (validated)
            {
                RunAction();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (everyFrame && (updateMethod == UpdateMethod.Update || updateMethod == UpdateMethod.AllUpdate || updateMethod == UpdateMethod.None))
            {
                PerformUpdateLogic();
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (everyFrame && (updateMethod == UpdateMethod.FixedUpdate || updateMethod == UpdateMethod.AllUpdate))
            {
                PerformUpdateLogic();
            }
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (everyFrame && (updateMethod == UpdateMethod.LateUpdate || updateMethod == UpdateMethod.AllUpdate))
            {
                PerformUpdateLogic();
            }
        }
    }
}
