using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationDisplay : MonoBehaviour
{
    // Unity UI References
    public Text textDisplay;
    // Test UI References
    
    // Conversation Variables
    private ConversationJSON m_currentConversation;
    private ConversationPromptJSON m_currentPrompt;

    // Text Animation Variables
    private float m_deltaTime;
    private float m_frameTime = 0.05f;
    private float m_frame;

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

    private void ResetTextVariables()
    {
        textDisplay.text = "";
        m_deltaTime = 0f;
        m_frame = 0f;
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

        // Get Default Prompt
        m_currentPrompt = m_currentConversation.conversationTree.GetDefaultPrompt();
        Debug.Log("Got conversation prompt: " + m_currentPrompt.promptIndex);
    }

    void Update()
    {
        if (m_currentConversation != null && m_currentPrompt != null)
        {
            string fullText = m_currentPrompt.promptText;
            string currentText = textDisplay.text;

            // Determine if text is needed to be added to the display
            if (currentText.Length < fullText.Length)
            {
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
                        Debug.Log("Reached the end of the Text");
                    }	
                }
                // Add the next character to the text string
                char nextChar = fullText.ToCharArray()[currentText.Length];
                Debug.Log("Adding character: " + nextChar);
                textDisplay.text = currentText + nextChar;
            }
        }
    }
}
