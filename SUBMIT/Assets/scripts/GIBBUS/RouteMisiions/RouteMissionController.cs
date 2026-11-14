
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteMissionController : MonoBehaviour
{
    [Header("UI References")]
    public Text eventTitleText;
    public Text eventDescriptionText;
    public Text resultText;
    public Text statusText;

    [Tooltip("Add three buttons in the Inspector.")]
    public Button[] choiceButtons;

    [Tooltip("Button shown when the route events are complete.")]
    public Button continueButton;

    private MissionStateManager missionManager;
    private List<RouteEvent> currentEvents;
    private bool isOutbound;

    private void Start()
    {
        missionManager = MissionStateManager.Instance;

        if (missionManager == null) {
            Debug.LogError(
                "MissionStateManager is missing. Add it to your first scene."
            );
            enabled = false;
            return;
        }

        if (choiceButtons == null || choiceButtons.Length < 3) {
            Debug.LogError(
                "Assign at least three choice buttons in the Inspector."
            );
            enabled = false;
            return;
        }

        for (int i = 0; i < choiceButtons.Length; i++) {
            int buttonIndex = i;
            choiceButtons[i].onClick.AddListener(
                () => ChooseOption(buttonIndex)
            );
        }

        if (continueButton != null) {
            continueButton.onClick.AddListener(FinishRoute);
        }

        RefreshRoute();
    }

    public void RefreshRoute()
    {
        if (missionManager == null)
            return;

        MissionPhase phase = missionManager.State.currentPhase;

        if (phase == MissionPhase.OutboundCruise) {
            isOutbound = true;
            currentEvents = RouteMissionData.GetOutboundEvents();
            DisplayCurrentEvent();
        } else if (phase == MissionPhase.EarthReturn) {
            isOutbound = false;
            currentEvents = RouteMissionData.GetReturnEvents();
            DisplayCurrentEvent();
        } else {
            ShowWaitingMessage(
                "No route event is active in this mission phase."
            );
        }
    }

    private int GetCurrentEventIndex()
    {
        return isOutbound
            ? missionManager.State.outboundEventIndex
            : missionManager.State.returnEventIndex;
    }

    private void SetCurrentEventIndex(int value)
    {
        if (isOutbound)
            missionManager.State.outboundEventIndex = value;
        else
            missionManager.State.returnEventIndex = value;
    }

    private void DisplayCurrentEvent()
    {
        int eventIndex = GetCurrentEventIndex();

        if (eventIndex >= currentEvents.Count) {
            DisplayRouteComplete();
            return;
        }

        RouteEvent routeEvent = currentEvents[eventIndex];

        eventTitleText.text = routeEvent.title;
        eventDescriptionText.text = routeEvent.description;

        if (resultText != null)
            resultText.text = "";

        UpdateStatus();

        for (int i = 0; i < choiceButtons.Length; i++) {
            bool hasChoice = i < routeEvent.choices.Length;

            choiceButtons[i].gameObject.SetActive(hasChoice);

            if (hasChoice) {
                Text buttonText =
                    choiceButtons[i].GetComponentInChildren<Text>();

                if (buttonText != null)
                    buttonText.text = routeEvent.choices[i].label;

                choiceButtons[i].interactable = true;
            }
        }

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);
    }

    private void ChooseOption(int choiceIndex)
    {
        int eventIndex = GetCurrentEventIndex();

        if (eventIndex >= currentEvents.Count)
            return;

        RouteEvent routeEvent = currentEvents[eventIndex];

        if (choiceIndex < 0 ||
            choiceIndex >= routeEvent.choices.Length)
            return;

        foreach (Button button in choiceButtons)
            button.interactable = false;

        RouteChoice selectedChoice = routeEvent.choices[choiceIndex];

        ApplyChoice(selectedChoice.effect);

        missionManager.RecordResult(selectedChoice.result);

        if (resultText != null)
            resultText.text = selectedChoice.result;

        SetCurrentEventIndex(eventIndex + 1);
        missionManager.SaveMission();

        Invoke(nameof(DisplayCurrentEvent), 2f);
    }

    private void ApplyChoice(RouteChoiceEffect effect)
    {
        MissionRuntimeState state = missionManager.State;

        switch (effect) {
            case RouteChoiceEffect.DiagnosePropulsion:
                state.batteryKwh -= 3f;
                state.extraRouteMinutes += 10f;
                state.propulsionDiagnosed = true;
                break;

            case RouteChoiceEffect.MonitorPropulsion:
                state.propulsionMargin -= 8f;
                state.communicationsDegraded = true;
                break;

            case RouteChoiceEffect.UseBackupMode:
                if (state.hasBackupMode) {
                    state.batteryKwh -= 5f;
                    state.backupModeUsed = true;
                    state.propulsionDiagnosed = true;
                } else {
                    state.lastEventResult =
                        "Backup mode was not installed. The spacecraft continues monitoring.";
                    state.propulsionMargin -= 5f;
                    state.communicationsDegraded = true;
                }
                break;

            case RouteChoiceEffect.ReduceNonEssentialPower:
                state.batteryKwh += 5f;
                state.powerSavingEnabled = true;
                break;

            case RouteChoiceEffect.MaintainFullOperations:
                state.batteryKwh -= 8f;
                break;

            case RouteChoiceEffect.RunNavigationCheck:
                state.batteryKwh -= 2f;
                state.extraRouteMinutes += 5f;
                break;

            case RouteChoiceEffect.UseCurrentEstimate:
                state.propulsionMargin -= 5f;
                break;

            case RouteChoiceEffect.SendPriorityData:
                state.scienceDataReturnedMb += 40f;
                state.batteryKwh -= 3f;
                break;

            case RouteChoiceEffect.SendAllData:
                state.scienceDataReturnedMb +=
                    state.scienceDataCollectedMb;
                state.batteryKwh -= 8f;
                break;

            case RouteChoiceEffect.UseBackupCommunications:
                if (state.hasBackupMode) {
                    state.batteryKwh -= 5f;
                    state.communicationsDegraded = false;
                } else {
                    state.batteryKwh -= 2f;
                    state.extraRouteMinutes += 15f;
                }
                break;

            case RouteChoiceEffect.RetryPrimaryCommunications:
                state.batteryKwh -= 5f;
                state.extraRouteMinutes += 15f;
                state.communicationsDegraded = false;
                break;

            case RouteChoiceEffect.EnterPowerSavingMode:
                state.batteryKwh -= 2f;
                state.powerSavingEnabled = true;
                break;

            case RouteChoiceEffect.MaintainNormalOperations:
                state.batteryKwh -= 8f;
                break;

            case RouteChoiceEffect.ConfirmArrivalReadiness:
                state.lastEventResult =
                    "Arrival readiness confirmed.";
                break;
        }

        // Keep all game values within sensible prototype limits.
        state.batteryKwh = Mathf.Clamp(state.batteryKwh, 0f, 100f);
        state.propulsionMargin =
            Mathf.Clamp(state.propulsionMargin, 0f, 100f);

        state.scienceDataReturnedMb = Mathf.Clamp(
            state.scienceDataReturnedMb,
            0f,
            state.scienceDataCollectedMb
        );

        // Route events must not cause a crew fatality.
        state.crewSafe = true;

        missionManager.SaveMission();
    }

    private void DisplayRouteComplete()
    {
        eventTitleText.text = isOutbound
            ? "OUTBOUND JOURNEY COMPLETE"
            : "EARTH RETURN JOURNEY COMPLETE";

        eventDescriptionText.text = isOutbound
            ? "The spacecraft has completed its outbound route checks. Mission control is ready for lunar orbit operations."
            : "The return route checks are complete. Mission control is ready to prepare the recovery phase.";

        UpdateStatus();

        if (resultText != null)
            resultText.text = "All route events completed.";

        foreach (Button button in choiceButtons)
            button.gameObject.SetActive(false);

        if (continueButton != null)
            continueButton.gameObject.SetActive(true);
    }

    private void FinishRoute()
    {
        if (isOutbound)
            missionManager.SetPhase(MissionPhase.LunarOrbit);
        else
            missionManager.SetPhase(MissionPhase.Recovery);

        ShowWaitingMessage("Mission phase updated.");
    }

    private void UpdateStatus()
    {
        if (statusText == null)
            return;

        MissionRuntimeState state = missionManager.State;

        statusText.text =
            "BATTERY: " + state.batteryKwh.ToString("0") + " kWh\n" +
            "PROPULSION MARGIN: " +
            state.propulsionMargin.ToString("0") + "%\n" +
            "EXTRA ROUTE TIME: " +
            state.extraRouteMinutes.ToString("0") + " min\n" +
            "SCIENCE DATA RETURNED: " +
            state.scienceDataReturnedMb.ToString("0") + " MB\n" +
            "CREW STATUS: SAFE";
    }

    private void ShowWaitingMessage(string message)
    {
        if (eventTitleText != null)
            eventTitleText.text = message;

        if (eventDescriptionText != null)
            eventDescriptionText.text = "";

        foreach (Button button in choiceButtons) {
            if (button != null)
                button.gameObject.SetActive(false);
        }

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);
    }
}
