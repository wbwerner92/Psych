using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ConversationJSON
{
    public string conversationId;
    public string[] participationList;
    public ConversationTreeJSON conversationTree;

    public static ConversationJSON CreateFromJSON(string conversationStr)
    {
        // Debug.Log("Convo Str: " + conversationStr);
        JSONNode node = JSON.Parse(conversationStr);
        ConversationJSON conversationJSON = JsonUtility.FromJson<ConversationJSON>(node.ToString());
        conversationJSON.conversationTree = ConversationTreeJSON.CreateFromJSON(node["conversationTree"].ToString());
        return conversationJSON;
    }

    public string GetFieldValues()
    {
        string val = "";
        foreach (ConversationPromptJSON prompt in conversationTree.prompts)
        {
            val += prompt.GetFieldValues();
        }
        foreach (ConversationResponseJSON response in conversationTree.responses)
        {
            val += response.GetFieldValues();
        }
        return val;
    }
}
public class ConversationTreeJSON
{
    public string defaultPrompt;
    public List<ConversationPromptJSON> prompts;
    public List<ConversationResponseJSON> responses;

    public static ConversationTreeJSON CreateFromJSON(string treeStr)
    {
        // Debug.Log("Tree String: " + treeStr);
        JSONNode node = JSON.Parse(treeStr);
        ConversationTreeJSON treeJSON = JsonUtility.FromJson<ConversationTreeJSON>(node.ToString());

        // Prompts
        treeJSON.prompts = new List<ConversationPromptJSON>();
        foreach (JSONNode promptNode in node["prompts"].AsArray)
        {
            treeJSON.prompts.Add(ConversationPromptJSON.CreateFromJSON(promptNode.ToString()));
        }
        // Responses
        treeJSON.responses = new List<ConversationResponseJSON>();
        foreach (JSONNode responseNode in node["responses"].AsArray)
        {
            treeJSON.responses.Add(ConversationResponseJSON.CreateFromJSON(responseNode.ToString()));
        }

        return treeJSON;
    }
}
public class ConversationPromptJSON
{
    public string promptIndex;
    public string speakerId;
    public string promptText;
    public string[] responseList;

    public static ConversationPromptJSON CreateFromJSON(string promptStr)
    {
        return JsonUtility.FromJson<ConversationPromptJSON>(promptStr);
    }

    public string GetFieldValues()
    {
        string val = "";
        val += "Prompt Index: " + promptIndex + "\n";
        val += "Speaker Id: " + speakerId + "\n";
        val += "Prompt Text: " + promptText + "\n";
        foreach (string response in responseList)
        {
            val += "Response: " + response + "\n";
        }
        return val;
    }
}
public class ConversationResponseJSON
{
    public string responseIndex;
    public string responseText;
    public string[] responseActionList;

    public static ConversationResponseJSON CreateFromJSON(string responseStr)
    {
        return JsonUtility.FromJson<ConversationResponseJSON>(responseStr);
    }
    public string GetFieldValues()
    {
        string val = "";
        val += "Response Index: " + responseIndex + "\n";
        val += "Response Text: " + responseText + "\n";
        foreach (string action in responseActionList)
        {
            val += "Response Action: " + action + "\n";
        }
        return val;
    }
}

public class ConversationManager : ManagerClass
{
    public static ConversationManager instance;

    // Unity Object References
    public GameObject ConversationDisplayPrefab;
    private ConversationDisplay m_activeConversationDisplay;
    public Transform uiParentTransform;

    // Conversation Variables
    private string conversationFilePath = "Files/ConversationFiles";
    private Dictionary<string, ConversationJSON> conversationFilesById;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LoadConversationResources();
        Initialize();
    }

    public void LoadConversationResources()
    {
        conversationFilesById = new Dictionary<string, ConversationJSON>();

        TextAsset[] conversationFileAssets = Resources.LoadAll<TextAsset>(conversationFilePath);
        // Debug.Log("Number of Conversation Assets: " + conversationFileAssets.Length);
        foreach (TextAsset conversationAsset in conversationFileAssets)
        {
            // Debug.Log("Conversation text asset: " + conversationAsset.text);
            ConversationJSON conversation = ConversationJSON.CreateFromJSON(conversationAsset.text);
            if (conversation != null)
            {
                // Debug.Log("Conversation: " + conversation.GetFieldValues());
                conversationFilesById.Add(conversation.conversationId, conversation);
            }
        }
        Debug.Log("Loaded: " + conversationFilesById.Keys.Count + " Conversations");
    }

    /// <summary>
    /// Return a conversation file given the Conversation Id String
    /// </summary>
    /// <param name="convoId">Conversation Id String</param>
    public ConversationJSON GetConversationById(string convoId)
    {
        if (conversationFilesById == null || conversationFilesById.ContainsKey(convoId) == false)
        {
            Debug.LogError("Cannot Retrieve Conversation with Id: " + convoId);
            return null;
        }

        return conversationFilesById[convoId];
    }

    public void StartConversation(string convoId)
    {
        if (m_activeConversationDisplay == null)
        {
            m_activeConversationDisplay = Instantiate(ConversationDisplayPrefab, uiParentTransform).GetComponent<ConversationDisplay>();
        }

        m_activeConversationDisplay.LoadConversationById(convoId);
    }
}
