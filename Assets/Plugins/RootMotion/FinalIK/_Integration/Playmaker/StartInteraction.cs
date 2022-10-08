using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Starts an interaction with the InteractionSystem")]
	public class StartInteraction : InteractionAction {

		[RequiredField]
		[CheckForComponent(typeof(InteractionObject))]
		[Tooltip("The character with the InteractionSystem component")]
		public FsmOwnerDefault interactionObject;

		[Tooltip("Can this interaction interrupt an ongoing interaction?")]
		public FsmBool interrupt;

		public override void Reset() {
			base.Reset();

			interactionObject = null;
			interrupt = new FsmBool() { UseVariable = true };
		}

		protected override void Action(InteractionSystem sys) {
			var interactionGo = Fsm.GetOwnerDefaultTarget(interactionObject);
			if (interactionGo == null) return;

			var obj = interactionGo.GetComponent<InteractionObject>();
			if (obj == null) {
				Debug.LogWarning("No InteractionObject component found on " + interactionGo.name);
				return;
			}

			foreach (FullBodyBipedEffector effectorType in effectorTypes) {
				sys.StartInteraction(effectorType, obj, interrupt.Value);
			}
		}
	}
}
