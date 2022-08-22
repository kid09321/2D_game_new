using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossFight : StoryEvent
{
    private Transform m_playerCamera;
    private Vector2   m_camStartPos;
    private Vector2   m_camTargetPos_1;
    private Vector2   m_camTargetPos_2;
    private float     m_camMoveSpeed;
    private StoryDialogue m_storyDialogue;

    public FirstBossFight(string storyEventName, StoryDialogue storyDialogue): base(storyEventName, storyDialogue)
    {
        m_playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        m_camTargetPos_1 = new Vector2(91, 4);
        m_camMoveSpeed = 10f;
        m_camStartPos = m_playerCamera.position;
        m_playerCamera.GetComponent<PlayerCamera>().enabled = false;
        m_storyDialogue = storyDialogue;

    }

    public override void OnEnter()
    {
        base.OnEnter();
        DialogueManager.Instance.SetCurrentDialogue(m_storyDialogue);
    }

    public override void Update(float deltaTime)
    {
        MoveCamera(m_camTargetPos_1, m_camMoveSpeed * deltaTime);
        if (Vector2.Distance(m_camTargetPos_1, m_playerCamera.transform.position) < 0.1f)
        {
            // Camera arrived target position
            Debug.Log("camera Arrived");
            if (DialogueManager.Instance.CurrentState == DialogueManager.DialogueState.Idle) { 
                DialogueManager.Instance.StartDialogue();
            }
            if (DialogueManager.Instance.CurrentState == DialogueManager.DialogueState.Over)
            {
                OnExit();
            }
        }
    }

    public virtual void OnExit()
    {
        base.OnExit();
        m_playerCamera.GetComponent<PlayerCamera>().enabled = true;
    }

    private void MoveCamera(Vector2 targetPos, float step)
    {
        Vector2 newPos = Vector2.MoveTowards(m_playerCamera.transform.position,
            targetPos, step);
        m_playerCamera.transform.position = new Vector3(newPos.x, newPos.y, m_playerCamera.transform.position.z);
    }
}
