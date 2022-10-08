using UnityEngine;
/*
namespace BehaviorDesigner.Runtime.Tasks.PlayMaker
{
    [TaskDescription("Broadcasts an event to all PlayMaker FSMs. The task will return success immediately.")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/playmaker/")]
    [TaskIcon("Assets/Behavior Designer/Integrations/PlayMaker/Editor/PlayMakerIcon.png")]
    [TaskCategory("PlayMaker")]
    public class BroadcastEvent : Action
    {
        [Tooltip("The name of the event to broadcast to the all of the PlayMaker FSMs")]
        public SharedString eventName = "FsmEvent";

        public override TaskStatus OnUpdate()
        {
            var playMakerFSMs = PlayMakerFSM.FsmList;
            if (playMakerFSMs != null && !eventName.Equals("")) {
                for (int i = playMakerFSMs.Count - 1; i > -1; --i) {
                    playMakerFSMs[i].SendEvent(eventName.Value);
                }
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            eventName = "FsmEvent";
        }
    }
}
*/