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
    private bool buildMode = false;
    Player[] playerList;
    PhotonView PV;

    public bool coroutineStarted = false;

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

    public bool rdyToLoad;
    public bool loadedStats = false;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        playerList = PhotonNetwork.PlayerList;
        PV = GetComponent<PhotonView>();

        if (PV.Owner.Equals(playerList[0]))
        {
            playerType = PlayerType.Titan;
        }
        else
        {
            playerType = PlayerType.Murloc;
        }

        stats = new TypeStats(playerType);
        StartCoroutine("ILoadMultipliers");
    }

    private IEnumerator ILoadMultipliers()
    {
        coroutineStarted = true;
        while (true)
        {
            rdyToLoad = TypeStats.rdyToLoad;
            if (rdyToLoad)
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

                loadedStats = true;
                yield return null;
            }
        }
    }

    public bool BuildMode { get { return buildMode; } set { buildMode = value; } }
}
