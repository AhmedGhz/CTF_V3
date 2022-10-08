using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Resumes a paused interaction with the InteractionSystem")]
	public class ResumeInteraction : InteractionAction {

		protected override void Action(InteractionSystem sys) {
			foreach (FullBodyBipedEffector effectorType in effectorTypes) {
				sys.ResumeInteraction(effectorType);
			}
		}
	}
}


