using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private Animator m_animator;
    private bool     m_switch;  // Use to turn on/off the saw.
    private List<Collider2D> damagedColliders;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_switch = true;
        m_animator.SetBool("Switch", m_switch);
        damagedColliders = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !damagedColliders.Contains(collision))
        {
            damagedColliders.Add(collision);
            PrototypeHeroDemo player = collision.gameObject.GetComponent<PrototypeHeroDemo>();
            player.Damaged(5, this.gameObject);
            StartCoroutine(RemoveDamagedCollider(collision));
        }
    }

    IEnumerator RemoveDamagedCollider(Collider2D damagedCollider)
    {
        yield return new WaitForSeconds(0.5f);
        damagedColliders.Remove(damagedCollider);
    }
}
