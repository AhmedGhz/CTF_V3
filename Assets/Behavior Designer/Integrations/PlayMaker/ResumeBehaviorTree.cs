using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace HutongGames.PlayMaker.Action
{
    [ActionCategory("Behavior Designer")]
    [Tooltip("Resumes an already executing behavior tree.")]
    public class ResumeBehaviorTree : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Was the FSM a success?")]
        public FsmBool success;

        public override void Reset()
        {
            success = false;
        }
        
        public override void OnEnter()
        {
            // Let the Behavior Manager know that the FSM is done
            BehaviorManager.instance.PlayMakerFinished(Fsm, (success.Value ? TaskStatus.Success : TaskStatus.Failure));

            Finish();
        }
    }
}