using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogueContent", menuName = "ScriptableObjects/DialogueContent")]
public class DialogueContent : ScriptableObject
{
    [SerializeField] string m_content = "Default dialogue content.";
    [SerializeField] Sprite m_speakerImage;
    
    public string GetContent()
    {
        return m_content;
    }

    public Sprite GetSpeaker()
    {
        return m_speakerImage;
    }

}
