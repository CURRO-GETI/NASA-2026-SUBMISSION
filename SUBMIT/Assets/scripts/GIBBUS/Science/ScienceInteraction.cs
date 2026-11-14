using UnityEngine;

namespace GIBBUS
{
    // Add to an interactable object in the lunar scene.
    // Call Interact() from your interaction/raycast system.
    public class ScienceInteraction : MonoBehaviour
    {
        public ScienceActivityType activityType = ScienceActivityType.GeologicalSurvey;
        public string requiredInstrumentId = "camera";
        [TextArea] public string resultText = "Terrain observations recorded.";
        public bool requiresInstrument = true;

        public void Interact()
        {
            var manager = MissionManager.Instance;
            if (manager == null) return;

            string instrument = requiresInstrument ? requiredInstrumentId : "";
            if (requiresInstrument && !manager.CanUseInstrument(instrument))
            {
                Debug.Log($"Cannot perform {activityType}: {requiredInstrumentId} was not selected for this mission.");
                return;
            }

            bool success = manager.RecordScienceActivity(activityType, instrument, resultText);
            if (success) Debug.Log($"Science recorded: {resultText}");
            else Debug.Log("Science activity unavailable in the current mission phase.");
        }
    }
}
