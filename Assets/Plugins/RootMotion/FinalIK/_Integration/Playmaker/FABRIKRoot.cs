using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages the FABRIKRoot component.")]
	public class FABRIKRoot : IKAction {
		
		[System.Serializable]
		public class Solver {
			
			[HasFloatSlider(0, 1)]
			[Tooltip("Solver weight for smooth blending.")]
			public FsmFloat weight;

			[HasFloatSlider(0, 1)]
			[Tooltip("Clamping rotation of the solver. 0 is free rotation, 1 is completely clamped to transform axis.")]
			public FsmFloat rootPin;
			
			public void Reset() {
				weight = new FsmFloat { UseVariable = true };
				rootPin = new FsmFloat { UseVariable = true };
			}
			
			public void Update(IKSolverFABRIKRoot solver) {
				weight.Value = Mathf.Clamp(weight.Value, 0f, 1f);
				rootPin.Value = Mathf.Clamp(rootPin.Value, 0f, 1f);
				
				solver.IKPositionWeight = weight.Value;
				solver.rootPin = rootPin.Value;
			}
		}
		
		public Solver solver = new Solver();
		
		protected override void ResetAction() {
			solver.Reset();
		}
		
		protected override void UpdateAction() {
			solver.Update((component as RootMotion.FinalIK.FABRIKRoot).solver);
		}
		
		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.FABRIKRoot);
		}
	}
}

