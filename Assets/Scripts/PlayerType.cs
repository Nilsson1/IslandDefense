using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
     Murloc, Titan, Troll 
}

public enum ObjectType
{
    WALL, TOWER, FOODSOURCE, SHELTER, RESEARCH_CENTER, MERCHANT, ADVANCED_RESEARCH_CENTER, BASE_UNIT
}

public class ObjectMultiplier
{
    public float damageMultiplier;
    public float currentExpMultiplier;
    public float currentLevelMultiplier;
    public float maxHealthMultiplier;
    public float currentHealthMultiplier;
    public float attackCooldownMultiplier;
    public float armorMultiplier;
    public float attackSpeedMultiplier;
    public float moveSpeedMultiplier;
    public float expToNextLevelMultiplier;

    public ObjectMultiplier(ObjectType objectType)
    {
        switch (objectType)
        {
            case ObjectType.WALL:
                maxHealthMultiplier = 1.5f;
                armorMultiplier = 5f;
                break;

            case ObjectType.BASE_UNIT:
                damageMultiplier = 1f;
                currentExpMultiplier = 1f;
                currentLevelMultiplier = 1f;
                maxHealthMultiplier = 1f;
                currentHealthMultiplier = 1f;
                attackCooldownMultiplier = 1f;
                armorMultiplier = 1f;
                attackSpeedMultiplier = 1f;
                moveSpeedMultiplier = 1f;
                expToNextLevelMultiplier = 1f;
                break;

            default:
                Debug.Log("Default switch PlayerType");
                break;
        }
    }
}