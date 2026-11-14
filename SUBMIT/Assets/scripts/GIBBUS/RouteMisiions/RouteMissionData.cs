
using System;
using System.Collections.Generic;

public enum RouteChoiceEffect
{
    DiagnosePropulsion,
    MonitorPropulsion,
    UseBackupMode,

    ReduceNonEssentialPower,
    MaintainFullOperations,

    RunNavigationCheck,
    UseCurrentEstimate,

    SendPriorityData,
    SendAllData,

    UseBackupCommunications,
    RetryPrimaryCommunications,

    EnterPowerSavingMode,
    MaintainNormalOperations,

    ConfirmArrivalReadiness
}

[Serializable]
public class RouteChoice
{
    public string label;
    public string result;
    public RouteChoiceEffect effect;

    public RouteChoice(
        string choiceLabel,
        string choiceResult,
        RouteChoiceEffect choiceEffect)
    {
        label = choiceLabel;
        result = choiceResult;
        effect = choiceEffect;
    }
}

[Serializable]
public class RouteEvent
{
    public string title;
    public string description;
    public RouteChoice[] choices;

    public RouteEvent(
        string eventTitle,
        string eventDescription,
        params RouteChoice[] eventChoices)
    {
        title = eventTitle;
        description = eventDescription;
        choices = eventChoices;
    }
}

public static class RouteMissionData
{
    public static List<RouteEvent> GetOutboundEvents()
    {
        return new List<RouteEvent>
        {
            new RouteEvent(
                "PROPULSION ANOMALY",
                "Mission control has detected an unexpected propulsion-system reading. Review the telemetry before continuing.",
                new RouteChoice(
                    "Run diagnostic check",
                    "The diagnostic takes additional time and power, but gives mission control a clearer picture of the propulsion system.",
                    RouteChoiceEffect.DiagnosePropulsion),
                new RouteChoice(
                    "Continue monitoring",
                    "You preserve immediate power, but the uncertainty remains. The mission keeps a reduced propulsion margin.",
                    RouteChoiceEffect.MonitorPropulsion),
                new RouteChoice(
                    "Use backup operating mode",
                    "The spacecraft switches to its backup operating mode. This uses additional power but reduces the uncertainty.",
                    RouteChoiceEffect.UseBackupMode)
            ),

            new RouteEvent(
                "SOLAR ACTIVITY WARNING",
                "The spacecraft has received a warning about increased solar activity. Review the power plan.",
                new RouteChoice(
                    "Reduce non-essential power",
                    "Non-essential systems are placed into a reduced-power state. More battery is preserved for later operations.",
                    RouteChoiceEffect.ReduceNonEssentialPower),
                new RouteChoice(
                    "Maintain full operations",
                    "All normal systems remain active, but the battery reserve decreases.",
                    RouteChoiceEffect.MaintainFullOperations)
            ),

            new RouteEvent(
                "NAVIGATION ASSESSMENT",
                "The navigation display has updated the spacecraft's estimated position. Confirm the next navigation decision.",
                new RouteChoice(
                    "Run navigation check",
                    "The navigation check takes time and a small amount of power, but improves confidence in the current estimate.",
                    RouteChoiceEffect.RunNavigationCheck),
                new RouteChoice(
                    "Use current estimate",
                    "The spacecraft continues using its current estimate. The mission accepts a slightly smaller navigation margin.",
                    RouteChoiceEffect.UseCurrentEstimate)
            )
        };
    }

    public static List<RouteEvent> GetReturnEvents()
    {
        return new List<RouteEvent>
        {
            new RouteEvent(
                "SCIENCE DATA TRANSMISSION",
                "The spacecraft has collected scientific data from the lunar surface. Choose how much data to prioritise for transmission to Earth.",
                new RouteChoice(
                    "Send priority data",
                    "The highest-priority science data is transmitted first, using less power.",
                    RouteChoiceEffect.SendPriorityData),
                new RouteChoice(
                    "Send all available data",
                    "A larger amount of science data is transmitted, using more power.",
                    RouteChoiceEffect.SendAllData)
            ),

            new RouteEvent(
                "COMMUNICATIONS WARNING",
                "Mission control reports an unstable communications link. Choose how to restore reliable communication.",
                new RouteChoice(
                    "Use backup communications",
                    "The spacecraft switches to its alternate communications option, if available.",
                    RouteChoiceEffect.UseBackupCommunications),
                new RouteChoice(
                    "Retry primary communications",
                    "The spacecraft attempts to restore the primary link. This takes additional time and power.",
                    RouteChoiceEffect.RetryPrimaryCommunications)
            ),

            new RouteEvent(
                "POWER MANAGEMENT REVIEW",
                "The spacecraft is preparing for the final part of its journey. Review the remaining power reserve.",
                new RouteChoice(
                    "Enter power-saving mode",
                    "Non-essential systems are reduced to preserve the remaining battery.",
                    RouteChoiceEffect.EnterPowerSavingMode),
                new RouteChoice(
                    "Maintain normal operations",
                    "Normal operations continue, using more of the remaining battery.",
                    RouteChoiceEffect.MaintainNormalOperations)
            ),

            new RouteEvent(
                "EARTH ARRIVAL REVIEW",
                "Mission control is preparing for spacecraft recovery. Review the mission status and confirm arrival readiness.",
                new RouteChoice(
                    "Confirm arrival readiness",
                    "The final route review is complete. Mission control can prepare for recovery.",
                    RouteChoiceEffect.ConfirmArrivalReadiness)
            )
        };
    }
}
