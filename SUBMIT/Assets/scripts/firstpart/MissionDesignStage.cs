using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewMissionStage",
    menuName = "GIBBUS/Mission Design/Stage"
)]
public class MissionDesignStage : ScriptableObject
{
    [Header("Stage Information")]
    public string stageTitle;

    [TextArea(2, 4)]
    public string stageSubtitle;

    [Header("Information Pages")]
    public MissionInfoPage[] pages;

    [Header("Player Decision (Optional)")]
    [TextArea(2, 4)]
    public string decisionPrompt;

    public string decisionKey;

    public MissionDecisionOption[] decisionOptions;

    public bool HasDecision =>
        decisionOptions != null && decisionOptions.Length > 0;
}

[Serializable]
public class MissionInfoPage
{
    public string heading;

    [TextArea(4, 12)]
    public string description;

    public Sprite image;
}

[Serializable]
public class MissionDecisionOption
{
    public string optionID;
    public string optionName;

    [TextArea(2, 5)]
    public string description;
}