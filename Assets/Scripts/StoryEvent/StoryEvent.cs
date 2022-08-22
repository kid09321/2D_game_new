using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEvent
{
    protected string            m_storyEventName;
    protected StoryEventManager m_storyEventManager;
    protected StoryDialogue     m_storyDialogue;

    public StoryEvent(string storyEventName, StoryDialogue storyDialogue)
    {
        m_storyEventName = storyEventName;
        if (m_storyEventManager == null)
        {
            m_storyEventManager = GameObject.FindObjectOfType<StoryEventManager>();
        }

        m_storyDialogue = storyDialogue;
    }

    public virtual void OnEnter()
    {
        GameManager.Instance.SetCurrentState(GameManager.GameState.Story);
    }

    public virtual void Update(float deltaTime)
    {
        
    }

    public virtual void OnExit()
    {
        m_storyEventManager.SetStoryEvent(null);
        GameManager.Instance.SetCurrentState(GameManager.GameState.Normal);
    }
}
