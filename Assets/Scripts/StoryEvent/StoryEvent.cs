using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUnit
{
    protected StoryEvent m_storyEvent;
    protected bool       m_finished;
    public EventUnit(StoryEvent storyEvent)
    {
        m_storyEvent = storyEvent;
        m_finished = false;
    }
    public virtual void StartEvent()
    {

    }

    public virtual void Update(float deltaTime)
    {

    }
}

public class StoryEvent
{
    protected string            m_storyEventName;
    protected StoryEventManager m_storyEventManager;
    protected StoryDialogue     m_storyDialogue;
    protected int m_currentEventIndex = 0;
    protected List<EventUnit>   m_events;


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

    public virtual void ExecuteNext()
    {
        if (m_currentEventIndex + 1 < m_events.Count)
        {
            m_currentEventIndex += 1;
            m_events[m_currentEventIndex].StartEvent();
            Debug.Log("m_currentIndex" + m_currentEventIndex);
        }
    }
}
