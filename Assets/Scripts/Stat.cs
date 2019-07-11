using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int healthValue = 100;
    [SerializeField]
    private int armorValue;
    [SerializeField]
    private int damageValue;

    public int CurrentExp { get; set; } = 0;

    private int level = 1;
    private int maxHealth = 100;
    private int currentHealth;
    private int armor = 4;
    private float attackCooldown = 1.0f;
    private float attackSpeed;
    private float moveSpeed = 1.0f;

    public int ExpToNextLevel { get; set; } = 100;
    public int CurrentLevel { get; set; }

    public int GetHealth()
    {
        return healthValue;
    }

    public int GetArmor()
    {
        return armorValue;
    }

    public int GetDamage()
    {
        return damageValue;
    }
}
