using System;
using System.Collections.Generic;

namespace GIBBUS.LaunchUI
{
    [Serializable]
    public class LaunchChecklistSnapshot
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Acknowledged { get; set; }
    }

    [Serializable]
    public class LaunchTelemetrySnapshot
    {
        public float AltitudeKm { get; set; }
        public float VelocityKmPerSecond { get; set; }
        public float PropellantPercent { get; set; }
        public float PowerPercent { get; set; }
        public string CommunicationsStatus { get; set; }
        public string Trajectory { get; set; }
    }

    [Serializable]
    public class LaunchLogEntry
    {
        public string Timestamp { get; set; }
        public string Message { get; set; }
    }

    [Serializable]
    public class LaunchSnapshot
    {
        public string MissionCode { get; set; }
        public string MissionPhase { get; set; }
        public string MissionObjective { get; set; }
        public bool LaunchAuthorized { get; set; }
        public int CountdownSeconds { get; set; }
        public LaunchChecklistSnapshot[] Checklist { get; set; }
        public LaunchTelemetrySnapshot Telemetry { get; set; }
        public LaunchLogEntry[] EventLog { get; set; }
    }
}
