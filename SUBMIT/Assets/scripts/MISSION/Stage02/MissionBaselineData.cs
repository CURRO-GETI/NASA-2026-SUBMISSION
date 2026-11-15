
using UnityEngine;

namespace GIBBUS
{
    [CreateAssetMenu(
        fileName = "GIBBUS_MissionBaseline",
        menuName = "GIBBUS/Mission/Baseline Data"
    )]
    public class MissionBaselineData : ScriptableObject
    {
        [Header("LAUNCH VEHICLE")]
        public string launchVehicleName = "LV-2";
        public float launchVehicleCapacity = 32000f;
        public float launchVehicleCost = 180f;

        [Header("SPACECRAFT MASS (KG)")]
        public float crewReturnSpacecraftMass = 9000f;
        public float serviceModuleMass = 8500f;
        public float lunarLanderMass = 9500f;

        [Header("SCIENCE EQUIPMENT MASS (KG)")]
        public float cameraMass = 100f;
        public float spectrometerMass = 150f;
        public float samplingKitMass = 200f;
        public float seismometerMass = 100f;
        public float dataStorageMass = 50f;

        [Header("SUPPORT EQUIPMENT MASS (KG)")]
        public float crewSuppliesMass = 1400f;
        public float integrationHardwareMass = 1000f;

        [Header("POWER (KW)")]
        public float availablePower = 7.5f;

        public float cameraPower = 1f;
        public float spectrometerPower = 1.2f;
        public float samplingKitPower = 0.6f;
        public float seismometerPower = 0.8f;
        public float dataStoragePower = 0.3f;
        public float avionicsPower = 0.8f;
        public float crewSupportPower = 0.9f;
        public float communicationsPower = 0.4f;

        [Header("BATTERY")]
        public float usableBatteryEnergy = 18f;
        public float criticalPowerDemand = 1.5f;

        [Header("DATA STORAGE (GB)")]
        public float storageCapacity = 400f;
        public float cameraData = 120f;
        public float spectrometerData = 80f;
        public float seismometerData = 40f;
        public float telemetryData = 10f;

        [Header("FINANCIAL BUDGET (MILLIONS)")]
        public float totalBudget = 500f;
        public float spacecraftCost = 70f;
        public float serviceModuleCost = 55f;
        public float lunarLanderCost = 65f;
        public float scienceEquipmentCost = 15f;
        public float suppliesAndIntegrationCost = 10f;
        public float missionOperationsCost = 25f;

        [Header("SAMPLE STORAGE (KG)")]
        public float sampleCapacity = 2f;
        public float plannedSampleMass = 1.5f;
    }
}
