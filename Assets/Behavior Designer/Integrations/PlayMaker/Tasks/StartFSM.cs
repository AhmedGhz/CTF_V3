using UnityEngine;
using HutongGames.PlayMaker;

namespace BehaviorDesigner.Runtime.Tasks.PlayMaker
{
    [TaskDescription("Start executing a PlayMaker FSM. The task will stay in a running state until PlayMaker FSM has returned success or failure. " +
                     "The PlayMaker FSM must contain a Behavior Listener state with the specified event name to start executing and finish with a Resume From PlayMaker action.")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/playmaker/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/PlayMaker/Editor/PlayMakerIcon.png")]
    [TaskCategory("PlayMaker")]
    public class StartFSM : Action
    {
        [Tooltip("The GameObject that the PlayMaker FSM component is attached to")]
        public SharedGameObject playMakerGameObject;
        [Tooltip("The name of the FSM component. This allows you to have multiple FSM components on a single GameObject")]
        public SharedString FsmName = "FSM";
        [Tooltip("The name of the event to fire to start executing the FSM within PlayMaker")]
        public SharedString startEventName = "StartFSM";
        [Tooltip("The name of the event to fire to pause executing the FSM within PlayMaker")]
        public SharedString pauseEventName = "";
        [Tooltip("The name of the event to fire to resume executing the FSM within PlayMaker")]
        public SharedString resumeEventName = "";
        [Tooltip("The name of the event to fire to end executing the FSM within PlayMaker")]
        public SharedString endEventName = "";
        [Tooltip("Should the task wait for the FSM to complete its execution?")]
        public SharedBool waitForFSMCompletion = true;
        [Tooltip("When the PlayMaker FSM is complete should we restart the FSM back to its original state? This variable is used by the Behavior Manager")]
        public SharedBool resetOnComplete = false;
        [Tooltip("Should the local variables be synchronized between Behavior Designer and PlayMaker?")]
        public SharedBool synchronizeVariables = false;
        [Tooltip("Should the global variables be synchronized between Behavior Designer and PlayMaker?")]
        public SharedBool synchronizeGlobalVariables = false;

        // A cache of the PlayMakerFSM
        private PlayMakerFSM playMakerFSM;
        public PlayMakerFSM PlayMakerFSM { get { return playMakerFSM; } }
        // The return status of the FSM after it has finished executing
        private TaskStatus status;

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
            } else {
                // Tell the Behavior Manager that we are running a PlayMaker FSM instance.
                if (playMakerFSM != null && (!waitForFSMCompletion.Value || BehaviorManager.instance.MapObjectToTask(playMakerFSM.Fsm, this, BehaviorManager.ThirdPartyObjectType.PlayMaker))) {
                    if (waitForFSMCompletion.Value) {
                        status = TaskStatus.Running;
                    } else {
                        status = TaskStatus.Success;
                    }

                    // Synchronize variables
                    if (synchronizeVariables.Value) {
                        BehaviorManager.instance.SyncVariablesToPlayMaker(Owner.GetBehaviorSource(), playMakerFSM.FsmVariables);
                    }
                    if (synchronizeGlobalVariables.Value) {
                        BehaviorManager.instance.SyncGlobalVariablesToPlayMaker();
                    }

                    // Fire an event to start PlayMaker.
                    playMakerFSM.Fsm.Event(startEventName.Value);
                } else {
                    // If something went wrong then return failure.
                    status = TaskStatus.Failure;
                }
            }
        }
        
        public override TaskStatus OnUpdate()
        {
            // We are returning the same status until we hear otherwise.
            return status;
        }

        public override void OnPause(bool paused)
        {
            if (playMakerFSM != null) {
                if (paused && !pauseEventName.Equals("")) {
                    playMakerFSM.Fsm.Event(pauseEventName.Value);
                } else if (!paused && !resumeEventName.Equals("")) {
                    playMakerFSM.Fsm.Event(resumeEventName.Value);
                }
            }
        }

        // The PlayMaker action ResumeFromPlayMaker will call this function when it has completed. 
        public void PlayMakerFinished(TaskStatus playMakerStatus)
        {
            // Update the status with what PlayMaker returned.
            status = playMakerStatus;
        }

        public override void OnEnd()
        {
            if (playMakerFSM == null)
                return;

            // Synchronize variables
            if (synchronizeVariables.Value) {
                BehaviorManager.instance.SyncVariablesFromPlayMaker(Owner.GetBehaviorSource(), playMakerFSM.FsmVariables);
            }
            if (synchronizeGlobalVariables.Value) {
                BehaviorManager.instance.SyncGlobalVariablesFromPlayMaker();
            }
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            playMakerGameObject = null;
            FsmName = "FSM";
            startEventName = "StartFSM";
            endEventName = pauseEventName = resumeEventName = "";
            resetOnComplete = false;
        }
    }
}
