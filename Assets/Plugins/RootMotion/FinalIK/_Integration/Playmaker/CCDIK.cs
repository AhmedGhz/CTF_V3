using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages the CCDIK component.")]
	public class CCDIK : IKAction {

		[System.Serializable]
		public class Solver {

			[Tooltip("The target Transform (optional, you can use just the position instead).")]
			public FsmGameObject target;

			[HasFloatSlider(0, 1)]
			[Tooltip("Solver weight for smooth blending.")]
			public FsmFloat weight;
			
			[Tooltip("The target position.")]
			public FsmVector3 position;

			public void Reset() {
				target = new FsmGameObject { UseVariable = true };
				weight = new FsmFloat { UseVariable = true };
				position = new FsmVector3 { UseVariable = true };
			}

			public void Update(IKSolverCCD solver) {
				weight.Value = Mathf.Clamp(weight.Value, 0f, 1f);

				solver.target = target.Value == null? null: target.Value.transform;
				solver.IKPositionWeight = weight.Value;
				solver.IKPosition = position.Value;
			}
		}

		public Solver solver = new Solver();
		
		protected override void ResetAction() {
			solver.Reset();
		}
		
		protected override void UpdateAction() {
			solver.Update((component as RootMotion.FinalIK.CCDIK).solver);
		}
		
		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.CCDIK);
		}
	}
}
