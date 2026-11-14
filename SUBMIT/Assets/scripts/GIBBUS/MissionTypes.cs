using System;
using System.Collections.Generic;
using UnityEngine;

namespace GIBBUS
{
    public enum MissionPhase
    {
        Briefing, Design, Validation, Launch, OutboundCruise, LunarOrbit,
        Landing, SurfaceOperations, Ascent, Rendezvous, EarthReturn,
        Recovery, Debrief
    }

    public enum ScienceActivityType
    {
        GeologicalSurvey, SampleCollection, InstrumentDeployment, SpectrometerScan,
        SeismicMeasurement, GeneralObservation
    }

    [Serializable]
    public class EquipmentDefinition
    {
        public string id;
        public string displayName;
        [TextArea] public string description;

        [Header("Fictional game values")]
        public float massKg;
        public float cost;
        public float powerDemandKw;
        public float dataOutputMb;
        public int sampleCapacity;
        public bool isRequired;
    }

    [Serializable]
    public class LaunchVehicleDefinition
    {
        public string id;
        public string displayName;
        public float payloadCapacityKg;
        public float cost;
        public bool supportsLunarDeparture = true;
    }

    [Serializable]
    public class SpacecraftArchitectureDefinition
    {
        public string id;
        public string displayName;
        public bool hasCrewReturnSpacecraft = true;
        public bool hasServiceModule = true;
        public bool hasLunarLander = true;
        public float dryMassKg;
        public float cost;
        public float powerGenerationKw;
        public float batteryCapacityKwh;
        public float propulsionAvailable;
        public float propulsionRequired;
        public bool hasCommunications = true;
        public bool hasNavigation = true;
        public bool hasCrewSupport = true;
        public bool hasBackupMode;
    }

    [Serializable]
    public class MissionObjective
    {
        public string id;
        public string title;
        [TextArea] public string description;
        public ScienceActivityType requiredActivity;
        public int requiredCount = 1;
    }

    [Serializable]
    public class MissionConfiguration
    {
        public string launchVehicleId;
        public string spacecraftId;
        public List<string> selectedInstrumentIds = new List<string>();
        public string landingSiteId;
        public bool prioritiseDetailedAnalysis;
        public bool prioritiseSampleCollection;
    }

    [Serializable]
    public class ScienceRecord
    {
        public string activityId;
        public string instrumentId;
        public string result;
        public string timestamp;
    }

    [Serializable]
    public class MissionRuntimeState
    {
        public MissionPhase currentPhase = MissionPhase.Briefing;
        public float currentPropellant;
        public float currentBatteryKwh;
        public int samplesCollected;
        public bool anomalyResolved;
        public bool landed;
        public bool ascentSystemAvailable = true;
        public bool crewSafe = true;
        public List<ScienceRecord> scienceLog = new List<ScienceRecord>();
        public List<string> completedObjectiveIds = new List<string>();
    }

    public class ValidationIssue
    {
        public string title;
        public string explanation;
        public string suggestedFix;
        public bool isCritical;

        public ValidationIssue(string title, string explanation, string suggestedFix, bool critical = true)
        {
            this.title = title;
            this.explanation = explanation;
            this.suggestedFix = suggestedFix;
            this.isCritical = critical;
        }

        public override string ToString()
        {
            return title + ": " + explanation + " Suggested fix: " + suggestedFix;
        }
    }
}
