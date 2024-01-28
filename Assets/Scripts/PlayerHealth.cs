using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 3f; // 3 lives
    private float health;

    private void Start()
    {
        health = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        damage = Mathf.Abs(damage); // prevents negative damage
        health -= damage;

        if (health <= 0)
        {
            GameManager.instance.EndGame();
        }
    }

}
