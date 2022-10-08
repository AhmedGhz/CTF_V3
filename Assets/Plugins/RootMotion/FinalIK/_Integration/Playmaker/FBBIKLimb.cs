using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages a FullBodyBipedIK limb. You can alternately use FBBIKEffector and FBBIKChain and FBBIKMapping.")]
	public class FBBIKLimb : IKAction {

		[ActionSection("Chain")]
		
		[Tooltip("The FBBIK chain type.")]
		public FullBodyBipedChain limb;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("When all chains have pull equal to 1, pull weight is distributed equally between the limbs. " +
		         "That means reaching all effectors is not quaranteed if they are very far from each other. " +
		         "However, when for instance the left arm chain has pull weight equal to 1 and all others have 0, you can pull the character from it's left hand to Infinity without losing contact.")]
		public FsmFloat pull;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("Increasing this value will make the limb pull the body closer to the target.")]
		public FsmFloat reach;

		[HasFloatSlider(0, 1)]
		[Tooltip("The weight of the end-effector pushing the first node.")]
		public FsmFloat push;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("The amount of push force transferred to the parent (from hand or foot to the body).")]
		public FsmFloat pushParent;
		
		[Tooltip("Smoothing the effect of the reach with the expense of some accuracy.")]
		public FBIKChain.Smoothing reachSmoothing;

		[Tooltip("Smoothing the effect of the Push.")]
		public FBIKChain.Smoothing pushSmoothing;

		[Tooltip("The bend goal GameObject. The limb will be bent in the direction towards this GameObject.")]
		public FsmGameObject bendGoal;

		[Tooltip("The weight of bending the limb towards the Bend Goal.")]
		public FsmFloat bendGoalWeight;

		[ActionSection("End Effector (Hand/Foot)")]

		[Tooltip("The target Transform (optional, you can use just the position and rotation instead).")]
		public FsmGameObject target;

		[Tooltip("Set the effector position to a point in world space. This has no effect if the effector's positionWeight is 0.")]
		public FsmVector3 position;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("The effector position weight.")]
		public FsmFloat positionWeight;
		
		[Tooltip("The effector rotation, this only an effect with limb end-effectors (hands and feet).")]
		public FsmQuaternion rotation;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("Weighing in the effector rotation, this only an effect with limb end-effectors (hands and feet).")]
		public FsmFloat rotationWeight;
		
		[Tooltip("Offsets the hand from it's animated position. If effector positionWeight is 1, this has no effect. " +
		         "Note that the effectors will reset their positionOffset to Vector3.zero after each update, so you can (and should) use them additively. " +
		         "This enables you to easily edit the value by more than one script.")]
		public FsmVector3 positionOffset;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("Keeps the node position relative to the triangle defined by the plane bones (applies only to end-effectors).")]
		public FsmFloat maintainRelativePositionWeight;

		[ActionSection("Start Effector (Shoulder/Thigh)")]

		[Tooltip("The target Transform (optional, you can use just the position and rotation instead).")]
		public FsmGameObject startEffectorTarget;

		[Tooltip("Set the effector position to a point in world space. This has no effect if the effector's positionWeight is 0.")]
		public FsmVector3 startEffectorPosition;
		
		[HasFloatSlider(0, 1)]
		[Tooltip("The effector position weight.")]
		public FsmFloat startEffectorPositionWeight;
		
		[Tooltip("Offsets the hand from it's animated position. If effector positionWeight is 1, this has no effect. " +
		         "Note that the effectors will reset their positionOffset to Vector3.zero after each update, so you can (and should) use them additively. " +
		         "This enables you to easily edit the value by more than one script.")]
		public FsmVector3 startEffectorPositionOffset;

		[ActionSection("Mapping")]

		[HasFloatSlider(0, 1)]
		[Tooltip("The slerp weight of rotating the limb to it's IK pose. This can be useful if you want to disable the effect of IK for the limb or move the hand to the target in a sperical trajectory instead of linear.")]
		public FsmFloat mappingWeight;

		[HasFloatSlider(0, 1)]
		[Tooltip("The weight of maintaining the bone's animated rotation in world space.")]
		public FsmFloat maintainRotationWeight;

		protected override void ResetAction() {
			limb = FullBodyBipedChain.LeftArm;
			pull = new FsmFloat { UseVariable = true };
			reach = new FsmFloat { UseVariable = true };
			push = new FsmFloat { UseVariable = true };
			pushParent = new FsmFloat { UseVariable = true };
			bendGoal = new FsmGameObject { UseVariable = true };
			bendGoalWeight = new FsmFloat { UseVariable = true };

			target = new FsmGameObject { UseVariable = true };
			position = new FsmVector3 { UseVariable = true };
			positionWeight = new FsmFloat { UseVariable = true };
			rotation = new FsmQuaternion { UseVariable = true };
			rotationWeight = new FsmFloat { UseVariable = true };
			positionOffset = new FsmVector3 { UseVariable = true };
			maintainRelativePositionWeight = new FsmFloat { UseVariable = true };

			startEffectorTarget = new FsmGameObject { UseVariable = true };
			startEffectorPosition = new FsmVector3 { UseVariable = true };
			startEffectorPositionWeight = new FsmFloat { UseVariable = true };
			startEffectorPositionOffset = new FsmVector3 { UseVariable = true };

			maintainRotationWeight = new FsmFloat { UseVariable = true };
			mappingWeight = new FsmFloat { UseVariable = true};
			reachSmoothing = FBIKChain.Smoothing.Exponential;
			pushSmoothing = FBIKChain.Smoothing.Exponential;

			rotation = Quaternion.identity;
			mappingWeight = 1f;
			pull = 1f;
			reach = 0.05f;
			push = 0f;
			pushParent = 0f;
			bendGoalWeight = 0f;
		}

		protected override void UpdateAction() {
			var solver = (component as RootMotion.FinalIK.FullBodyBipedIK).solver;
			
			var effector = solver.GetEndEffector(limb);
			var chain = solver.GetChain(limb);
			var mapping = solver.GetLimbMapping(limb);
			var startEffector = solver.GetEffector(GetStartEffector(limb));

			pull.Value = Mathf.Clamp(pull.Value, 0f, 1f);
			reach.Value = Mathf.Clamp(reach.Value, 0f, 1f);
			push.Value = Mathf.Clamp(push.Value, 0f, 1f);
			pushParent.Value = Mathf.Clamp(pushParent.Value, -1f, 1f);

			positionWeight.Value = Mathf.Clamp(positionWeight.Value, 0f, 1f);
			rotationWeight.Value = Mathf.Clamp(rotationWeight.Value, 0f, 1f);
			maintainRelativePositionWeight.Value = Mathf.Clamp(maintainRelativePositionWeight.Value, 0f, 1f);

			startEffectorPositionWeight.Value = Mathf.Clamp(startEffectorPositionWeight.Value, 0f, 1f);

			maintainRotationWeight.Value = Mathf.Clamp(maintainRotationWeight.Value, 0f, 1f);
			mappingWeight.Value = Mathf.Clamp(mappingWeight.Value, 0f, 1f);

			chain.pull = pull.Value;
			chain.reach = reach.Value;
			chain.push = push.Value;
			chain.pushParent = pushParent.Value;
			chain.reachSmoothing = reachSmoothing;
			chain.pushSmoothing = pushSmoothing;
			chain.bendConstraint.bendGoal = bendGoal.Value == null? null: bendGoal.Value.transform;
			chain.bendConstraint.weight = bendGoalWeight.Value;

			effector.target = target.Value == null? null: target.Value.transform;
			effector.position = position.Value;
			effector.positionWeight = positionWeight.Value;
			effector.rotation = rotation.Value;
			effector.rotationWeight = rotationWeight.Value;
			effector.positionOffset = positionOffset.Value;
			effector.maintainRelativePositionWeight = maintainRelativePositionWeight.Value;

			startEffector.target = startEffectorTarget.Value == null? null: startEffectorTarget.Value.transform;
			startEffector.position = startEffectorPosition.Value;
			startEffector.positionWeight = startEffectorPositionWeight.Value;
			startEffector.positionOffset = startEffectorPositionOffset.Value;

			mapping.maintainRotationWeight = maintainRotationWeight.Value;
			mapping.weight = mappingWeight.Value;
		}

		private static FullBodyBipedEffector GetStartEffector(FullBodyBipedChain chain) {
			switch(chain) {
			case FullBodyBipedChain.LeftArm: return FullBodyBipedEffector.LeftShoulder;
			case FullBodyBipedChain.RightArm: return FullBodyBipedEffector.RightShoulder;
			case FullBodyBipedChain.LeftLeg: return FullBodyBipedEffector.LeftThigh;
			default: return FullBodyBipedEffector.RightThigh;
			}
		}
		
		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.FullBodyBipedIK);
		}
	}
}