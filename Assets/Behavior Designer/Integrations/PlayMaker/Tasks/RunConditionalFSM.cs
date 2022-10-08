using UnityEngine;
using HutongGames.PlayMaker;

namespace BehaviorDesigner.Runtime.Tasks.PlayMaker
{
    [TaskDescription("Run a PlayMaker FSM that completes within the same frame. If the FSM does not complete in time then the task will return failure. " +
                     "The PlayMaker FSM must contain a Behavior Listener state with the specified event name to start executing and finish with a Resume From PlayMaker action.")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/playmaker/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/PlayMaker/Editor/PlayMakerIcon.png")]
    [TaskCategory("PlayMaker")]
    public class RunConditionalFSM : Conditional
    {
        [Tooltip("The GameObject that the PlayMaker FSM component is attached to")]
        public SharedGameObject playMakerGameObject;
        [Tooltip("The name of the FSM component. This allows you to have multiple FSM components on a single GameObject")]
        public SharedString FsmName = "FSM";
        [Tooltip("The name of the event to fire to start executing the FSM within PlayMaker")]
        public SharedString eventName = "StartFSM";

        // A cache of the PlayMakerFSM
        private PlayMakerFSM playMakerFSM;
        public PlayMakerFSM PlayMakerFSM { get { return playMakerFSM; } }
        // The return status of the FSM after it has finished executing
        private TaskStatus status;
        public TaskStatus Status { get { return status; } }

        public override void OnStart()
        {
            // Find the correct PlayMakerFSM based on the name.
            var playMakerComponents = playMakerGameObject.Value != null ? playMakerGameObject.Value.GetComponents<PlayMakerFSM>() : gameObject.GetComponents<PlayMakerFSM>();
            if (playMakerComponents != null && playMakerComponents.Length > 0) {
                playMakerFSM = playMakerComponents[0];
                //  We don't need the FsmName if there is only one PlayMakerFSM component
                if (playMakerComponents.Length > 1) {
                    for (int i = 0; i < playMakerComponents.Length; ++i) {
                        if (playMakerComponents[i].FsmName.Equals(FsmName.Value)) {
                            // Cache the result when we have a match and stop looping.
                            playMakerFSM = playMakerComponents[i];
                            break;
                        }
                    }
                }
            }

            // We can't do much if there isn't a PlayMakerFSM.
            if (playMakerFSM == null) {
                Debug.LogError(string.Format("Unable to find PlayMaker FSM {0}{1}", FsmName.Value, (playMakerGameObject.Value != null ? string.Format(" attached to {0}", playMakerGameObject.Value.name) : "")));
            }
        }

        public override TaskStatus OnUpdate()
        {
            // Tell the Behavior Manager that we are running a PlayMaker FSM instance.
            if (playMakerFSM != null && BehaviorManager.instance.MapObjectToTask(playMakerFSM.Fsm, this, BehaviorManager.ThirdPartyObjectType.PlayMaker)) {
                status = TaskStatus.Failure;

                // Fire an event to start PlayMaker.
                playMakerFSM.Fsm.Event(eventName.Value);

                // Remove the task from the mapping immediately after the event is called. If the Fsm hasn't returned by now it is running longer than one frame and has lost its chance to return
                BehaviorManager.instance.RemoveActiveThirdPartyTask(this);
            } else {
                // If something went wrong then return failure.
                status = TaskStatus.Failure;
            }
            return status;
        }

        // The PlayMaker action ResumeFromPlayMaker will call this function when it has completed. 
        public void PlayMakerFinished(TaskStatus playMakerStatus)
        {
            // Update the status with what PlayMaker returned.
            status = playMakerStatus;
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            playMakerGameObject = null;
            FsmName = "FSM";
            eventName = "StartFSM";
        }
    }
}
