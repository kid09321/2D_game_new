using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    static public DialogueManager Instance;
    public StoryDialogue CurrentDialogue { get; private set; }
    public enum DialogueState { Idle, NormalDisplaying, FastDisplaying, Finished, Over}

    //
    [SerializeField] TextMeshProUGUI m_TMP;
    [SerializeField] GameObject      m_dialogueUI;
    [SerializeField] Image           m_dialogueImage;
    //
    private int             m_lineCounter = 0;
    private bool            m_lineFinished = false;
    private DialogueState   m_currentState = DialogueState.Idle;

    public DialogueState CurrentState
    {
        get{
            return m_currentState;
        }
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Error: More than 1 DialogueManager");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_lineCounter = 0;
        m_currentState = DialogueState.Idle;
        m_dialogueUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentState == DialogueState.Idle) return;
        if (Input.GetMouseButtonDown(0))
        {
            switch (m_currentState)
            {
                case DialogueState.NormalDisplaying:
                    m_currentState = DialogueState.FastDisplaying;
                    break;
                case DialogueState.FastDisplaying:
                    break;
                case DialogueState.Finished:
                    m_lineCounter++;
                    StartDialogue();
                    m_currentState = DialogueState.NormalDisplaying;
                    break; 
            }
            
        }
        if(m_lineCounter >= CurrentDialogue.GetStoryDialogue().Count)
        {
            m_currentState = DialogueState.Over;
        }
        if(m_currentState == DialogueState.Over)
        {
            m_dialogueUI.SetActive(false);
        }
    }

    public void SetCurrentDialogue(StoryDialogue storyDialogue)
    {
        m_lineCounter = 0;
        m_currentState = DialogueState.Idle;
        //m_dialogueImage.sprite = null;
        CurrentDialogue = storyDialogue;
    }

    public void StartDialogue()
    {
        m_dialogueUI.SetActive(true);
        m_currentState = DialogueState.NormalDisplaying;
        List<DialogueContent> dialogues = CurrentDialogue.GetStoryDialogue();
        m_lineFinished = false;
        if (m_lineCounter < dialogues.Count)
        {
            StartCoroutine("DisplayDialogue", dialogues[m_lineCounter]);
            m_dialogueImage.sprite = dialogues[m_lineCounter].GetSpeaker();
        }
    }

    IEnumerator DisplayDialogue(DialogueContent dialogueContent)
    {
        string content = dialogueContent.GetContent();
        string displayContent = "";
        for (int i = 0; i < content.Length; i++)
        {
            displayContent += content[i];
            m_TMP.text = displayContent;
            if (m_currentState == DialogueState.NormalDisplaying)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return null;
            }
        }
        m_lineFinished = true;
        m_currentState = DialogueState.Finished;
    }
}
