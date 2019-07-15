using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeStats
{
    PlayerType playerType;

    public float damageMultiplier = 1.0f;
    public float currentExpMultiplier = 1.0f;
    public float currentLevelMultiplier = 1.0f;
    public float maxHealthMultiplier = 1.0f;
    public float currentHealthMultiplier = 1.0f;
    public float attackCooldownMultiplier = 1.0f;
    public float armorMultiplier = 1.0f;
    public float attackSpeedMultiplier = 1.0f;
    public float moveSpeedMultiplier = 1.0f;
    public float expToNextLevelMultiplier = 1.0f;

    public static bool rdyToLoad = false;

    public TypeStats(PlayerType type)
    {
        playerType = type;
        GetPlayerType();
        rdyToLoad = true;
    }
    
    private void GetPlayerType()
    {
        switch (playerType)
        {
            case PlayerType.Titan:
                Debug.Log("Entered Titan case");
                LoadTitanMultipliers();
                break;

            case PlayerType.Murloc:
                LoadMurlocMultipliers();
                break;

            case PlayerType.Troll:
                LoadTrollMultipliers();
                break;

            default:
                LoadDefaultMultipliers();
                break;
        }
    }

    private void LoadDefaultMultipliers()
    {
        damageMultiplier = 0.5f;
        currentExpMultiplier = 1.0f;
        currentLevelMultiplier = 1.0f;
        maxHealthMultiplier = 0.5f;
        currentHealthMultiplier = 0.5f;
        attackCooldownMultiplier = 1.0f;
        armorMultiplier = 0.5f;
        attackSpeedMultiplier = 1.0f;
        moveSpeedMultiplier = 1.0f;
        expToNextLevelMultiplier = 1.0f;
    }

    private void LoadTitanMultipliers()
    {
        damageMultiplier = 1.0f;
        currentExpMultiplier = 1.0f;
        currentLevelMultiplier = 1.0f;
        maxHealthMultiplier = 1.0f;
        currentHealthMultiplier = 1.0f;
        attackCooldownMultiplier = 1.0f;
        armorMultiplier = 1.0f;
        attackSpeedMultiplier = 1.0f;
        moveSpeedMultiplier = 1.0f;
        expToNextLevelMultiplier = 1.0f;
    }

    private void LoadMurlocMultipliers()
    {
        damageMultiplier = 1.0f;
        currentExpMultiplier = 1.0f;
        currentLevelMultiplier = 1.0f;
        maxHealthMultiplier = 1.0f;
        currentHealthMultiplier = 1.0f;
        attackCooldownMultiplier = 1.0f;
        armorMultiplier = 1.0f;
        attackSpeedMultiplier = 1.0f;
        moveSpeedMultiplier = 1.0f;
        expToNextLevelMultiplier = 1.0f;
    }

    private void LoadTrollMultipliers()
    {
        damageMultiplier = 1.0f;
        currentExpMultiplier = 1.0f;
        currentLevelMultiplier = 1.0f;
        maxHealthMultiplier = 1.0f;
        currentHealthMultiplier = 1.0f;
        attackCooldownMultiplier = 1.0f;
        armorMultiplier = 1.0f;
        attackSpeedMultiplier = 1.0f;
        moveSpeedMultiplier = 1.0f;
        expToNextLevelMultiplier = 1.0f;
    }

}
