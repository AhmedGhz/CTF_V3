using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace HutongGames.PlayMaker.Action
{
    [ActionCategory("Behavior Designer")]
    [Tooltip("Starts the specified behavior tree.")]
    public class StartBehaviorTree : FsmStateAction
    {
        [Tooltip("The GameObject that contains the behavior tree component")]
        public FsmOwnerDefault gameObject = null;
        [Tooltip("The group of the behavior tree")]
        public FsmInt group = 0;
        [Tooltip("Should the action wait for the behavior tree to finish executing?")]
        public FsmBool waitForCompletion = true;
        [Tooltip("Should the local variables be synchronized between Behavior Designer and PlayMaker?")]
        public FsmBool synchronizeVariables = false;
        [Tooltip("Should the global variables be synchronized between Behavior Designer and PlayMaker?")]
        public FsmBool synchronizeGlobalVariables = false;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("The results after the behavior tree has finished running.")]
        public FsmBool storeSuccess;

        private GameObject prevGameObject;
        private BehaviorTree behaviorTree;

        public override void Reset()
        {
            gameObject = null;
            group = 0;
            waitForCompletion = true;
            synchronizeVariables = false;
            synchronizeGlobalVariables = false;
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

            // Synchronize variables
            if (synchronizeVariables.Value) {
                BehaviorManager.instance.SyncVariablesFromPlayMaker(behaviorTree.GetBehaviorSource(), Fsm.Variables);
            }
            if (synchronizeGlobalVariables.Value) {
                BehaviorManager.instance.SyncGlobalVariablesFromPlayMaker();
            }

            behaviorTree.EnableBehavior();

            if (!waitForCompletion.Value)
                Finish();
        }

        public override void OnUpdate()
        {
            // Continue waiting until the task gets done executing
            if (behaviorTree.ExecutionStatus != TaskStatus.Running) {
                storeSuccess.Value = behaviorTree.ExecutionStatus == TaskStatus.Success;
                Finish();
            }
        }

        public override void OnExit()
        {
 	        // Synchronize variables
            if (synchronizeVariables.Value) {
                BehaviorManager.instance.SyncVariablesToPlayMaker(behaviorTree.GetBehaviorSource(), Fsm.Variables);
            }
            if (synchronizeGlobalVariables.Value) {
                BehaviorManager.instance.SyncGlobalVariablesToPlayMaker();
            }
        }
    }
}