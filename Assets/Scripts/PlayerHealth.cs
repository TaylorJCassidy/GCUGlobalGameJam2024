using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 3f; // 3 lives
    private float health;

    public float GetHealth() {  return health; }
    private void Start()
    {
        health = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        if (GameManager.instance.gameState == GameManager.GameState.Dodge) {
            damage = Mathf.Abs(damage); // prevents negative damage
            health -= damage;
            Debug.Log("Player took " + damage + " damage. Health is now " + health);
            if (health <= 0)
            {
                GameManager.instance.EndGame();
            }
        }
    }

}