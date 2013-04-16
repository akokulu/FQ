using UnityEngine;
using System.Collections;

public class EnemyProps : MonoBehaviour
{
    public bool hasItem;
    public float currentHealth = 100.0f;
    public static float maxHealth = 100.0f;

    public static float weaponDamage = 10.0f;
    public static float shieldDamage = 1.0f;

    public static float fieldOfViewRange = 230f;    // Enemies field of view in degrees
    public static float minDetectDistance = 3f;
    public static float viewDistance = 20f;  // Distance the enemy can see in front of him

    public static float fieldOfViewAttack = 50f;    // Enemies field of view for attacking in degrees
    public static float attackDistance = 2.2f;

    public static float searchTime = 20f; // Amount of search time after enemy loses sight of player

    public GameObject itemPrefab;   // Prefab of item to be dropped

    public void OnEnemyDeath ()
    {
        PlayerStats.increaseKillCount();
        if (hasItem)
        {
            // Spawn item on death if enemy has item
            Instantiate(itemPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(270, 0, 0));
        }

        // Regen health on kill
        if (PlayerProps.currentHealth + PlayerProps.currentHealth * PlayerProps.healthRegen >= PlayerProps.maxHealth)  
            PlayerProps.currentHealth = PlayerProps.maxHealth;   
        else
            PlayerProps.currentHealth += PlayerProps.maxHealth * PlayerProps.healthRegen;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Weapon")
        {
            currentHealth -= PlayerProps.weaponDamage;
            c.enabled = false;
        }
    }
}