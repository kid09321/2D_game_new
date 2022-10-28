using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDialogueEventUnit : EventUnit
{
    private StoryDialogue m_storyDialogue;
    private Transform     m_playerCameraTrans;
    private float         m_camMoveSpeed = 1f;


    public IntroDialogueEventUnit(StoryEvent storyEvent):base(storyEvent)
    {
        m_storyEvent = storyEvent;
        m_storyDialogue = Resources.Load<StoryDialogue>("Data/StoryDialogue/IntroStory/IntroStory");
        m_playerCameraTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        GameManager.Instance.m_canMoveCamera = false;
    }

    public override void StartEvent()
    {
        DialogueManager.Instance.SetCurrentStoryContext(m_storyDialogue);
        DialogueManager.Instance.StartStoryContext();

    }
    public override void Update(float deltaTime)
    {
        if (m_finished) return;
        MoveCamera(new Vector2(78f, 1.7f), m_camMoveSpeed * deltaTime);

        if (DialogueManager.Instance.CurrentStoryContextState == DialogueManager.DialogueState.Finished && !m_finished)
        {
            m_finished = true;
            m_storyEvent.ExecuteNext();
        }
    }
    private void MoveCamera(Vector2 targetPos, float step)
    {
        Vector2 newPos = Vector2.MoveTowards(m_playerCameraTrans.position,targetPos, step);
        m_playerCameraTrans.position = new Vector3(newPos.x, newPos.y, m_playerCameraTrans.position.z);
    }
}
