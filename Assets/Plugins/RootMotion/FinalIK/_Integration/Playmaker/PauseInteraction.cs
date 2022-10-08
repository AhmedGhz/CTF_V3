using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Pauses an interaction with the InteractionSystem")]
	public class PauseInteraction : InteractionAction {

		protected override void Action(InteractionSystem sys) {
			foreach (FullBodyBipedEffector effectorType in effectorTypes) {
				sys.PauseInteraction(effectorType);
			}
		}
	}
}

