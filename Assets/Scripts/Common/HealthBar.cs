using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image           m_healthBarFill;
    [SerializeField] TextMeshProUGUI m_healthBarText;
    [SerializeField] Transform       m_owner;
    [SerializeField] float           m_Offset;
    // Start is called before the first frame update
    void Start()
    {
        if(m_healthBarFill == null)
        {
            Debug.LogError("Health bar fill is null!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_owner != null)
        {
            FollowCharacter(m_owner.position);
        }
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float ratio = (float)currentHealth / maxHealth;
        // Because this script is used by player and enemy.
        // But enemy doesn't need to show the value of their health.
        if (m_healthBarText != null)
        {
            m_healthBarText.text = currentHealth.ToString() + "/" + maxHealth.ToString() +"("+ (ratio*100).ToString("0.00") + "%)";
        }
        if (ratio >= 0 && ratio <= 1)
            m_healthBarFill.fillAmount = ratio;
    }

    void FollowCharacter(Vector3 targetPosition)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(targetPosition + new Vector3(0, m_Offset, 0));
        transform.position = screenPoint;
    }
}
