using UnityEngine;
using HutongGames.PlayMaker;

namespace BehaviorDesigner.Runtime.Tasks.PlayMaker
{
    [TaskDescription("Synchronize a SharedGameObject to/from a FsmGameObject.")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/playmaker/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/PlayMaker/Editor/PlayMakerIcon.png")]
    [TaskCategory("PlayMaker/Variables")]
    public class SynchronizeGameObject : Action
    {
        [Tooltip("The SharedVariable that is being synchronized")]
        public SharedGameObject sharedVariable;
        [Tooltip("Should the variable be synchronized from Behavior Designer to PlayMaker?")]
        public SharedBool toBehaviorDesigner = true;
        [Tooltip("The name of the FSM Variable being synchronized")]
        public SharedString FsmVariableName;
        [Tooltip("Is the FSM Variable a global variable?")]
        public SharedBool isGlobalFsmVariable;
        [Tooltip("Required if isGlobalFsmVariable is false. The GameObject that the PlayMaker FSM component is attached to")]
        public SharedGameObject playMakerGameObject;
        [Tooltip("Required if isGlobalFsmVariable is false. The name of the FSM component. This allows you to have multiple FSM components on a single GameObject")]
        public SharedString FsmName = "FSM";

        private PlayMakerFSM playMakerFSM;

        public override void OnStart()
        {
            // playMakerFSM only needs to be found if we are searching for a local FSM variable.
            if (!isGlobalFsmVariable.Value) {
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
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (isGlobalFsmVariable.Value) {
                var fsmVariable = FsmVariables.GlobalVariables.FindFsmGameObject(FsmVariableName.Value);
                // The variable has to exist
                if (fsmVariable != null) {
                    // Sync to Behavior Designer
                    if (toBehaviorDesigner.Value) {
                        sharedVariable.Value = fsmVariable.Value;
                    } else { // Sync to PlayMaker
                        fsmVariable.Value = sharedVariable.Value;
                    }
                    return TaskStatus.Success;
                }
                return TaskStatus.Failure;
            } else {
                // Can't do anything if there isn't a FSM component
                if (playMakerFSM != null) {
                    var fsmVariable = playMakerFSM.FsmVariables.FindFsmGameObject(FsmVariableName.Value);
                    // The variable has to exist
                    if (fsmVariable != null) {
                        // Sync to Behavior Designer
                        if (toBehaviorDesigner.Value) {
                            sharedVariable.Value = fsmVariable.Value;
                        } else { // Sync to PlayMaker
                            fsmVariable.Value = sharedVariable.Value;
                        }
                        return TaskStatus.Success;
                    }
                }
                return TaskStatus.Failure;
            }
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            sharedVariable = null;
            toBehaviorDesigner = true;
            FsmVariableName = "";
            isGlobalFsmVariable = false;
            playMakerGameObject = null;
            FsmName = "FSM";
        }
    }
}