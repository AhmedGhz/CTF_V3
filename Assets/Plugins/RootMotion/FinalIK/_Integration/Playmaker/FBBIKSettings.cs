using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages the general settings of a FullBodyBipedIK component.")]
	public class FBBIKSettings : IKAction {
		
		[HasFloatSlider(0, 1)]
		[Tooltip("Solver weight for smooth blending.")]
		public FsmFloat weight;
		
		[Tooltip("Solver iteration count.")]
		public FsmInt iterations;
		
		protected override void ResetAction() {
			weight = new FsmFloat { UseVariable = true };
			iterations = new FsmInt { UseVariable = true };

			iterations = 4;
		}
		
		protected override void UpdateAction() {
			var solver = (component as RootMotion.FinalIK.FullBodyBipedIK).solver;
			
			weight.Value = Mathf.Clamp(weight.Value, 0f, 1f);
			iterations.Value = Mathf.Clamp(iterations.Value, 0, 10);

			solver.IKPositionWeight = weight.Value;
			solver.iterations = iterations.Value;
		}
		
		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.FullBodyBipedIK);
		}
	}
}

