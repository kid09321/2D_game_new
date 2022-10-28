using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    [SerializeField] Camera m_playerCamera;

    [SerializeField] StoryEventManager m_storyEventManager;
    [SerializeField] StoryDialogue m_storyDialogue;

    public enum GameState { Normal, Story}
    public GameState CurrentState { get; private set; }

    public bool m_canControlPlayer = true;
    public bool m_canMoveCamera = true;

    private bool m_isFading = false;
    public bool Fading
    {
        get
        {
            return m_isFading;
        }
    }

    public Camera PlayerCamera
    {
        get
        {
            return m_playerCamera;
        }
    }

    [SerializeField] Image m_fade;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Error: More than 1 GameManager");
        }
    }

    void Start()
    {
        SetCurrentState(GameState.Normal);
        FadeIn();
        FirstStory storyEvent = new FirstStory("firstStory", m_storyDialogue);
        m_storyEventManager.SetStoryEvent(storyEvent);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCurrentState(GameState gameState)
    {
        CurrentState = gameState;
        UpdateStateSettings(gameState);
    }

    private void UpdateStateSettings(GameState gameState)
    {
        Debug.Log("Update State" + gameState);
        switch (gameState)
        {
            case GameState.Normal:
                m_canControlPlayer = true;
                m_canMoveCamera = true;
                break;
            case GameState.Story:
                m_canControlPlayer = false;
                m_canMoveCamera = false;
                break;
            default:
                m_canControlPlayer = true;
                m_canMoveCamera = true;
                break;
        }
    }

    public void FadeOut()
    {
        m_isFading = true;
        LockCharacterMovement();
        StartCoroutine(FadeOutAsync());
    }

    IEnumerator FadeOutAsync()
    {
        float secPerLoop = 1.5f / 2000f;
        Color tmpColor = m_fade.color;
        for (int i = 0; i <= 255; i++)
        {
            tmpColor.a = i / 255f;
            m_fade.color = tmpColor;
            yield return new WaitForSeconds(secPerLoop);
        }
        m_isFading = false;
        UnlockCharacterMovement();
    }

    public void FadeIn()
    {
        m_isFading = true;
        LockCharacterMovement();
        StartCoroutine(FadeInAsync());
    }

    IEnumerator FadeInAsync()
    {
        float secPerLoop = 1.5f / 200f;
        Color tmpColor = m_fade.color;
        for (int i = 255; i > 200; i--)
        {
            tmpColor.a = i / 255f;
            m_fade.color = tmpColor;
            yield return new WaitForSeconds(secPerLoop);
        }
        secPerLoop = 1.5f / 10000f;
        for (int i = 200; i >= 0; i-=2)
        {
            tmpColor.a = i / 255f;
            m_fade.color = tmpColor;
            yield return new WaitForSeconds(secPerLoop);
        }
        m_isFading = false;
        //UnlockCharacterMovement();
    }

    void LockCharacterMovement()
    {
        m_canControlPlayer = false;
        m_canMoveCamera = false;
    }

    void UnlockCharacterMovement()
    {
        m_canControlPlayer = true;
        m_canMoveCamera = true;
    }
}
