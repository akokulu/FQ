using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
    public GameObject healthBar;

    private float healthBarWidth;

    private GameObject mob;
    private Quaternion rot;

    private EnemyProps enemy;

    private float curHP;
    private float maxHP;

    void Awake()
    {
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;

        healthBarWidth = healthBar.transform.localScale.x;  // Set initial width of health bar

        mob = transform.parent.gameObject;
        transform.parent = null;    // IMPORTANT. Detatch healthbar from player to prevent rotation
        
        // Determine if bar is attached to enemy or player and use appropriate health stats
        if (mob.tag == "Player")
        {
            curHP = PlayerProps.currentHealth;
            maxHP = PlayerProps.maxHealth;
        }
        else
        {
            enemy = mob.GetComponent<EnemyProps>();
            curHP = enemy.currentHealth;
            maxHP = EnemyProps.maxHealth;
        }

        transform.localScale = new Vector3(0.1f, 0.1f, 0.005f);    // Set size of health bar
    }

    void Update()
    {
        if (mob.tag == "Player")
            curHP = PlayerProps.currentHealth;
        else
            curHP = enemy.currentHealth;

        // Update the size of the health bar
        healthBar.transform.localScale = new Vector3(healthBarWidth * (curHP / maxHP), 
                                                     healthBar.transform.localScale.y, 
                                                     healthBar.transform.localScale.z);

        float newXPos = transform.position.x - (healthBarWidth - healthBarWidth * (curHP / maxHP)) / 2;
        healthBar.transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);


        // Position health bar over head
        transform.position = mob.transform.position + new Vector3(0, 2, 0);
        
        // Rotate health bar to face main camera
        transform.rotation = Quaternion.identity;
        transform.Rotate(315, 0, 0);

        if (curHP <= 0)
            Destroy(gameObject);
    }
}
