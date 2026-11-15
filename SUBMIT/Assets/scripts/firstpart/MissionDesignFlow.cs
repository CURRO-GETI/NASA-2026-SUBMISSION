using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDesignFlow : MonoBehaviour
{
    [Header("Mission Stages")]
    [SerializeField] private MissionDesignStage[] stages;

    [SerializeField] private MissionDesignSession session;

    [Header("Main Panels")]
    [SerializeField] private GameObject beginPanel;
    [SerializeField] private GameObject designPanel;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject decisionPanel;
    [SerializeField] private GameObject completionPanel;

    [Header("Stage Information UI")]
    [SerializeField] private TMP_Text stageTitleText;
    [SerializeField] private TMP_Text stageSubtitleText;
    [SerializeField] private TMP_Text stageCounterText;

    [Header("Information Page UI")]
    [SerializeField] private TMP_Text pageHeadingText;
    [SerializeField] private TMP_Text pageDescriptionText;
    [SerializeField] private TMP_Text pageCounterText;
    [SerializeField] private Image pageImage;

    [Header("Decision UI")]
    [SerializeField] private TMP_Text decisionPromptText;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private MissionChoiceButton choiceButtonPrefab;
    [SerializeField] private Button confirmDecisionButton;

    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text nextButtonText;
    [SerializeField] private Slider progressSlider;

    private int currentStageIndex;
    private int currentPageIndex;
    private int selectedChoiceIndex = -1;

    private readonly List<MissionChoiceButton> spawnedChoices =
        new List<MissionChoiceButton>();

    private MissionDesignStage CurrentStage =>
        stages[currentStageIndex];

    private void Start()
    {
        if (stages == null || stages.Length == 0)
        {
            Debug.LogError(
                "MissionDesignFlow: No mission stages assigned!"
            );
            enabled = false;
            return;
        }

        if (session == null)
        {
            Debug.LogError(
                "MissionDesignFlow: MissionDesignSession is missing!"
            );
            enabled = false;
            return;
        }

        if (beginPanel != null)
            beginPanel.SetActive(false); //Edited by Tino true to false

        if (designPanel != null)
            designPanel.SetActive(false);

        if (completionPanel != null)
            completionPanel.SetActive(false);

        if (confirmDecisionButton != null)
        {
            confirmDecisionButton.interactable = false;
        }
    }

    // Called by BEGIN MISSION DESIGN
    public void BeginMissionDesign()
    {
        currentStageIndex = 0;
        currentPageIndex = 0;

        session.ResetSession();

        beginPanel.SetActive(false);
        designPanel.SetActive(true);

        ShowStage();
    }

    private void ShowStage()
    {
        if (currentStageIndex >= stages.Length)
        {
            CompleteMissionDesign();
            return;
        }

        currentPageIndex = 0;
        selectedChoiceIndex = -1;

        ClearChoices();

        decisionPanel.SetActive(false);
        informationPanel.SetActive(true);

        stageTitleText.text = CurrentStage.stageTitle;
        stageSubtitleText.text = CurrentStage.stageSubtitle;

        UpdateStageCounter();
        ShowPage();
    }

    private void ShowPage()
    {
        MissionInfoPage page =
            CurrentStage.pages[currentPageIndex];

        pageHeadingText.text = page.heading;
        pageDescriptionText.text = page.description;

        if (pageImage != null)
        {
            pageImage.sprite = page.image;

            pageImage.gameObject.SetActive(
                page.image != null
            );
        }

        pageCounterText.text =
            $"PAGE {currentPageIndex + 1:00} / " +
            $"{CurrentStage.pages.Length:00}";

        backButton.interactable =
            currentPageIndex > 0;

        bool isLastPage =
            currentPageIndex >= CurrentStage.pages.Length - 1;

        if (isLastPage && CurrentStage.HasDecision)
        {
            nextButtonText.text = "REVIEW DECISION";
        }
        else if (isLastPage)
        {
            nextButtonText.text = "COMPLETE STAGE";
        }
        else
        {
            nextButtonText.text = "NEXT";
        }
    }

    // Called by NEXT
    public void NextPage()
    {
        if (currentPageIndex < CurrentStage.pages.Length - 1)
        {
            currentPageIndex++;
            ShowPage();
            return;
        }

        if (CurrentStage.HasDecision)
        {
            ShowDecision();
        }
        else
        {
            CompleteStage();
        }
    }

    // Called by BACK
    public void PreviousPage()
    {
        if (decisionPanel.activeSelf)
        {
            decisionPanel.SetActive(false);
            informationPanel.SetActive(true);

            ShowPage();
            return;
        }

        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage();
        }
    }

    private void ShowDecision()
    {
        informationPanel.SetActive(false);
        decisionPanel.SetActive(true);

        decisionPromptText.text =
            CurrentStage.decisionPrompt;

        selectedChoiceIndex = -1;

        confirmDecisionButton.interactable = false;

        ClearChoices();

        for (int i = 0; i < CurrentStage.decisionOptions.Length; i++)
        {
            int index = i;

            MissionDecisionOption option =
                CurrentStage.decisionOptions[i];

            MissionChoiceButton newButton =
                Instantiate(choiceButtonPrefab, choiceContainer);

            newButton.Setup(
                option.optionName,
                option.description,
                () => SelectChoice(index)
            );

            spawnedChoices.Add(newButton);
        }
    }

    private void SelectChoice(int index)
    {
        selectedChoiceIndex = index;

        for (int i = 0; i < spawnedChoices.Count; i++)
        {
            spawnedChoices[i].SetSelected(i == index);
        }

        confirmDecisionButton.interactable = true;
    }

    // Called by CONFIRM DECISION
    public void ConfirmDecision()
    {
        if (selectedChoiceIndex < 0)
            return;

        MissionDecisionOption selectedOption =
            CurrentStage.decisionOptions[selectedChoiceIndex];

        session.SetDecision(
            CurrentStage.decisionKey,
            selectedOption.optionID
        );

        CompleteStage();
    }

    private void CompleteStage()
    {
        currentStageIndex++;

        if (currentStageIndex >= stages.Length)
        {
            CompleteMissionDesign();
            return;
        }

        ShowStage();
    }

    private void CompleteMissionDesign()
    {
        designPanel.SetActive(false);

        if (completionPanel != null)
            completionPanel.SetActive(true);

        Debug.Log("GIBBUS: Mission design stages completed.");
    }

    private void UpdateStageCounter()
    {
        stageCounterText.text =
            $"STAGE {currentStageIndex + 1:00} / {stages.Length:00}";

        if (progressSlider != null)
        {
            progressSlider.value =
                (float)(currentStageIndex + 1) / stages.Length;
        }
    }

    private void ClearChoices()
    {
        foreach (MissionChoiceButton choice in spawnedChoices)
        {
            if (choice != null)
                Destroy(choice.gameObject);
        }

        spawnedChoices.Clear();
    }

    public void ShowBeginMissionDesign()
    {
        beginPanel.SetActive(true);
    }
}