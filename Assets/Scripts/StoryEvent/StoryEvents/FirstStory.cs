using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStory : StoryEvent
{
    private Transform m_playerCamera;

    public FirstStory(string storyEventName, StoryDialogue storyDialogue) : base(storyEventName, storyDialogue)
    {
        m_events = new List<EventUnit>(){new IntroDialogueEventUnit(this),
                                         new IntroCharacterMove(this),
                                         new IntroFirstConversation(this),
                                         new IntroEnemyShow(this),
                                         new IntroSecondConversation(this)};
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (m_events.Count > 0)
        {
            m_events[0].StartEvent();
        }
    }

    // Update is called once per frame
    public override void Update(float deltaTime)
    {
        m_events[m_currentEventIndex].Update(Time.deltaTime);
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public override void ExecuteNext()
    {
        base.ExecuteNext();       
    }
}
