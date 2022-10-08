using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages the TrigonometricIK component.")]
	public class TrigonometricIK : IKAction {

		[System.Serializable]
		public class Solver {

			[Tooltip("The target Transform (optional, you can use just the position and rotation instead).")]
			public FsmGameObject target;

			[HasFloatSlider(0, 1)]
			[Tooltip("Position weight for smooth blending.")]
			public FsmFloat positionWeight;
			
			[Tooltip("The target position.")]
			public FsmVector3 position;
			
			[HasFloatSlider(0, 1)]
			[Tooltip("Rotation weight for smooth blending.")]
			public FsmFloat rotationWeight;
			
			[Tooltip("The target rotation.")]
			public FsmQuaternion rotation;
			
			[Tooltip("The bend plane normal.")]
			public FsmVector3 bendNormal;
			
			public void Reset() {
				target = new FsmGameObject { UseVariable = true };
				positionWeight = new FsmFloat { UseVariable = true };
				position = new FsmVector3 { UseVariable = true };
				rotationWeight = new FsmFloat { UseVariable = true };
				rotation = new FsmQuaternion { UseVariable = true };
				bendNormal = new FsmVector3 { UseVariable = true };

				bendNormal = Vector3.right;
				rotation = Quaternion.identity;
			}
			
			public void Update(IKSolverTrigonometric solver) {
				positionWeight.Value = Mathf.Clamp(positionWeight.Value, 0f, 1f);
				rotationWeight.Value = Mathf.Clamp(rotationWeight.Value, 0f, 1f);

				solver.target = target.Value == null? null: target.Value.transform;
				solver.IKPositionWeight = positionWeight.Value;
				solver.IKPosition = position.Value;
				solver.IKRotationWeight = rotationWeight.Value;
				solver.IKRotation = rotation.Value;
				solver.bendNormal = bendNormal.Value.normalized;
			}
		}
		
		public Solver solver = new Solver();
		
		protected override void ResetAction() {
			solver.Reset();
		}
		
		protected override void UpdateAction() {
			solver.Update((component as RootMotion.FinalIK.TrigonometricIK).solver);
		}
		
		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.TrigonometricIK);
		}
		
	}
}

