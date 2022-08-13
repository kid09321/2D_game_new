using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosManager : MonoBehaviour
{
    [SerializeField] Transform m_groundCheck;
    [SerializeField] Transform m_wallCheck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_wallCheck.position, 0.2f);
    }
}
