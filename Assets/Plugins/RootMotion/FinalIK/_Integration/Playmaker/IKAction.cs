
using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	// The base abstract class for IK PlayMaker actions
	public abstract class IKAction : FsmStateAction {

		[ActionSection("Component")]

		[RequiredField]
		[Tooltip("The IK gameobject.")]
		public FsmOwnerDefault gameObject = new FsmOwnerDefault();
		
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame = true;

		protected Component component {
			get {
				if (gameObject == null) return null;
				go = Fsm.GetOwnerDefaultTarget(gameObject);
				if (go == null) return null; // This should not happen, but just in case

				// If gameobject has been switched out, need to find new component
				if (go != lastGo) _component = null;
				lastGo = go;

				if (_component == null) _component = go.GetComponent(GetComponentType());
				if (_component == null) {
					var componentType = GetComponentType().ToString();
					Debug.LogWarning("Component of type " + componentType + " was not found on " + go.name + ". Can't apply PlayMaker action.");
					return null;
				}
				return _component;
			}
		}
		private Component _component;
		private GameObject go;
		private GameObject lastGo;

		protected virtual void ResetAction() {} // Component might be missing
		protected abstract void UpdateAction(); // Component guaranteed
		protected abstract System.Type GetComponentType();
		
		public override void Reset() {
			gameObject = null;
			everyFrame = true;

			ResetAction();
		}
		
		public override void OnEnter() {
			UpdateActionSafe();
			
			if (!everyFrame) Finish();
		}
		
		public override void OnUpdate() {
			UpdateActionSafe();
		}

		private void UpdateActionSafe() {
			if (component == null) return;
			
			UpdateAction();
		}
	}
}
