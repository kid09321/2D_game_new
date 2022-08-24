using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    public enum GameState { Normal, Story}
    public GameState CurrentState { get; private set; }

    public bool m_canControlPlayer = true;
    public bool m_canMoveCamera = true;
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
}
