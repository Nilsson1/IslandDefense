using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public CharacterStats stats;
    private PhotonView PV;

    private float timeSinceAttack = 0.0f;
    private GameObject gameObjectSelected;

    private bool objectToMove = false;
    private Vector3 movePoint;
    private float stopDist;

    Material highlightMat;
    Material originalMat;

    public static WallController instance = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {

    }

    void OnGUI()
    {

    }

    void OnEnable()
    {
        InteractWithGameObject.RegisterListener(OnInteracting);
        PlayerAttackEvent.RegisterListener(OnUnitAttacked);
        //LeftMouseSelectEvent.RegisterListener(OnLeftMouseSelected);
        RightMouseSelectEvent.RegisterListener(OnRightClick);
    }

    void OnDisable()
    {
        InteractWithGameObject.UnregisterListener(OnInteracting);
        PlayerAttackEvent.UnregisterListener(OnUnitAttacked);
        //LeftMouseSelectEvent.UnregisterListener(OnLeftMouseSelected);
        RightMouseSelectEvent.UnregisterListener(OnRightClick);
    }

    private void OnRightClick(RightMouseSelectEvent rightClick)
    {
        movePoint = Vector3.zero;
        if (objectToMove && gameObjectSelected != null)
        {
            //attack = false;
            //rdyToAttack = false;
            movePoint = Vector3.zero;
            //rayMove = true;

            // attack = true;
            movePoint = rightClick.rightClickGameObject.transform.position;
            stopDist = 2.5f;

            Debug.Log(gameObjectSelected.name + " hit: " + rightClick.rightClickGameObject.name);

            //agent.ResetPath();
            //agent.SetDestination(movePoint);
        }
    }
    /* OnLeftMouseSelected
    private void OnLeftMouseSelected(LeftMouseSelectEvent objectSelected)
    {
        if (transform.childCount == 0) { return; }
        gameObjectSelected = null;
        foreach (Transform enemy in transform)
        {
            if (objectSelected.selectedGameObject == enemy.gameObject)
            {
                gameObjectSelected = enemy.gameObject;
            }
            else if (enemy.gameObject.GetComponent<MeshFilter>() != null)
            {
                originalMat = Resources.Load("Mat/AttackAble", typeof(Material)) as Material;
                MeshRenderer meshRenderer = enemy.GetComponent<MeshRenderer>();
                // Get the current material applied on the GameObject
                Material oldMaterial = meshRenderer.material;
                // Set the new material on the GameObject
                meshRenderer.material = originalMat;
            }
        }
        if (gameObjectSelected != null)
        {

            highlightMat = Resources.Load("Mat/HighlightMat", typeof(Material)) as Material;
            MeshRenderer meshRenderer = gameObjectSelected.GetComponent<MeshRenderer>();
            // Get the current material applied on the GameObject
            Material oldMaterial = meshRenderer.material;
            // Set the new material on the GameObject
            meshRenderer.material = highlightMat;

            objectToMove = true;
        }
        else
        {
            objectToMove = false;
        }
    }*/

    private void OnInteracting(InteractWithGameObject interact)
    {
        if (gameObject == interact.InteractingWithThisGameObject)
        {
            //TODO: Fix:This overwrites the stats, so if 2 players attack different objects the last attacked object takes damage from both.
            stats = GetComponent<CharacterStats>();

        }
    }
    
    private void OnUnitAttacked(PlayerAttackEvent unitAttack)
    {
        if (stats != null)
        {
            //DoDamage(stats.Damage);

            float damage = unitAttack.UnitStats.Damage;
            damage -= stats.Armor;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
            stats.CurrentHealth -= damage;
            Debug.Log(unitAttack.UnitAttacked.name + " took " + damage + " damage from " + unitAttack.UnitAttacker.name);

            if (stats.CurrentHealth <= 0)
            {
                stats.CurrentHealth = 0;
                Die(unitAttack.UnitAttacked);
            }
        }
    }

    public void DoDamage(float damage)
    {
        // TODO: Make this independent on Player attacking the object first.
        if (stats != null)
        {
            if (Time.time - timeSinceAttack > 1.5)
            {
                EnemyAttackEvent enemyAttack = new EnemyAttackEvent();
                enemyAttack.UnitAttacker = stats.gameObject;
                //enemyAttack.UnitAttacked = player.gameObject;
                enemyAttack.UnitStats = stats;
                enemyAttack.FireEvent();

                timeSinceAttack = Time.time;
            }
        }
    }

    public void Die(GameObject gameObj)
    {
        //Die
        if (gameObject != null)
        {
            UnitDeathEvent unitDeathEvent = new UnitDeathEvent();
            unitDeathEvent.UnitDied = gameObj;
            unitDeathEvent.expDropped = 50;
            unitDeathEvent.FireEvent();


            PV.RPC("RPCDestroyGO", RpcTarget.All);
            Debug.Log(gameObj.name + " died");
        }
    }

    [PunRPC]
    private void RPCDestroyGO(PhotonMessageInfo info)
    {
        if (info.photonView.IsMine)
        {
            PhotonNetwork.Destroy(info.photonView);
        }
    }
}