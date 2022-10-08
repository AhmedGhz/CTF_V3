using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace HutongGames.PlayMaker.Action
{
    [ActionCategory("Behavior Designer")]
    [Tooltip("Sends an event to the specified behavior tree.")]
    public class SendEvent : FsmStateAction
    {
        [Tooltip("The GameObject that contains the behavior tree component")]
        public FsmOwnerDefault gameObject = null;
        [Tooltip("The group of the behavior tree")]
        public FsmInt group = 0;
        [Tooltip("The name of the event")]
        public FsmString eventName;

        private BehaviorTree behaviorTree;

        public override void Reset()
        {
            gameObject = null;
            group = 0;
            eventName = "";
        }
        
        public override void OnEnter()
        {
            // Find the correct behavior tree based on the grouping
            var behaviorTreeComponents = Fsm.GetOwnerDefaultTarget(gameObject).GetComponents<BehaviorTree>();
            if (behaviorTreeComponents != null && behaviorTreeComponents.Length > 0) {
                behaviorTree = behaviorTreeComponents[0];
                //  We don't need the behaviorTreeGroup if there is only one behavior tree component
                if (behaviorTreeComponents.Length > 1) {
                    for (int i = 0; i < behaviorTreeComponents.Length; ++i) {
                        if (behaviorTreeComponents[i].Group == group.Value) {
                            // Cache the result when we have a match and stop looping.
                            behaviorTree = behaviorTreeComponents[i];
                            break;
                        }
                    }
                }
            }

            behaviorTree.SendEvent(eventName.Value);

            Finish();
        }
    }
}