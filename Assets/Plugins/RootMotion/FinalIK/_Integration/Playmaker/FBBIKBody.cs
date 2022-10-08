using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages a FullBodyBipedIK effector")]
	public class FBBIKBody : IKAction {

		[ActionSection("Body Effector")]

		[Tooltip("The target Transform (optional, you can use just the position instead).")]
		public FsmGameObject target;

		[Tooltip("Set the effector position to a point in world space. This has no effect if the effector's positionWeight is 0.")]
		public FsmVector3 position;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("The effector position weight.")]
		public FsmFloat positionWeight;
		
		[Tooltip("Offsets the hand from it's animated position. If effector positionWeight is 1, this has no effect. " +
		         "Note that the effectors will reset their positionOffset to Vector3.zero after each update, so you can (and should) use them additively. " +
		         "This enables you to easily edit the value by more than one script.")]
		public FsmVector3 positionOffset;

		[Tooltip("If false, child nodes will be ignored by this effector.")]
		public bool useThighs;

		[ActionSection("Chain")]

		[HasFloatSlider(0, 1)]
		[Tooltip("The bend resistance of the spine.")]
		public FsmFloat spineStiffness;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("Weight of hand effectors pulling the body vertically.")]
		public FsmFloat pullBodyVertical;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("Weight of hand effectors pulling the body horizontally.")]
		public FsmFloat pullBodyHorizontal;

		[ActionSection("Mapping")]

		[Tooltip("Spine mapping FABRIK iteration count.")]
		public FsmInt spineMappingIterations;

		[HasFloatSlider(0, 1)]
		[Tooltip("Weight of twisting the spine bones to the chest triangle.")]
		public FsmFloat spineTwistWeight;

		[HasFloatSlider(0, 1)]
		[Tooltip("The weight of maintaining the bone's animated rotation in world space.")]
		public FsmFloat maintainHeadRotation;

		protected override void ResetAction() {
			position = new FsmVector3 { UseVariable = true };
			positionWeight = new FsmFloat { UseVariable = true };
			positionOffset = new FsmVector3 { UseVariable = true };
			spineMappingIterations = new FsmInt { UseVariable = true };
			spineStiffness = new FsmFloat { UseVariable = true };
			pullBodyVertical = new FsmFloat { UseVariable = true };
			pullBodyHorizontal = new FsmFloat { UseVariable = true };
			maintainHeadRotation = new FsmFloat { UseVariable = true };
			spineTwistWeight = new FsmFloat { UseVariable = true };
			target = new FsmGameObject { UseVariable = true };

			useThighs = true;
			spineMappingIterations = 3;
			spineStiffness = 0.5f;
			pullBodyHorizontal = 0f;
			pullBodyVertical = 0.5f;
			spineTwistWeight = 1f;
		}
		
		protected override void UpdateAction() {
			var solver = (component as RootMotion.FinalIK.FullBodyBipedIK).solver;
			
			positionWeight.Value = Mathf.Clamp(positionWeight.Value, 0f, 1f);
			spineMappingIterations.Value = Mathf.Clamp(spineMappingIterations.Value, 1, int.MaxValue);
			spineStiffness.Value = Mathf.Clamp(spineStiffness.Value, 0f, 1f);
			pullBodyVertical.Value = Mathf.Clamp(pullBodyVertical.Value, 0f, 1f);
			pullBodyHorizontal.Value = Mathf.Clamp(pullBodyHorizontal.Value, 0f, 1f);
			maintainHeadRotation.Value = Mathf.Clamp(maintainHeadRotation.Value, 0f, 1f);
			spineTwistWeight.Value = Mathf.Clamp(spineTwistWeight.Value, 0f, 1f);

			solver.bodyEffector.target = target.Value == null? null: target.Value.transform;
			solver.bodyEffector.position = position.Value;
			solver.bodyEffector.positionWeight = positionWeight.Value;
			solver.bodyEffector.positionOffset = positionOffset.Value;
			solver.bodyEffector.effectChildNodes = useThighs;
			solver.spineMapping.iterations = spineMappingIterations.Value;
			solver.spineStiffness = spineStiffness.Value;
			solver.pullBodyVertical = pullBodyVertical.Value;
			solver.pullBodyHorizontal = pullBodyHorizontal.Value;
			solver.spineMapping.twistWeight = spineTwistWeight.Value;
			
			solver.boneMappings[0].maintainRotationWeight = maintainHeadRotation.Value;
		}
		
		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.FullBodyBipedIK);
		}
	}
}
