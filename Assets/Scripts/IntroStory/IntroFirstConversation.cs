using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroFirstConversation : EventUnit
{
    private StoryDialogue m_storyDialogue;

    public IntroFirstConversation(StoryEvent storyEvent) : base(storyEvent)
    {
        m_storyEvent = storyEvent;
        m_storyDialogue = Resources.Load<StoryDialogue>("Data/StoryDialogue/IntroSecondDialogue/IntroSecondDialogue");

    }

    public override void StartEvent()
    {
        DialogueManager.Instance.SetCurrentDialogue(m_storyDialogue);
        DialogueManager.Instance.StartDialogue();   
    }

    public override void Update(float deltaTime)
    {
        if (m_finished) return;
        if (DialogueManager.Instance.CurrentState == DialogueManager.DialogueState.Over)
        {
            m_finished = true;
            m_storyEvent.ExecuteNext();
        }
    }
}
