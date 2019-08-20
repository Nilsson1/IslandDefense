using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIHandler : MonoBehaviour
{
    UnitController player;
    CharacterStats stats;

    private Text levelUI;
    private Text damageUI;
    private Text healthUI;
    private Text armorUI;
    private Text enemyInfoUI;

    // Start is called before the first frame update
    void Start()
    {
        player = UnitController.instance;

        levelUI = GameObject.Find("Level").GetComponent<Text>();
        damageUI = GameObject.Find("Damage").GetComponent<Text>();
        healthUI = GameObject.Find("Health").GetComponent<Text>();
        armorUI = GameObject.Find("Armor").GetComponent<Text>();
        enemyInfoUI = GameObject.Find("EnemyInfo").GetComponent<Text>();

        levelUI.text = "Level " + player.stats.CurrentLevel.ToString() + " (" + player.stats.CurrentExp.ToString() + "/" + player.stats.ExpToNextLevel.ToString() + ")";
        healthUI.text = "Health " + player.stats.CurrentHealth.ToString() + "/" + player.stats.MaxHealth.ToString();
        damageUI.text = "Damage " + player.stats.Damage.ToString();
        armorUI.text = "Armor " + player.stats.Armor.ToString();
        enemyInfoUI.text = "";

    }

    void OnEnable()
    {
        UnitDeathEvent.RegisterListener(OnUnitDeath);
        LeftMouseSelectEvent.RegisterListener(OnLeftMouseSelect);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Instead of doing this each frame (Update), listen to an UpdateUI event that fires when something changes.
        /*levelUI.text = "Level " + player.stats.CurrentLevel.ToString() + " (" + player.stats.CurrentExp.ToString() + "/" + player.stats.ExpToNextLevel.ToString() + ")";
        healthUI.text = "Health " + player.stats.CurrentHealth.ToString() + "/" + player.stats.MaxHealth.ToString();
        damageUI.text = "Damage " + player.stats.Damage.ToString();
        armorUI.text = "Armor " + player.stats.Armor.ToString();*/

        /*levelUI.text = "Level " + stats.CurrentLevel.ToString() + " (" + stats.CurrentExp.ToString() + "/" + stats.ExpToNextLevel.ToString() + ")";
        healthUI.text = "Health " + stats.CurrentHealth.ToString() + "/" + stats.MaxHealth.ToString();
        damageUI.text = "Damage " + stats.Damage.ToString();
        armorUI.text = "Armor " + stats.Armor.ToString();*/
        //enemyInfoUI.text = "PlayerType " + leftMouseSelectEvent.selectedGameObject.GetComponentInParent<PlayerManager>().playerType.ToString();
    }

    void OnLeftMouseSelect(LeftMouseSelectEvent leftMouseSelectEvent)
    {
        if (leftMouseSelectEvent.selectedGameObject.GetComponent<CharacterStats>())
        {
            Debug.Log("Updating UI stats!");
            stats = leftMouseSelectEvent.selectedGameObject.GetComponent<CharacterStats>();

            levelUI.text = "Level " + stats.CurrentLevel.ToString() + " (" + stats.CurrentExp.ToString() + "/" + stats.ExpToNextLevel.ToString() + ")";
            healthUI.text = "Health " + stats.CurrentHealth.ToString() + "/" + stats.MaxHealth.ToString();
            damageUI.text = "Damage " + stats.Damage.ToString();
            armorUI.text = "Armor " + stats.Armor.ToString();
            enemyInfoUI.text = "PlayerType " + leftMouseSelectEvent.selectedGameObject.GetComponentInParent<PlayerManager>().playerType.ToString();
        }
    }

    void OnUnitDeath(UnitDeathEvent unitDeathEvent)
    {
        enemyInfoUI.text = "Enemy Health: 0";
        StartCoroutine("WaitAndUpdate");
    }

    IEnumerator WaitAndUpdate()
    {
        yield return new WaitForSeconds(2);
        enemyInfoUI.text = "";
    }

    //Inventory buttons

    public void UIBuildOnClick()
    {
        BuilderEvent build = new BuilderEvent();
        build.buildMode = true;
        build.FireEvent();
    }
}
