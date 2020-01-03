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

    public void LoadConversationById(string convoId)
    {
        m_currentConversation = ConversationManager.instance.GetConversationById(convoId);
        if (m_currentConversation != null)
        {
            Debug.Log("Loaded Conversation: " + convoId);
        }
    }
}
