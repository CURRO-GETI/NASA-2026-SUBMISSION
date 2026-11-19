
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ControlButtons : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI titleToChange;
    [SerializeField] private TextMeshProUGUI textToChange;
    [SerializeField] private TextMeshProUGUI stageToChange;
    [SerializeField] private TextMeshProUGUI pageCounter;

    [Header("Image Reference")]
    [SerializeField] private Image images;
    [SerializeField] private Sprite defaultSprite;

    [Header("Navigation Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    [Header("Panels")]
    [SerializeField] private GameObject beginMissionDesignPanel;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject completedPanel;

    [Header("Mission Information")]
    [SerializeField] private InfoPanel[] infoArray;

    [Header("Events")]
    public UnityEvent onBriefingCompleted;

    private int currentPos = -1;
    private bool briefingComplete;

    // Call this when the director finishes the opening briefing.
    public void ShowBeginMissionDesign()
    {
        if (beginMissionDesignPanel != null)
            beginMissionDesignPanel.SetActive(true);

        if (informationPanel != null)
            informationPanel.SetActive(false);

        if (completedPanel != null)
            completedPanel.SetActive(false);
    }

    // Connect this to the BEGIN MISSION DESIGN button.
    public void ShowObjectives()
    {
        if (infoArray == null || infoArray.Length == 0)
        {
            Debug.LogWarning("ControlButtons: No mission information has been assigned.");
            return;
        }

        currentPos = 0;
        briefingComplete = false;

        if (beginMissionDesignPanel != null)
            beginMissionDesignPanel.SetActive(false);

        if (completedPanel != null)
            completedPanel.SetActive(false);

        if (informationPanel != null)
            informationPanel.SetActive(true);

        DisplayCurrentPanel();
    }

    // Connect this to your NEXT button.
    public void CycleText()
    {
        if (briefingComplete || infoArray == null || infoArray.Length == 0)
            return;

        if (currentPos < 0)
        {
            currentPos = 0;
        }
        else if (currentPos >= infoArray.Length - 1)
        {
            CompleteBriefing();
            return;
        }
        else
        {
            currentPos++;
        }

        DisplayCurrentPanel();
    }

    // Connect this to your BACK button.
    public void PreviousText()
    {
        if (briefingComplete || currentPos <= 0)
            return;

        currentPos--;
        DisplayCurrentPanel();
    }

    private void DisplayCurrentPanel()
    {
        if (currentPos < 0 || currentPos >= infoArray.Length)
            return;

        InfoPanel currentInfo = infoArray[currentPos];

        if (titleToChange != null)
            titleToChange.text = currentInfo.title;

        if (textToChange != null)
            textToChange.text = currentInfo.text;

        if (stageToChange != null)
        {
            stageToChange.text =
                $"STAGE {currentInfo.stageNumber:00} — {currentInfo.stageTitle}";
        }

        if (pageCounter != null)
        {
            pageCounter.text =
                $"{currentPos + 1} / {infoArray.Length}";
        }

        if (images != null)
        {
            Sprite spriteToDisplay = currentInfo.sprite != null
                ? currentInfo.sprite
                : defaultSprite;

            images.sprite = spriteToDisplay;
            images.enabled = spriteToDisplay != null;
        }

        if (previousButton != null)
            previousButton.interactable = currentPos > 0;

        if (nextButton != null)
            nextButton.interactable = true;
    }

    private void CompleteBriefing()
    {
        briefingComplete = true;

        if (informationPanel != null)
            informationPanel.SetActive(false);

        if (completedPanel != null)
            completedPanel.SetActive(true);

        if (nextButton != null)
            nextButton.interactable = false;

        onBriefingCompleted?.Invoke();

        Debug.Log("GIBBUS mission-design briefing completed.");
    }

    // General panel controls.
    public void OpenTab(Transform tab)
    {
        if (tab != null)
            tab.gameObject.SetActive(true);
    }

    public void CloseTab(Transform tab)
    {
        if (tab != null)
            tab.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class InfoPanel
{
    [Header("Stage Information")]
    public int stageNumber;
    public string stageTitle;

    [Header("Panel Content")]
    public string title;
    [TextArea(4, 12)]
    public string text;

    [Header("Panel Image")]
    public Sprite sprite;
}
