using System.Collections.Generic;
using UnityEngine;

namespace GIBBUS
{
    [CreateAssetMenu(fileName = "MissionCatalog", menuName = "GIBBUS/Mission Catalog")]
    public class MissionCatalog : ScriptableObject
    {
        [Header("Mission requirements")]
        public string missionName = "GIBBUS-01";
        public float totalBudget = 100000f;
        public float requiredDataStorageMb = 100f;
        public float minimumPropulsionReserve = 10f;
        public int requiredSampleCount = 1;
        public bool requireCrewReturnSpacecraft = true;
        public bool requireServiceModule = true;
        public bool requireLunarLander = true;

        [Header("Available launch vehicles")]
        public List<LaunchVehicleDefinition> launchVehicles = new List<LaunchVehicleDefinition>();

        [Header("Available spacecraft architectures")]
        public List<SpacecraftArchitectureDefinition> spacecraftOptions = new List<SpacecraftArchitectureDefinition>();

        [Header("Available instruments")]
        public List<EquipmentDefinition> instruments = new List<EquipmentDefinition>();

        [Header("Science objectives")]
        public List<MissionObjective> objectives = new List<MissionObjective>();

        public LaunchVehicleDefinition GetLauncher(string id) =>
            launchVehicles.Find(x => x.id == id);

        public SpacecraftArchitectureDefinition GetSpacecraft(string id) =>
            spacecraftOptions.Find(x => x.id == id);

        public EquipmentDefinition GetInstrument(string id) =>
            instruments.Find(x => x.id == id);

        public bool HasInstrument(string id)
        {
            return instruments.Exists(x => x.id == id);
        }
    }
}
