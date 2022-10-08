using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Controls the updating order of IK components")]
	public class IKExecutionOrder : FsmStateAction {
		
		[Tooltip("Does the Animation/Animator component of the character have animatePhysics checked or using UpdateMode.AnimatePhysics?")]
		public bool animatePhysics;

		[Tooltip("Update order of the IK components")]
		public IK[] IKComponents = new IK[0];
		
		private bool updateFrame;
		
		public override void Awake() {
			Fsm.HandleFixedUpdate = true;
		}
		
		public override void Reset() {
			updateFrame = false;
			IKComponents = new IK[0];
		}
		
		public override void OnEnter() {
			foreach (IK ik in IKComponents) ik.Disable();
		}
		
		public override void OnFixedUpdate() {
			updateFrame = true;
		}
		
		public override void OnLateUpdate() {
			if (animatePhysics && !updateFrame) return;
			
			Action();
			
			updateFrame = false;
		}
		
		private void Action() {
			foreach (IK ik in IKComponents) ik.GetIKSolver().Update();
		}
	}
}

