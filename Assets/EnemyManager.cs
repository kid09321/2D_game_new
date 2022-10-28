using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    static public EnemyManager Instance;

    [SerializeField] GameObject m_BoD;
    public GameObject BoD
    {
        get
        {
            return m_BoD;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Error: More than 1 GameManager");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
