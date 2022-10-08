using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages the BipedIK component.")]
	public class BipedIK : IKAction {

		public HutongGames.PlayMaker.Actions.LimbIK.Solver leftFoot = new HutongGames.PlayMaker.Actions.LimbIK.Solver();
		public HutongGames.PlayMaker.Actions.LimbIK.Solver rightFoot = new HutongGames.PlayMaker.Actions.LimbIK.Solver();
		public HutongGames.PlayMaker.Actions.LimbIK.Solver leftHand = new HutongGames.PlayMaker.Actions.LimbIK.Solver();
		public HutongGames.PlayMaker.Actions.LimbIK.Solver rightHand = new HutongGames.PlayMaker.Actions.LimbIK.Solver();
		public HutongGames.PlayMaker.Actions.FABRIK.Solver spine = new HutongGames.PlayMaker.Actions.FABRIK.Solver();
		public HutongGames.PlayMaker.Actions.AimIK.Solver aim = new HutongGames.PlayMaker.Actions.AimIK.Solver();
		public HutongGames.PlayMaker.Actions.LookAtIK.Solver lookAt = new HutongGames.PlayMaker.Actions.LookAtIK.Solver();

		[Tooltip("Pelvis offset from animation. If there is no animation playing and Fix Transforms is unchecked, it will make the character fly away.")]
		public FsmVector3 pelvisPositionOffset;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("Weight of lerping the pelvis to pelvisPosition")]
		public FsmFloat pelvisPositionWeight;
		
		[Tooltip("The position to lerp the pelvis to by pelvisPositionWeight.")]
		public FsmVector3 pelvisPosition;

		[Tooltip("Pelvis rotation offset from animation. If there is no animation playing and Fix Transforms is unchecked, it will make the character spin.")]
		public FsmVector3 pelvisRotationOffset;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("Weight of slerping the pelvis to pelvisRotation")]
		public FsmFloat pelvisRotationWeight;
		
		[Tooltip("The rotation to lerp the pelvis to by pelvisRotationWeight")]
		public FsmVector3 pelvisRotation;
		
		protected override void ResetAction() {
			leftFoot.Reset();
			rightFoot.Reset();
			leftHand.Reset();
			rightHand.Reset();
			spine.Reset();
			aim.Reset();
			lookAt.Reset();

			pelvisPositionOffset = new FsmVector3 { UseVariable = true };
			pelvisPositionWeight = new FsmFloat { UseVariable = true };
			pelvisPosition = new FsmVector3 { UseVariable = true };
			pelvisRotationOffset = new FsmVector3 { UseVariable = true };
			pelvisRotationWeight = new FsmFloat { UseVariable = true };
			pelvisRotation = new FsmVector3 { UseVariable = true };

			if (component != null) {
				var solvers = (component as RootMotion.FinalIK.BipedIK).solvers;
				pelvisPosition = solvers.pelvis.position;
				pelvisRotation = solvers.pelvis.rotation;
			}
		}
		
		protected override void UpdateAction() {
			var solvers = (component as RootMotion.FinalIK.BipedIK).solvers;

			leftFoot.Update(solvers.leftFoot);
			rightFoot.Update(solvers.rightFoot);
			leftHand.Update(solvers.leftHand);
			rightHand.Update(solvers.rightHand);
			spine.Update(solvers.spine);
			aim.Update(solvers.aim);
			lookAt.Update(solvers.lookAt);

			pelvisPositionWeight.Value = Mathf.Clamp(pelvisPositionWeight.Value, 0f, 1f);
			pelvisRotationWeight.Value = Mathf.Clamp(pelvisRotationWeight.Value, 0f, 1f);

			solvers.pelvis.positionOffset = pelvisPositionOffset.Value;
			solvers.pelvis.positionWeight = pelvisPositionWeight.Value;
			solvers.pelvis.position = pelvisPosition.Value;
			solvers.pelvis.rotationOffset = pelvisRotationOffset.Value;
			solvers.pelvis.rotationWeight = pelvisRotationWeight.Value;
			solvers.pelvis.rotation = pelvisRotation.Value;
		}
		
		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.BipedIK);
		}
	}
}
