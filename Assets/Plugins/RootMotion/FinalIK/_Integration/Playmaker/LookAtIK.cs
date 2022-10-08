using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages the LookAtIK component.")]
	public class LookAtIK : IKAction {

		[System.Serializable]
		public class Solver {

			[Tooltip("The target Transform (optional, you can use just the position instead).")]
			public FsmGameObject target;

			[HasFloatSlider(0, 1)]
			[Tooltip("Solver weight for smooth blending.")]
			public FsmFloat weight;
			
			[Tooltip("The target position.")]
			public FsmVector3 position;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("The weight multiplier for the spine bones.")]
			public FsmFloat bodyWeight;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("The weight multiplier for the head bone.")]
			public FsmFloat headWeight;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("The weight multiplier for the eyes")]
			public FsmFloat eyesWeight;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("Clamping rotation of the spine bones. 0 is free rotation, 1 is completely clamped.")]
			public FsmFloat clampWeight = 0.1f;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("Clamping rotation of the head bone. 0 is free rotation, 1 is completely clamped.")]
			public FsmFloat clampWeightHead = 0.1f;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("Clamping rotation of the eyes. 0 is free rotation, 1 is completely clamped.")]
			public FsmFloat clampWeightEyes = 0.1f;
			
			public void Reset() {
				target = new FsmGameObject { UseVariable = true };
				weight = new FsmFloat { UseVariable = true };
				position = new FsmVector3 { UseVariable = true };
				bodyWeight = new FsmFloat { UseVariable = true };
				headWeight = new FsmFloat { UseVariable = true };
				eyesWeight = new FsmFloat { UseVariable = true };
				clampWeight = new FsmFloat { UseVariable = true };
				clampWeightHead = new FsmFloat { UseVariable = true };
				clampWeightEyes = new FsmFloat { UseVariable = true };
				
				bodyWeight = 0.5f;
				headWeight = 0.5f;
				eyesWeight = 1f;
				clampWeight = 0.5f;
				clampWeightHead = 0.5f;
				clampWeightEyes = 0.5f;
			}
			
			public void Update(IKSolverLookAt solver) {
				weight.Value = Mathf.Clamp(weight.Value, 0f, 1f);
				bodyWeight.Value = Mathf.Clamp(bodyWeight.Value, 0f, 1f);
				headWeight.Value = Mathf.Clamp(headWeight.Value, 0f, 1f);
				eyesWeight.Value = Mathf.Clamp(eyesWeight.Value, 0f, 1f);
				clampWeight.Value = Mathf.Clamp(clampWeight.Value, 0f, 1f);
				clampWeightHead.Value = Mathf.Clamp(clampWeightHead.Value, 0f, 1f);
				clampWeightEyes.Value = Mathf.Clamp(clampWeightEyes.Value, 0f, 1f);

				solver.target = target.Value == null? null: target.Value.transform;
				solver.IKPositionWeight = weight.Value;
				solver.IKPosition = position.Value;
				solver.bodyWeight = bodyWeight.Value;
				solver.headWeight = headWeight.Value;
				solver.eyesWeight = eyesWeight.Value;
				solver.clampWeight = clampWeight.Value;
				solver.clampWeightHead = clampWeightHead.Value;
				solver.clampWeightEyes = clampWeightEyes.Value;
			}
		}

		public Solver solver = new Solver();
		
		protected override void ResetAction() {
			solver.Reset();
		}
		
		protected override void UpdateAction() {
			solver.Update((component as RootMotion.FinalIK.LookAtIK).solver);
		}

		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.LookAtIK);
		}
	}
}
