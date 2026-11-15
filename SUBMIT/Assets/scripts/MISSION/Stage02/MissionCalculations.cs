
using UnityEngine;

namespace GIBBUS
{
    public static class MissionCalculations
    {
        // =========================================
        // MASS CALCULATIONS
        // =========================================

        public static float CalculateTotalMass(MissionBaselineData data)
        {
            return
                data.crewReturnSpacecraftMass +
                data.serviceModuleMass +
                data.lunarLanderMass +
                data.cameraMass +
                data.spectrometerMass +
                data.samplingKitMass +
                data.seismometerMass +
                data.dataStorageMass +
                data.crewSuppliesMass +
                data.integrationHardwareMass;
        }

        public static float CalculateRemainingMassCapacity(MissionBaselineData data)
        {
            return data.launchVehicleCapacity - CalculateTotalMass(data);
        }

        public static float CalculateMassUtilisation(MissionBaselineData data)
        {
            if (data.launchVehicleCapacity <= 0f)
                return 0f;

            return CalculateTotalMass(data) / data.launchVehicleCapacity * 100f;
        }


        // =========================================
        // POWER CALCULATIONS
        // =========================================

        public static float CalculateTotalPowerDemand(MissionBaselineData data)
        {
            return
                data.cameraPower +
                data.spectrometerPower +
                data.samplingKitPower +
                data.seismometerPower +
                data.dataStoragePower +
                data.avionicsPower +
                data.crewSupportPower +
                data.communicationsPower;
        }

        public static float CalculateRemainingPower(MissionBaselineData data)
        {
            return data.availablePower - CalculateTotalPowerDemand(data);
        }

        public static float CalculatePowerUtilisation(MissionBaselineData data)
        {
            if (data.availablePower <= 0f)
                return 0f;

            return CalculateTotalPowerDemand(data) / data.availablePower * 100f;
        }

        public static float CalculateBatteryEndurance(MissionBaselineData data)
        {
            if (data.criticalPowerDemand <= 0f)
                return 0f;

            return data.usableBatteryEnergy / data.criticalPowerDemand;
        }


        // =========================================
        // DATA STORAGE CALCULATIONS
        // =========================================

        public static float CalculateTotalDataGenerated(MissionBaselineData data)
        {
            return
                data.cameraData +
                data.spectrometerData +
                data.seismometerData +
                data.telemetryData;
        }

        public static float CalculateRemainingStorage(MissionBaselineData data)
        {
            return data.storageCapacity - CalculateTotalDataGenerated(data);
        }

        public static float CalculateStorageUtilisation(MissionBaselineData data)
        {
            if (data.storageCapacity <= 0f)
                return 0f;

            return CalculateTotalDataGenerated(data) / data.storageCapacity * 100f;
        }


        // =========================================
        // FINANCIAL CALCULATIONS
        // =========================================

        public static float CalculateTotalBudgetAllocation(MissionBaselineData data)
        {
            return
                data.launchVehicleCost +
                data.spacecraftCost +
                data.serviceModuleCost +
                data.lunarLanderCost +
                data.scienceEquipmentCost +
                data.suppliesAndIntegrationCost +
                data.missionOperationsCost;
        }

        public static float CalculateRemainingBudget(MissionBaselineData data)
        {
            return data.totalBudget - CalculateTotalBudgetAllocation(data);
        }

        public static float CalculateBudgetUtilisation(MissionBaselineData data)
        {
            if (data.totalBudget <= 0f)
                return 0f;

            return CalculateTotalBudgetAllocation(data) / data.totalBudget * 100f;
        }


        // =========================================
        // SAMPLE STORAGE CALCULATIONS
        // =========================================

        public static float CalculateRemainingSampleCapacity(MissionBaselineData data)
        {
            return data.sampleCapacity - data.plannedSampleMass;
        }

        public static float CalculateSampleStorageUtilisation(MissionBaselineData data)
        {
            if (data.sampleCapacity <= 0f)
                return 0f;

            return data.plannedSampleMass / data.sampleCapacity * 100f;
        }


        // =========================================
        // VALIDATION
        // =========================================

        public static bool IsMassWithinLimit(MissionBaselineData data)
        {
            return CalculateTotalMass(data) <= data.launchVehicleCapacity;
        }

        public static bool IsPowerWithinLimit(MissionBaselineData data)
        {
            return CalculateTotalPowerDemand(data) <= data.availablePower;
        }

        public static bool IsDataWithinLimit(MissionBaselineData data)
        {
            return CalculateTotalDataGenerated(data) <= data.storageCapacity;
        }

        public static bool IsBudgetWithinLimit(MissionBaselineData data)
        {
            return CalculateTotalBudgetAllocation(data) <= data.totalBudget;
        }

        public static bool IsSampleStorageWithinLimit(MissionBaselineData data)
        {
            return data.plannedSampleMass <= data.sampleCapacity;
        }

        public static bool IsBaselineConfigurationValid(MissionBaselineData data)
        {
            if (data == null)
                return false;

            return
                IsMassWithinLimit(data) &&
                IsPowerWithinLimit(data) &&
                IsDataWithinLimit(data) &&
                IsBudgetWithinLimit(data) &&
                IsSampleStorageWithinLimit(data);
        }
    }
}
