using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Stops an interaction with the InteractionSystem")]
	public class StopInteraction : InteractionAction {

		protected override void Action(InteractionSystem sys) {
			foreach (FullBodyBipedEffector effectorType in effectorTypes) {
				sys.StopInteraction(effectorType);
			}
		}
	}
}
