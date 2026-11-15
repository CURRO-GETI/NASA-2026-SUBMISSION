using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "MissionDesignSession",
    menuName = "GIBBUS/Mission Design/Session"
)]
public class MissionDesignSession : ScriptableObject
{
    [Serializable]
    public class DecisionRecord
    {
        public string decisionKey;
        public string selectedOptionID;
    }

    [SerializeField]
    private List<DecisionRecord> decisions = new List<DecisionRecord>();

    public void SetDecision(string key, string optionID)
    {
        DecisionRecord record = decisions.Find(
            x => x.decisionKey == key
        );

        if (record == null)
        {
            record = new DecisionRecord();
            record.decisionKey = key;
            decisions.Add(record);
        }

        record.selectedOptionID = optionID;
    }

    public string GetDecision(string key)
    {
        DecisionRecord record = decisions.Find(
            x => x.decisionKey == key
        );

        return record != null ? record.selectedOptionID : "";
    }

    public void ResetSession()
    {
        decisions.Clear();
    }
}