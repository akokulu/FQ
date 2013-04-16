using UnityEngine;
using System.Collections;

public class PlayerProps : MonoBehaviour
{
    public static float maxHealth = 100.0f;
    public static float currentHealth = maxHealth;
    public static float weaponDamage = 20.0f;
    public static bool isBlocking = false;
    public static bool hasItem = false;

    public static float healthRegen = .2f;

    private bool sessionSaved = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Save current score when player dies
        if (PlayerProps.currentHealth <= 0 && !sessionSaved)
        {
            HighScoreManager._instance.SaveHighScore(PlayerStats.playerName, PlayerStats.enemiesKilled, PlayerStats.lampsFetched);
            sessionSaved = true;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "EnemyWeapon" && !isBlocking)
        {
            // Check which weapon hit and deduct appropriate damage
            if (c.gameObject.name == "woodenshield")
                currentHealth -= EnemyProps.shieldDamage;
            else
                currentHealth -= EnemyProps.weaponDamage;

            c.enabled = false;  // Disable collider after hit
        }
        else if (c.gameObject.tag == "Item")
        {
            Destroy(c.gameObject);
            hasItem = true;
            PlayerStats.increaseLamps();
        }
    }
}