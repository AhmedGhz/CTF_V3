using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	// The base abstract class for all Interaction System related actions
	public abstract class InteractionAction : FsmStateAction {

		protected abstract void Action(InteractionSystem sys);

		[RequiredField]
		[CheckForComponent(typeof(InteractionSystem))]
		[CheckForComponent(typeof(FullBodyBipedIK))]
		[Tooltip("The character with the InteractionSystem component")]
		public FsmOwnerDefault interactionSystem;
		
		[Tooltip("The effector(s) to use for the interaction")]
		public FullBodyBipedEffector[] effectorTypes;
		
		public override void Reset() {
			interactionSystem = null;
			effectorTypes = new FullBodyBipedEffector[0];
		}
		
		public override void OnEnter() {
			var go = Fsm.GetOwnerDefaultTarget(interactionSystem);
			if (go == null) return;
			
			var sys = go.GetComponent<InteractionSystem>();
			if (sys == null) {
				Debug.LogWarning("No InteractionSystem component found on " + go.name);
				return;
			}

			Action(sys);

			Finish();
		}
	}
}

