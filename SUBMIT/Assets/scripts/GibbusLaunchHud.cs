using System;
using System.Collections.Generic;
using Ceffy;
using Ceffy.Bridge;
using UnityEngine;
using UnityEngine.Events;
using GIBBUS.Ascent;

namespace GIBBUS.LaunchUI
{
    /// <summary>
    /// Full-screen HTML launch HUD bridge.
    /// Attach this to the same GameObject as CeffyInstance.
    /// Unity remains authoritative for launch readiness and the actual launch sequence.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class GibbusLaunchHud : MonoBehaviour, IGibbusLaunchCommands
    {
        [Header("Ascent and staging")]
        [SerializeField] private GibbusAscentStagingController ascentStaging;
        private const string PageLoadedMessage = "gibbus:page-loaded";
        private const string BridgeReadyMessage = "gibbus:bridge-ready";
        private const string PageUrl = "streaming-assets://GIBBUS/LaunchUI/index.html";

        [Serializable]
        private class ChecklistEntry
        {
            public string id;
            public string title;
            [TextArea(1, 2)] public string description;
            public bool acknowledged;
        }

        [Header("Ceffy")]
        [SerializeField] private CeffyInstance ceffyInstance;

        [Header("Mission")]
        [SerializeField] private string missionCode = "GIBBUS-01";
        [SerializeField] private string missionPhase = "PRE-LAUNCH";
        [TextArea(2, 4)]
        [SerializeField] private string missionObjective =
            "Investigate a selected lunar region, deploy scientific instruments, recover geological samples and return the crew safely to Earth.";

        [Header("Pre-launch checklist")]
        [SerializeField] private List<ChecklistEntry> checklist = new List<ChecklistEntry>
        {
            new ChecklistEntry { id = "vehicle", title = "Launch vehicle configuration", description = "Vehicle configuration and payload allocation reviewed." },
            new ChecklistEntry { id = "propulsion", title = "Propulsion and staging", description = "Propulsion configuration and staging plan reviewed." },
            new ChecklistEntry { id = "power", title = "Electrical power", description = "Spacecraft and launch-system power requirements reviewed." },
            new ChecklistEntry { id = "communications", title = "Communications link", description = "Ground-to-vehicle communications configuration reviewed." },
            new ChecklistEntry { id = "navigation", title = "Navigation and guidance", description = "Guidance, navigation and flight profile reviewed." },
            new ChecklistEntry { id = "crew", title = "Crew and spacecraft status", description = "Crew readiness and spacecraft status reviewed." }
        };

        [Header("Telemetry - connect these values to your simulation")]
        [SerializeField] private float altitudeKm;
        [SerializeField] private float velocityKmPerSecond;
        [Range(0, 100)][SerializeField] private float propellantPercent = 100f;
        [Range(0, 100)][SerializeField] private float powerPercent = 100f;
        [SerializeField] private string communicationsStatus = "STANDBY";
        [SerializeField] private string trajectory = "EARTH → LUNAR TRANSFER";
        [SerializeField] private int countdownSeconds = -1;

        [Header("Unity events - connect your existing scene controllers here")]
        [SerializeField] private UnityEvent onLaunchAuthorized = new UnityEvent();
        [SerializeField] private UnityEvent onReturnToMissionDesign = new UnityEvent();

        private readonly List<LaunchLogEntry> eventLog = new List<LaunchLogEntry>();
        private CeffyBridge bridge;
        private IGibbusLaunchUi ui;
        private bool runtimeInjected;
        private bool pageReady;
        private bool launchAuthorized;
        private float snapshotTimer;

        private void Awake()
        {
            if (ceffyInstance == null)
                ceffyInstance = GetComponent<CeffyInstance>();

            if (ceffyInstance == null)
            {
                Debug.LogError("[GIBBUS] GibbusLaunchHud requires a CeffyInstance on the same GameObject.", this);
                enabled = false;
                return;
            }

            // Set before CeffyInstance starts its browser in OnEnable.
            ceffyInstance.StartUrl = PageUrl;
            ceffyInstance.InputKeyboard = false;
            ceffyInstance.InputMouseMove = true;
            ceffyInstance.InputMouseClick = true;
            ceffyInstance.OnMessageFromCeffy += HandlePageMessage;

            bridge = new CeffyBridge(ceffyInstance);
            bridge.Bind<IGibbusLaunchCommands>(this);
            ui = bridge.CreateProxy<IGibbusLaunchUi>();

            AddLog("GIBBUS launch interface initialised.");
        }

        private void Update()
        {
            if (bridge == null) return;

            bridge.ProcessMessages();

            if (!pageReady) return;

            snapshotTimer += Time.unscaledDeltaTime;
            if (snapshotTimer < 0.25f) return;

            snapshotTimer = 0f;
            ui.UpdateSnapshot(BuildSnapshot());
        }

        private void OnDestroy()
        {
            if (ceffyInstance != null)
                ceffyInstance.OnMessageFromCeffy -= HandlePageMessage;

            bridge?.Dispose();
        }

        private void HandlePageMessage(string message)
        {
            if (message == PageLoadedMessage && !runtimeInjected)
            {
                runtimeInjected = true;
                bridge.InjectBridgeRuntime();
                AddLog("HTML interface loaded. Establishing C# / JavaScript bridge.");
                return;
            }

            if (message == BridgeReadyMessage && !pageReady)
            {
                pageReady = true;
                AddLog("Ceffy bridge online. Mission interface connected.");
                ui.UpdateSnapshot(BuildSnapshot());
                ui.ShowNotice("GIBBUS launch operations connected.");
            }
        }

        // IGibbusLaunchCommands: called by JavaScript.
        public LaunchSnapshot GetSnapshot()
        {
            return BuildSnapshot();
        }

        public void SetChecklistItem(string id, bool acknowledged)
        {
            if (launchAuthorized) return;

            ChecklistEntry entry = checklist.Find(item => item.id == id);
            if (entry == null) return;

            entry.acknowledged = acknowledged;
            AddLog($"{entry.title}: {(acknowledged ? "ACKNOWLEDGED" : "REOPENED")}.");
            PushSnapshot();
        }

        public bool AuthorizeLaunch()
        {
            if (launchAuthorized) return true;

            bool allAcknowledged = checklist.Count > 0 && checklist.TrueForAll(item => item.acknowledged);
            if (!allAcknowledged)
            {
                if (pageReady) ui.ShowNotice("Authorisation denied: checklist is incomplete.");
                AddLog("Launch authorisation request denied. Checklist incomplete.");
                PushSnapshot();
                return false;
            }

            launchAuthorized = true;
            missionPhase = "LAUNCH AUTHORISED";
            AddLog("Launch authorisation accepted. Handing control to Unity launch sequence.");
            onLaunchAuthorized.Invoke();
            PushSnapshot();
            return true;
        }

        public void RequestReturnToDesign()
        {
            AddLog("Return to mission design requested.");
            onReturnToMissionDesign.Invoke();
        }

        // Call these public methods from your launch sequence / telemetry scripts.
        public void SetMissionPhase(string value)
        {
            missionPhase = string.IsNullOrWhiteSpace(value) ? missionPhase : value;
            PushSnapshot();
        }

        public void SetCountdownSeconds(int seconds)
        {
            countdownSeconds = seconds;
            PushSnapshot();
        }

        public void SetTelemetry(float altitude, float velocity, float propellant, float power,
            string communications, string flightTrajectory)
        {
            altitudeKm = Mathf.Max(0f, altitude);
            velocityKmPerSecond = Mathf.Max(0f, velocity);
            propellantPercent = Mathf.Clamp(propellant, 0f, 100f);
            powerPercent = Mathf.Clamp(power, 0f, 100f);
            communicationsStatus = communications;
            trajectory = flightTrajectory;
            PushSnapshot();
        }

        public void AddMissionLog(string message)
        {
            AddLog(message);
            PushSnapshot();
        }

        private LaunchSnapshot BuildSnapshot()
        {
            var checklistData = new LaunchChecklistSnapshot[checklist.Count];
            for (int i = 0; i < checklist.Count; i++)
            {
                ChecklistEntry item = checklist[i];
                checklistData[i] = new LaunchChecklistSnapshot
                {
                    Id = item.id,
                    Title = item.title,
                    Description = item.description,
                    Acknowledged = item.acknowledged
                };
            }

            return new LaunchSnapshot
            {
                MissionCode = missionCode,
                MissionPhase = missionPhase,
                MissionObjective = missionObjective,
                LaunchAuthorized = launchAuthorized,
                CountdownSeconds = countdownSeconds,
                Checklist = checklistData,
                Telemetry = new LaunchTelemetrySnapshot
                {
                    AltitudeKm = altitudeKm,
                    VelocityKmPerSecond = velocityKmPerSecond,
                    PropellantPercent = propellantPercent,
                    PowerPercent = powerPercent,
                    CommunicationsStatus = communicationsStatus,
                    Trajectory = trajectory
                },
                EventLog = eventLog.ToArray()
            };
        }

        private void PushSnapshot()
        {
            if (pageReady && ui != null)
                ui.UpdateSnapshot(BuildSnapshot());
        }

        private void AddLog(string message)
        {
            eventLog.Insert(0, new LaunchLogEntry
            {
                Timestamp = DateTime.Now.ToString("HH:mm:ss"),
                Message = message
            });

            if (eventLog.Count > 12)
                eventLog.RemoveAt(eventLog.Count - 1);
        }

        public bool RequestStageSeparation(string stageId)
        {
            if (ascentStaging == null)
            {
                Debug.LogError("[GIBBUS] Ascent staging controller is not assigned.", this);
                return false;
            }

            bool accepted = ascentStaging.RequestStageSeparation(stageId);

            if (accepted)
            {
                AddLog($"Stage separation command accepted: {stageId}.");
                missionPhase = "ASCENT / STAGING";
                PushSnapshot();
            }
            else if (pageReady)
            {
                ui.ShowNotice("Separation unavailable. Check the next stage.");
            }

            return accepted;
        }
    }
    
    }
