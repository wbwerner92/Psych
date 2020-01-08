using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationDisplay : MonoBehaviour
{
    // Unity UI References
    public Text textDisplay;
    public GameObject responseBG;
    public GameObject responseObjPrefab;
    public Transform responseParentTransform;
    
    // Conversation Variables
    private ConversationJSON m_currentConversation;
    private ConversationPromptJSON m_currentPrompt;
    private List<ConversationResponseJSON> m_currentResponses;
    private Dictionary <GameObject, ConversationResponseJSON> m_activeResponseObjects;

    // Text Animation Variables
    private float m_deltaTime;
    private float m_frameTime = 0.05f;
    private float m_frame;
    private char m_pendingChar = '>';

    public void LoadConversationById(string convoId)
    {
        m_currentConversation = ConversationManager.instance.GetConversationById(convoId);
        if (m_currentConversation != null)
        {
            Debug.Log("Loaded Conversation: " + m_currentConversation.conversationId);
        }
    }
    public void LoadConversationFile(ConversationJSON convo)
    {
        m_currentConversation = convo;
        if (m_currentConversation != null)
        {
            Debug.Log("Loaded Conversation: " + m_currentConversation.conversationId);
        }
    }
    public void StartConversation()
    {
        if (m_currentConversation == null)
        {
            Debug.LogError("No Conversation loaded");
            return;
        }
        else
        {
            Debug.Log("Start Conversation");
        }

        // Empty text box and reset timer
        ResetTextVariables();

        // Get Default Prompt and all responses
        LoadPrompt(m_currentConversation.conversationTree.GetDefaultPrompt());
        Debug.Log("Got conversation prompt: " + m_currentPrompt.promptIndex);
    }
    public void EndConversation()
    {
        Debug.Log("End Conversation: " + m_currentConversation.conversationId);
        Destroy(this.gameObject);
    }
    private void ResetTextVariables()
    {
        textDisplay.text = "";
        m_deltaTime = 0f;
        m_frame = 0f;
    }
    private void LoadPrompt(ConversationPromptJSON prompt)
    {
       m_currentPrompt = prompt;
       SetCurrentResponses(m_currentPrompt);
    }
    private void SetCurrentResponses(ConversationPromptJSON prompt)
    {
        m_currentResponses = new List<ConversationResponseJSON>();
        foreach (string responseIndex in prompt.responseList)
        {
            m_currentResponses.Add(m_currentConversation.conversationTree.GetResponse(responseIndex));
        }
    }
    private void ClearResponseObjects()
    {
        responseBG.SetActive(false);
        if (m_activeResponseObjects == null)
        {
            m_activeResponseObjects = new Dictionary<GameObject, ConversationResponseJSON>();
            return;
        }
        foreach (GameObject responseObj in m_activeResponseObjects.Keys)
        {
            Destroy(responseObj);
        }
        m_activeResponseObjects.Clear();
    }
    private void PopulateResponseObjects()
    {
        Debug.Log("Populate Responses");
        ClearResponseObjects();
        responseBG.SetActive(true);
        foreach (ConversationResponseJSON response in m_currentResponses)
        {
            GameObject responseObj = Instantiate(responseObjPrefab, responseParentTransform);
            responseObj.name = "Response Object: " + response.responseIndex;
            Text responseText = responseObj.GetComponent<Text>();
            responseText.text = (m_currentResponses.IndexOf(response) + 1) + ". " + response.responseText;
            m_activeResponseObjects.Add(responseObj, response);
        }
    }

    private void HandleResponseActions(ConversationResponseJSON response)
    {
        foreach (string action in response.responseActionList)
        {
            Debug.Log("Response Action: " + action);
            string[] actionArr = action.Split('|');
            string actionKey = actionArr[0];
            switch (actionKey)
            {
                case "prompt":
                    string actionVal = actionArr[1];
                    Debug.Log("Prompt: " + actionVal);
                    LoadPrompt(m_currentConversation.conversationTree.GetPrompt(actionVal));
                    break;
                case "endConversation":
                    EndConversation();
                    break;
                default:
                    break;
            }
        }
    }

    private void ProgressConversation()
    {
        if (m_currentConversation != null && m_currentPrompt != null)
        {
            string fullText = m_currentPrompt.promptText;
            string currentText = textDisplay.text;

            if (currentText.Length >= fullText.Length)
            {
                m_currentPrompt = null;
                PopulateResponseObjects();
            }
            else
            {
                textDisplay.text = fullText + " " + m_pendingChar;
            }
        }
    }

    public void UI_InputAreaClick()
    {
        Debug.Log("Input Area Click Event");
        ProgressConversation();
    }
    public void UI_ResponseObjectClick(GameObject responseObj)
    {
        Debug.Log("Response Object Click Event");

        ConversationResponseJSON response = m_activeResponseObjects[responseObj];
        Debug.Log("Text Option Selected: " + response.responseText);

        ClearResponseObjects();
        textDisplay.text = "";

        HandleResponseActions(response);
    }

    void Update()
    {
        if (ControlsManager.instance.controlEvent == ControlsEvent.RETURN)
        {
            ProgressConversation();
        }
        if (m_currentConversation != null && m_currentPrompt != null)
        {
            string fullText = m_currentPrompt.promptText;
            string currentText = textDisplay.text;

            // Keep track of the time that has passed
            m_deltaTime += Time.deltaTime;
            
            while (m_deltaTime >= m_frameTime) 
            {
                // Track time progression and increase frame count
                m_deltaTime -= m_frameTime;
                m_frame++;

                if (m_frame >= fullText.Length)
                {
                    // Reached the end of the text line
                    // Debug.Log("Reached the end of the Text");
                }	
            }
            // Determine if text is needed to be added to the display
            if (currentText.Length < fullText.Length)
            {
                // Add the next character to the text string
                char nextChar = fullText.ToCharArray()[currentText.Length];
                Debug.Log("Adding character: " + nextChar);
                textDisplay.text = currentText + nextChar;
            }
            else if (currentText.Length == fullText.Length)
            {
                // Handle Pending Char
                Debug.Log("Show Pending character");
                textDisplay.text = fullText + " " + m_pendingChar; 
            }
        }
    }
}
