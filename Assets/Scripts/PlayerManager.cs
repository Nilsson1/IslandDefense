using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public PlayerType playerType;
    private TypeStats stats;
    public Player player;

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

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        playerType = PlayerType.Titan;
        stats = new TypeStats(playerType);
        StartCoroutine("ILoadMultipliers");
    }

    private IEnumerator ILoadMultipliers()
    {
        while (true)
        {
            if (TypeStats.rdyToLoad)
            {
                damageMultiplier = stats.damageMultiplier;
                currentExpMultiplier = stats.currentExpMultiplier;
                currentLevelMultiplier = stats.currentLevelMultiplier;
                maxHealthMultiplier = stats.maxHealthMultiplier;
                currentHealthMultiplier = stats.currentHealthMultiplier;
                attackCooldownMultiplier = stats.attackCooldownMultiplier;
                armorMultiplier = stats.armorMultiplier;
                attackSpeedMultiplier = stats.attackSpeedMultiplier;
                moveSpeedMultiplier = stats.moveSpeedMultiplier;
                expToNextLevelMultiplier = stats.expToNextLevelMultiplier;
                

                yield return null;
            }
        }
    }
}
