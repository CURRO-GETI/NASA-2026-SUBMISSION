
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GIBBUS
{
    public class MissionBudgetDisplay : MonoBehaviour
    {
        [System.Serializable]
        public class ResourceDisplay
        {
            [Header("TEXT REFERENCES")]
            public TMP_Text usedValueText;
            public TMP_Text remainingValueText;
            public TMP_Text utilisationText;
            public TMP_Text statusText;

            [Header("FILL BAR")]
            public Image fillImage;

            [Header("DISPLAY FORMATTING")]
            public string unit;
            [Range(0, 2)]
            public int decimalPlaces = 0;
        }

        [Header("MISSION DATA")]
        [SerializeField]
        private MissionBaselineData baselineData;

        [Header("RESOURCE DISPLAYS")]
        [SerializeField] private ResourceDisplay mass;
        [SerializeField] private ResourceDisplay power;
        [SerializeField] private ResourceDisplay dataStorage;
        [SerializeField] private ResourceDisplay budget;
        [SerializeField] private ResourceDisplay samples;

        [Header("VALIDATION")]
        [SerializeField] private TMP_Text baselineStatusText;

        private void Start()
        {
            RefreshDisplay();
        }

        public void RefreshDisplay()
        {
            if (baselineData == null)
            {
                Debug.LogError(
                    "[GIBBUS] MissionBaselineData is not assigned!",
                    this
                );

                if (baselineStatusText != null)
                    baselineStatusText.text = "NO BASELINE DATA";

                return;
            }

            // MASS
            UpdateResource(
                mass,
                MissionCalculations.CalculateTotalMass(baselineData),
                baselineData.launchVehicleCapacity
            );

            // POWER
            UpdateResource(
                power,
                MissionCalculations.CalculateTotalPowerDemand(baselineData),
                baselineData.availablePower
            );

            // DATA
            UpdateResource(
                dataStorage,
                MissionCalculations.CalculateTotalDataGenerated(baselineData),
                baselineData.storageCapacity
            );

            // BUDGET
            UpdateResource(
                budget,
                MissionCalculations.CalculateTotalBudgetAllocation(baselineData),
                baselineData.totalBudget
            );

            // SAMPLES
            UpdateResource(
                samples,
                baselineData.plannedSampleMass,
                baselineData.sampleCapacity
            );

            UpdateBaselineStatus();
        }

        private void UpdateResource(
            ResourceDisplay display,
            float used,
            float capacity
        )
        {
            if (display == null)
                return;

            float remaining = capacity - used;

            float utilisation = capacity > 0f
                ? (used / capacity) * 100f
                : 0f;

            bool withinLimit = used <= capacity;

            string format = "N" + display.decimalPlaces;

            if (display.usedValueText != null)
                display.usedValueText.text =
                    used.ToString(format) + display.unit;

            if (display.remainingValueText != null)
                display.remainingValueText.text =
                    remaining.ToString(format) + display.unit;

            if (display.utilisationText != null)
                display.utilisationText.text =
                    utilisation.ToString("F1") + "%";

            if (display.statusText != null)
            {
                display.statusText.text = withinLimit
                    ? "WITHIN LIMIT"
                    : "OVER LIMIT";
            }

            if (display.fillImage != null)
            {
                display.fillImage.fillAmount = capacity > 0f
                    ? Mathf.Clamp01(used / capacity)
                    : 0f;
            }
        }

        private void UpdateBaselineStatus()
        {
            if (baselineStatusText == null)
                return;

            bool valid =
                MissionCalculations.IsBaselineConfigurationValid(
                    baselineData
                );

            baselineStatusText.text = valid
                ? "BASELINE CONFIGURATION VERIFIED"
                : "BASELINE CONFIGURATION INVALID";
        }
    }
}
