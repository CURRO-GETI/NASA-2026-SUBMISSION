using System.Collections.Generic;
using UnityEngine;

namespace GIBBUS
{
    public static class MissionCalculator
    {
        public static float GetTotalMass(MissionCatalog catalog, MissionConfiguration config)
        {
            var craft = catalog.GetSpacecraft(config.spacecraftId);
            if (craft == null) return 0f;

            float total = craft.dryMassKg;
            foreach (string id in config.selectedInstrumentIds)
            {
                var item = catalog.GetInstrument(id);
                if (item != null) total += item.massKg;
            }
            return total;
        }

        public static float GetTotalCost(MissionCatalog catalog, MissionConfiguration config)
        {
            var launcher = catalog.GetLauncher(config.launchVehicleId);
            var craft = catalog.GetSpacecraft(config.spacecraftId);
            if (launcher == null || craft == null) return 0f;

            float total = launcher.cost + craft.cost;
            foreach (string id in config.selectedInstrumentIds)
            {
                var item = catalog.GetInstrument(id);
                if (item != null) total += item.cost;
            }
            return total;
        }

        public static float GetPowerDemand(MissionCatalog catalog, MissionConfiguration config)
        {
            float demand = 0f;
            foreach (string id in config.selectedInstrumentIds)
            {
                var item = catalog.GetInstrument(id);
                if (item != null) demand += item.powerDemandKw;
            }
            return demand;
        }

        public static float GetDataOutput(MissionCatalog catalog, MissionConfiguration config)
        {
            float data = 0f;
            foreach (string id in config.selectedInstrumentIds)
            {
                var item = catalog.GetInstrument(id);
                if (item != null) data += item.dataOutputMb;
            }
            return data;
        }

        public static int GetSampleCapacity(MissionCatalog catalog, MissionConfiguration config)
        {
            int capacity = 0;
            foreach (string id in config.selectedInstrumentIds)
            {
                var item = catalog.GetInstrument(id);
                if (item != null) capacity += item.sampleCapacity;
            }
            return capacity;
        }

        public static List<ValidationIssue> Validate(MissionCatalog catalog, MissionConfiguration config)
        {
            var issues = new List<ValidationIssue>();
            var launcher = catalog.GetLauncher(config.launchVehicleId);
            var craft = catalog.GetSpacecraft(config.spacecraftId);

            if (launcher == null)
                issues.Add(new ValidationIssue("No launch vehicle", "A launch vehicle has not been selected.", "Select LV-1, LV-2 or LV-3."));
            if (craft == null)
                issues.Add(new ValidationIssue("No spacecraft", "A spacecraft architecture has not been selected.", "Select a valid spacecraft architecture."));

            if (launcher == null || craft == null) return issues;

            float mass = GetTotalMass(catalog, config);
            float cost = GetTotalCost(catalog, config);
            float power = GetPowerDemand(catalog, config);
            float data = GetDataOutput(catalog, config);
            int samples = GetSampleCapacity(catalog, config);

            if (mass > launcher.payloadCapacityKg)
                issues.Add(new ValidationIssue("Payload mass exceeded",
                    $"Configured mass is {mass:0} kg, but {launcher.displayName} can carry {launcher.payloadCapacityKg:0} kg.",
                    "Choose a higher-capacity launcher or remove equipment."));

            if (cost > catalog.totalBudget)
                issues.Add(new ValidationIssue("Budget exceeded",
                    $"Mission cost is {cost:0}, above the available budget of {catalog.totalBudget:0}.",
                    "Choose cheaper hardware or remove optional equipment."));

            if (power > craft.powerGenerationKw)
                issues.Add(new ValidationIssue("Power demand too high",
                    $"Instrument demand is {power:0.0} kW, while the spacecraft generates {craft.powerGenerationKw:0.0} kW.",
                    "Remove an instrument or select a spacecraft with more power."));

            if (craft.propulsionAvailable < craft.propulsionRequired + catalog.minimumPropulsionReserve)
                issues.Add(new ValidationIssue("Propulsion reserve insufficient",
                    "The planned propulsion capability does not leave the required reserve.",
                    "Select a spacecraft configuration with more propulsion reserve."));

            if (data > catalog.requiredDataStorageMb)
                issues.Add(new ValidationIssue("Data storage exceeded",
                    $"Payload produces {data:0} MB, exceeding the required storage capacity of {catalog.requiredDataStorageMb:0} MB.",
                    "Choose a larger data-storage configuration or reduce data-producing instruments."));

            if (samples < catalog.requiredSampleCount)
                issues.Add(new ValidationIssue("Sample capacity insufficient",
                    $"The selected payload can store {samples} samples; the mission requires {catalog.requiredSampleCount}.",
                    "Add the geological sampling kit or increase sample capacity."));

            if (catalog.requireCrewReturnSpacecraft && !craft.hasCrewReturnSpacecraft)
                issues.Add(new ValidationIssue("Crew return system missing", "The architecture has no designated crew-return spacecraft.", "Select an architecture with a crew-return spacecraft."));
            if (catalog.requireServiceModule && !craft.hasServiceModule)
                issues.Add(new ValidationIssue("Service module missing", "The architecture has no service module.", "Add a service module."));
            if (catalog.requireLunarLander && !craft.hasLunarLander)
                issues.Add(new ValidationIssue("Lunar lander missing", "The architecture has no lunar lander.", "Add a lunar lander."));
            if (!craft.hasCommunications)
                issues.Add(new ValidationIssue("Communications unavailable", "The spacecraft cannot communicate with mission control.", "Select communications equipment."));
            if (!craft.hasNavigation)
                issues.Add(new ValidationIssue("Navigation unavailable", "The spacecraft has no navigation capability.", "Add navigation equipment."));
            if (!craft.hasCrewSupport)
                issues.Add(new ValidationIssue("Crew support unavailable", "The spacecraft cannot support the crew for the mission.", "Add crew-support capability."));
            if (!launcher.supportsLunarDeparture)
                issues.Add(new ValidationIssue("Lunar departure unsupported", "The selected launcher does not support the mission's lunar departure profile.", "Select a launcher that supports lunar departure."));

            foreach (var instrument in catalog.instruments)
            {
                if (instrument.isRequired && !config.selectedInstrumentIds.Contains(instrument.id))
                    issues.Add(new ValidationIssue("Required instrument missing",
                        $"{instrument.displayName} is required by the mission specification.",
                        $"Add {instrument.displayName} to the payload."));
            }

            return issues;
        }
    }
}
