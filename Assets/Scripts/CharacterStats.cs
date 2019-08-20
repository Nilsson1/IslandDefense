using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{

    private ObjectType objectType;

    private PlayerManager playerManager;
    private ObjectMultiplier objectMultiplier;

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
    public bool playManagerIsNull = false;

    private float[] statsArray = new float[10];


    public float[] CharcterStatsToArray()
    {
        statsArray[0] = damage;
        statsArray[1] = currentExp;
        statsArray[2] = currentLevel;
        statsArray[3] = maxHealth;
        statsArray[4] = currentHealth;
        statsArray[5] = attackCooldown;
        statsArray[6] = armor;
        statsArray[7] = attackSpeed;
        statsArray[8] = moveSpeed;
        statsArray[9] = expToNextLevel;

        return statsArray;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5.0f);
    }

    void Start()
    {
        playerManager = transform.parent.GetComponent<PlayerManager>();
        if (gameObject.GetComponent<WallController>() )
        {
            objectType = ObjectType.WALL;
        }

        if (gameObject.GetComponent<UnitController>())
        {
            objectType = ObjectType.BASE_UNIT;
        }

        objectMultiplier = new ObjectMultiplier(objectType);
    }

    void Update()
    {
        if (playerManager != null && !alreadyLoaded && playerManager.loadedStats)
        {
            damage = 20 * playerManager.damageMultiplier * objectMultiplier.damageMultiplier;
            currentExp = 0 * playerManager.currentExpMultiplier * objectMultiplier.currentExpMultiplier;
            currentLevel = 1 * playerManager.currentLevelMultiplier * objectMultiplier.currentLevelMultiplier;
            maxHealth = 100 * playerManager.maxHealthMultiplier * objectMultiplier.maxHealthMultiplier;
            currentHealth = 100 * playerManager.currentHealthMultiplier * objectMultiplier.currentHealthMultiplier;
            attackCooldown = 1.0f * playerManager.attackCooldownMultiplier * objectMultiplier.attackCooldownMultiplier;
            armor = 1 * playerManager.armorMultiplier * objectMultiplier.armorMultiplier;
            attackSpeed = (1.333f / 1.0f) * (playerManager.attackSpeedMultiplier) * objectMultiplier.attackSpeedMultiplier;
            moveSpeed = 1.0f * playerManager.moveSpeedMultiplier * objectMultiplier.moveSpeedMultiplier;
            expToNextLevel = 100 * playerManager.expToNextLevelMultiplier * objectMultiplier.expToNextLevelMultiplier;

            currentHealth = maxHealth;
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
