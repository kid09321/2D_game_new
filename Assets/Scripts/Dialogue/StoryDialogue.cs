using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryDialogue", menuName = "ScriptableObjects/StoryDialogue")]
public class StoryDialogue : ScriptableObject
{
    [SerializeField] List<DialogueContent> m_storyDialuges;

    public List<DialogueContent> GetStoryDialogue()
    {
        return m_storyDialuges;
    }
}
