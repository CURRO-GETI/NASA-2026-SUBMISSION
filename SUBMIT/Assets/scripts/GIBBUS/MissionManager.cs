using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GIBBUS
{
    public class MissionManager : MonoBehaviour
    {
        public static MissionManager Instance { get; private set; }

        [SerializeField] private MissionCatalog catalog;
        public MissionCatalog Catalog => catalog;
        public MissionConfiguration Configuration { get; private set; } = new MissionConfiguration();
        public MissionRuntimeState Runtime { get; private set; } = new MissionRuntimeState();

        public event Action OnMissionChanged;
        public event Action<List<ValidationIssue>> OnValidationCompleted;

        private List<ValidationIssue> lastIssues = new List<ValidationIssue>();
        public IReadOnlyList<ValidationIssue> LastIssues => lastIssues;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (catalog == null) Debug.LogError("GIBBUS: Assign a MissionCatalog to MissionManager.");
        }

        public void StartNewMission()
        {
            Configuration = new MissionConfiguration();
            Runtime = new MissionRuntimeState();
            lastIssues.Clear();
            OnMissionChanged?.Invoke();
        }

        public void SelectLaunchVehicle(string id)
        {
            Configuration.launchVehicleId = id;
            Changed();
        }

        public void SelectSpacecraft(string id)
        {
            Configuration.spacecraftId = id;
            Changed();
        }

        public void ToggleInstrument(string id)
        {
            if (catalog == null || !catalog.HasInstrument(id)) return;
            if (Configuration.selectedInstrumentIds.Contains(id))
                Configuration.selectedInstrumentIds.Remove(id);
            else
                Configuration.selectedInstrumentIds.Add(id);
            Changed();
        }

        public void SetLandingSite(string id)
        {
            Configuration.landingSiteId = id;
            Changed();
        }

        public void SetSciencePriority(bool detailedAnalysis, bool sampleCollection)
        {
            Configuration.prioritiseDetailedAnalysis = detailedAnalysis;
            Configuration.prioritiseSampleCollection = sampleCollection;
            Changed();
        }

        public List<ValidationIssue> ValidateMission()
        {
            lastIssues = MissionCalculator.Validate(catalog, Configuration);
            OnValidationCompleted?.Invoke(lastIssues);
            OnMissionChanged?.Invoke();
            return lastIssues;
        }

        public bool IsMissionValid()
        {
            return ValidateMission().Count == 0;
        }

        public bool ApproveLaunch()
        {
            if (!IsMissionValid()) return false;
            Runtime.currentPhase = MissionPhase.Launch;
            var craft = catalog.GetSpacecraft(Configuration.spacecraftId);
            Runtime.currentPropellant = craft != null ? craft.propulsionAvailable : 0f;
            Runtime.currentBatteryKwh = craft != null ? craft.batteryCapacityKwh : 0f;
            Changed();
            return true;
        }

        public bool TryAdvancePhase()
        {
            MissionPhase next;
            switch (Runtime.currentPhase)
            {
                case MissionPhase.Briefing: next = MissionPhase.Design; break;
                case MissionPhase.Design:
                case MissionPhase.Validation:
                    if (!IsMissionValid()) return false;
                    next = MissionPhase.Launch; break;
                case MissionPhase.Launch: next = MissionPhase.OutboundCruise; break;
                case MissionPhase.OutboundCruise: next = MissionPhase.LunarOrbit; break;
                case MissionPhase.LunarOrbit: next = MissionPhase.Landing; break;
                case MissionPhase.Landing:
                    if (!Runtime.landed) return false;
                    next = MissionPhase.SurfaceOperations; break;
                case MissionPhase.SurfaceOperations:
                    if (!AllScienceObjectivesComplete()) return false;
                    if (!Runtime.ascentSystemAvailable || Runtime.currentPropellant <= 0f) return false;
                    next = MissionPhase.Ascent; break;
                case MissionPhase.Ascent: next = MissionPhase.Rendezvous; break;
                case MissionPhase.Rendezvous: next = MissionPhase.EarthReturn; break;
                case MissionPhase.EarthReturn: next = MissionPhase.Recovery; break;
                case MissionPhase.Recovery:
                    if (!Runtime.crewSafe) return false;
                    next = MissionPhase.Debrief; break;
                default: return false;
            }

            Runtime.currentPhase = next;
            Changed();
            return true;
        }

        public void SetLanded(bool landed)
        {
            Runtime.landed = landed;
            Changed();
        }

        public void SetAnomalyResolved(bool resolved)
        {
            Runtime.anomalyResolved = resolved;
            Changed();
        }

        public void SetAscentSystemAvailable(bool available)
        {
            Runtime.ascentSystemAvailable = available;
            Changed();
        }

        public void SetCrewSafe(bool safe)
        {
            Runtime.crewSafe = safe;
            Changed();
        }

        public bool HasInstrument(string instrumentId)
        {
            return Configuration.selectedInstrumentIds.Contains(instrumentId);
        }

        public bool CanUseInstrument(string instrumentId)
        {
            return HasInstrument(instrumentId) && catalog != null && catalog.GetInstrument(instrumentId) != null;
        }

        public bool RecordScienceActivity(ScienceActivityType activity, string instrumentId, string result)
        {
            if (Runtime.currentPhase != MissionPhase.SurfaceOperations) return false;
            if (!string.IsNullOrEmpty(instrumentId) && !CanUseInstrument(instrumentId)) return false;

            Runtime.scienceLog.Add(new ScienceRecord
            {
                activityId = activity.ToString(),
                instrumentId = instrumentId,
                result = result,
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            });

            if (activity == ScienceActivityType.SampleCollection)
                Runtime.samplesCollected++;

            UpdateCompletedObjectives();
            Changed();
            return true;
        }

        public bool AllScienceObjectivesComplete()
        {
            if (catalog == null) return false;
            foreach (var objective in catalog.objectives)
            {
                int count = 0;
                foreach (var record in Runtime.scienceLog)
                    if (record.activityId == objective.requiredActivity.ToString()) count++;
                if (count < objective.requiredCount) return false;
            }
            return true;
        }

        private void UpdateCompletedObjectives()
        {
            foreach (var objective in catalog.objectives)
            {
                int count = 0;
                foreach (var record in Runtime.scienceLog)
                    if (record.activityId == objective.requiredActivity.ToString()) count++;
                if (count >= objective.requiredCount && !Runtime.completedObjectiveIds.Contains(objective.id))
                    Runtime.completedObjectiveIds.Add(objective.id);
            }
        }

        public string BuildDebrief()
        {
            return $"{catalog.missionName}\nPhase: {Runtime.currentPhase}\nSamples: {Runtime.samplesCollected}\nScience records: {Runtime.scienceLog.Count}\nObjectives completed: {Runtime.completedObjectiveIds.Count}/{catalog.objectives.Count}\nCrew safe: {Runtime.crewSafe}";
        }

        private void Changed()
        {
            OnMissionChanged?.Invoke();
        }
    }
}
