using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages the AimIK component.")]
	public class AimIK : IKAction {
		
		[System.Serializable]
		public class Solver {

			[Tooltip("The target Transform (optional, you can use just the position instead).")]
			public FsmGameObject target;

			[Tooltip("The pole target Transform (optional) - the position in world space to keep the pole axis of the Aim Transform directed at..")]
			public FsmGameObject poleTarget;

			[Tooltip("The transform that we want to aim at IKPosition.")]
			public FsmGameObject aimTransform;

			[Tooltip("The local axis of the Transform that you want to be aimed at IKPosition.")]
			public FsmVector3 axis;

			[Tooltip("Keeps that axis of the Aim Transform directed at the polePosition.")]
			public FsmVector3 poleAxis;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("Solver weight for smooth blending.")]
			public FsmFloat weight;

			[HasFloatSlider(0, 1)]
			[Tooltip("The weight of the Pole.")]
			public FsmFloat poleWeight;
			
			[Tooltip("Set the position to a point in world space that you want AimIK to aim the AimTransform at. This has no effect if the weight is 0.")]
			public FsmVector3 position;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("Clamping rotation of the solver. 0 is free rotation, 1 is completely clamped to transform axis.")]
			public FsmFloat clampWeight;

			public void Reset() {
				target = new FsmGameObject { UseVariable = true };
				poleTarget = new FsmGameObject { UseVariable = true };
				aimTransform = new FsmGameObject { UseVariable = true };
				axis = new FsmVector3 { UseVariable = true };
				poleAxis = new FsmVector3 { UseVariable = true };
				weight = new FsmFloat { UseVariable = true };
				poleWeight = new FsmFloat { UseVariable = true };
				position = new FsmVector3 { UseVariable = true };
				clampWeight = new FsmFloat { UseVariable = true };

				axis = Vector3.forward;
				poleAxis = Vector3.up;
				weight = 1f;
				poleWeight = 0f;
			}
			
			public void Update(IKSolverAim solver) {
				weight.Value = Mathf.Clamp(weight.Value, 0f, 1f);
				clampWeight.Value = Mathf.Clamp(clampWeight.Value, 0f, 1f);
				poleWeight.Value = Mathf.Clamp(poleWeight.Value, 0f, 1f);

				solver.target = target.Value == null? null: target.Value.transform;
				solver.poleTarget = poleTarget.Value == null? null: poleTarget.Value.transform;
				solver.transform = aimTransform.Value == null? null: aimTransform.Value.transform;
				solver.axis = axis.Value;
				solver.poleAxis = poleAxis.Value;
				solver.IKPositionWeight = weight.Value;
				solver.poleWeight = poleWeight.Value;
				solver.IKPosition = position.Value;
				solver.clampWeight = clampWeight.Value;
			}
		}

		public Solver solver = new Solver();

		protected override void ResetAction() {
			solver.Reset();
		}
		
		protected override void UpdateAction() {
			solver.Update((component as RootMotion.FinalIK.AimIK).solver);
		}

		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.AimIK);
		}
	}
}
