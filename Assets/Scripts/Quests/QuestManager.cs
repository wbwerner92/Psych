using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class QuestJSON
{
    public string questId;
    public string questName;
    public string location;
    public string questGiverId;
    public string description;
    public QuestRewardsJSON rewards;
    public string[] blockedBy;

    public static QuestJSON CreateFromJSON(string questStr)
    {
        JSONNode questNode = JSON.Parse(questStr);
        QuestJSON questJSON = JsonUtility.FromJson<QuestJSON>(questStr);
        questJSON.rewards = JsonUtility.FromJson<QuestRewardsJSON>(questNode["rewards"].ToString());
        return questJSON;
    }

    public string GetFieldValues()
    {
        string val = "";
        val += "Quest Id: " + questId + "\n";
        val += "Quest Name: " + questName + "\n";
        val += "Location: " + location + "\n";
        val += "Quest Giver Id: " + questGiverId + "\n";
        val += "Description: " + description + "\n";
        if (rewards != null)
        {
            val += "Money Reward: " + rewards.money + "\n";
            val += "Exp Reward: " + rewards.exp + "\n";
            foreach(string str in rewards.items)
            {
                val += "Item Reward: " + str + "\n";
            }
        }
        foreach (string str in blockedBy)
        {
            val += "Blocked by quest: " + str + "\n";
        }

        return val;
    }
}
public class QuestRewardsJSON
{
    public string money;
    public string exp;
    public string[] items;
}
public class QuestLineJSON
{
    public string questLineId;
    public string questLineName;
    public string description;
    public string[] questList;

    public static QuestLineJSON CreateFromJSON(string questLineStr)
    {
        QuestLineJSON questLineJSON = JsonUtility.FromJson<QuestLineJSON>(questLineStr);
        return questLineJSON;
    }

    public string GetFieldValues()
    {
        string val = "";
        val += "Quest Line Id: " + questLineId + "\n";
        val += "Quest Line Name: " + questLineName + "\n";
        val += "Description: " + description + "\n";
        foreach (string str in questList)
        {
            val += "Quest in QuestLine: " + str + "\n";
        }

        return val;
    }
}

public class QuestManager : ManagerClass
{
    public static QuestManager instance;

    private string questFilePath = "Files/QuestFiles/Quests";
    private string questLineFilePath = "Files/QuestFiles/QuestLines";
    private Dictionary<string, QuestJSON> questFilesById;
    private Dictionary<string, QuestLineJSON> questLineFilesById;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // Debug.Log("QuestManager - Start");
        LoadQuestResources();
        Initialize();
    }

    public void LoadQuestResources()
    {
        questFilesById = new Dictionary<string, QuestJSON>();

        TextAsset[] questFileAssets = Resources.LoadAll<TextAsset>(questFilePath);
        // Debug.Log("Number of Quest Assets: " + questFileAssets.Length);
        foreach (TextAsset questAsset in questFileAssets)
        {
            // Debug.Log("Quest text asset: " + questAsset.text);
            QuestJSON quest = QuestJSON.CreateFromJSON(questAsset.text);
            if (quest != null)
            {
                // Debug.Log("Quest: " + quest.GetFieldValues());
                questFilesById.Add(quest.questId, quest);
            }
        }
        Debug.Log("Loaded: " + questFilesById.Keys.Count + " Quests");
        
        questLineFilesById = new Dictionary<string, QuestLineJSON>();
        TextAsset[] questLineAssets = Resources.LoadAll<TextAsset>(questLineFilePath);
        foreach(TextAsset questLineAsset in questLineAssets)
        {
            QuestLineJSON questLine = QuestLineJSON.CreateFromJSON(questLineAsset.text);
            if (questLine != null)
            {
                // Debug.Log("Quest Line: " + questLine.GetFieldValues());
                questLineFilesById.Add(questLine.questLineId, questLine);
            }
        }
        Debug.Log("Loaded: " + questLineFilesById.Keys.Count + " Quest Lines");
    }
}
