using System.Text;
using UnityEngine;

namespace GIBBUS
{
    // Attach to a UI GameObject. Hook public methods to Unity Button OnClick events.
    public class MissionUIBridge : MonoBehaviour
    {
        public void ChooseLV1() => MissionManager.Instance.SelectLaunchVehicle("LV-1");
        public void ChooseLV2() => MissionManager.Instance.SelectLaunchVehicle("LV-2");
        public void ChooseLV3() => MissionManager.Instance.SelectLaunchVehicle("LV-3");

        public void ChooseCrewedArchitecture() => MissionManager.Instance.SelectSpacecraft("crewed_lunar_architecture");

        public void ToggleCamera() => MissionManager.Instance.ToggleInstrument("camera");
        public void ToggleSpectrometer() => MissionManager.Instance.ToggleInstrument("spectrometer");
        public void ToggleSamplingKit() => MissionManager.Instance.ToggleInstrument("sampling_kit");
        public void ToggleSeismometer() => MissionManager.Instance.ToggleInstrument("seismometer");
        public void ToggleDataStorage() => MissionManager.Instance.ToggleInstrument("data_storage");

        public void Validate() => MissionManager.Instance.ValidateMission();
        public void Launch() => MissionManager.Instance.ApproveLaunch();
        public void NextPhase() => MissionManager.Instance.TryAdvancePhase();

        public string GetResourceSummary()
        {
            var m = MissionManager.Instance;
            var c = m.Catalog;
            var cfg = m.Configuration;
            var craft = c.GetSpacecraft(cfg.spacecraftId);
            var launcher = c.GetLauncher(cfg.launchVehicleId);
            if (craft == null || launcher == null) return "Select a launch vehicle and spacecraft.";

            return
                $"MASS: {MissionCalculator.GetTotalMass(c, cfg):0} / {launcher.payloadCapacityKg:0} kg\n" +
                $"POWER: {MissionCalculator.GetPowerDemand(c, cfg):0.0} / {craft.powerGenerationKw:0.0} kW\n" +
                $"COST: {MissionCalculator.GetTotalCost(c, cfg):0} / {c.totalBudget:0}\n" +
                $"DATA: {MissionCalculator.GetDataOutput(c, cfg):0} / {c.requiredDataStorageMb:0} MB\n" +
                $"SAMPLES: {MissionCalculator.GetSampleCapacity(c, cfg)} available";
        }

        public string GetValidationReport()
        {
            var issues = MissionManager.Instance.ValidateMission();
            if (issues.Count == 0) return "MISSION VALID: All configured checks passed.";

            var sb = new StringBuilder("MISSION NOT VALID\n");
            foreach (var issue in issues)
                sb.AppendLine("• " + issue.ToString());
            return sb.ToString();
        }

        public string GetDebrief() => MissionManager.Instance.BuildDebrief();
    }
}
