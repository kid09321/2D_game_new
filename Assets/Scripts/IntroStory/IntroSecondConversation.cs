using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSecondConversation : EventUnit
{
    StoryDialogue m_storyDialogue;

    public IntroSecondConversation(StoryEvent storyEvent): base(storyEvent)
    {
        m_storyEvent = storyEvent;
        m_storyDialogue = Resources.Load<StoryDialogue>("Data/StoryDialogue/IntroThirdDialogue/IntroThirdDialogue");
    }


    public override void StartEvent()
    {
        base.StartEvent();
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
