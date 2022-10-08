using UnityEngine;
using RootMotion.FinalIK;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Final IK")]
	[Tooltip("Manages a chain of a FABRIKRoot component.")]
	public class FABRIKRootChain : IKAction {

		[Tooltip("The index of the chain in FABRIKRoot chains")]
		public int chainIndex;

		[HasFloatSlider(0, 1)]
		[Tooltip("Parent pulling weight.")]
		public FsmFloat pull;
			
		[HasFloatSlider(0, 1)]
		[Tooltip("Resistance to being pulled by child chains.")]
		public FsmFloat pin;
	
		protected override void ResetAction() {
			pull = new FsmFloat { UseVariable = true };
			pin = new FsmFloat { UseVariable = true };
		}
			
		protected override void UpdateAction() {
			pull.Value = Mathf.Clamp(pull.Value, 0f, 1f);
			pin.Value = Mathf.Clamp(pin.Value, 0f, 1f);

			var solver = (component as RootMotion.FinalIK.FABRIKRoot).solver;

			if (chainIndex < 0 || chainIndex >= solver.chains.Length) {
				Debug.LogWarning("Invalid chainindex.");
				return;
			}

			var chain = solver.chains[chainIndex];

			chain.pull = pull.Value;
			chain.pin = pin.Value;
		}
		
		protected override System.Type GetComponentType() {
			return typeof(RootMotion.FinalIK.FABRIKRoot);
		}
	}
}

