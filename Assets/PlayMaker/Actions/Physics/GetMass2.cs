// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets the Mass of a Game Object's Rigid Body EVERYFRAME.")]
	
	public class GetMass2 : ComponentAction<Rigidbody>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
        [Tooltip("The GameObject that owns the Rigidbody")]
		public FsmOwnerDefault gameObject;
		
        [RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the mass in a float variable.")]
		public FsmFloat storeResult;
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnUpdate()
		{
			DoGetMass();
			
			if(!everyFrame)
			{
				Finish();
			}
		}

		void DoGetMass()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
		    if (UpdateCache(go))
		    {
                storeResult.Value = rigidbody.mass;
		    }
		}
	}
}