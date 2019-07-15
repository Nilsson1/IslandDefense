using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    PlayerManager playerManager;

    private float damage;
    private float currentExp;
    private float currentLevel;
    private float maxHealth;
    private float currentHealth;
    private float attackCooldown;
    private float armor;
    private float attackSpeed;
    private float moveSpeed;
    private float expToNextLevel;
    private bool alreadyLoaded = false;



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5.0f);
    }

    void Start()
    {

    }

    void Update()
    {
        playerManager = transform.parent.GetComponent<PlayerManager>();
        if (playerManager != null && !alreadyLoaded)
        {
            damage = 20 * playerManager.damageMultiplier;
            currentExp = 0 * playerManager.currentExpMultiplier;
            currentLevel = 1 * playerManager.currentLevelMultiplier;
            maxHealth = 100 * playerManager.maxHealthMultiplier;
            currentHealth = 100 * playerManager.currentHealthMultiplier;
            attackCooldown = 1.0f * playerManager.attackCooldownMultiplier;
            armor = 1 * playerManager.armorMultiplier;
            attackSpeed = (1.333f / 1.0f) * (playerManager.attackSpeedMultiplier);
            moveSpeed = 1.0f * playerManager.moveSpeedMultiplier;
            expToNextLevel = 100 * playerManager.expToNextLevelMultiplier;

            alreadyLoaded = true;
            
        }
    }

    public float Damage { get { return damage; } set { damage = value; } }
    public float CurrentExp { get { return currentExp; } set { currentExp = value; } }
    public float CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    public float AttackCooldown { get { return attackCooldown; } set { attackCooldown = value; } } 
    public float Armor { get { return armor; } set { armor = value; } } 
    public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } } 
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } } 
    public float ExpToNextLevel { get { return expToNextLevel; } set { expToNextLevel = value; } } 

}
