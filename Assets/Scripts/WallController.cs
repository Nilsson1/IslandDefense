using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    RPC rpc;

    public CharacterStats stats;
    public float health;
    private PhotonView PV;

    private float timeSinceAttack = 0.0f;
    private GameObject gameObjectSelected;

    private bool objectToMove = false;
    private Vector3 movePoint;
    private float stopDist;
    private bool loadedMat = false;

    Material highlightMat;
    Material originalMat;


    void Awake()
    {
        rpc = RPC.instance;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (gameObject.GetComponentInParent<PlayerManager>().loadedStats && !loadedMat)
        {
            UpdateMaterial();
            loadedMat = true;
        }
    }

    void OnEnable()
    {
        InteractWithGameObject.RegisterListener(OnInteracting);
        PlayerAttackEvent.RegisterListener(OnUnitAttacked);
        //LeftMouseSelectEvent.RegisterListener(OnLeftMouseSelected);
    }

    void OnDisable()
    {
        InteractWithGameObject.UnregisterListener(OnInteracting);
        PlayerAttackEvent.UnregisterListener(OnUnitAttacked);
        //LeftMouseSelectEvent.UnregisterListener(OnLeftMouseSelected);
    }

    private void UpdateMaterial()
    {
        //rpc.UpdateMaterial(PV.ViewID, gameObject.GetComponentInParent<PlayerManager>().playerType.ToString());
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
            //stats = GetComponent<CharacterStats>();
        }
    }
    
    private void OnUnitAttacked(PlayerAttackEvent attackedEvent)
    {

        if (stats != null && attackedEvent.attackedViewID == gameObject.GetPhotonView().ViewID)
        {
            if (gameObject.GetPhotonView().ViewID == attackedEvent.attackedViewID)
            {
                float actualDamageTaken = attackedEvent.UnitStatsArray[0] - stats.Armor;
                stats.CurrentHealth -= Mathf.Clamp(actualDamageTaken, 0, float.MaxValue);
                if (stats.CurrentHealth <= 0)
                {
                    Die(gameObject, attackedEvent.attackerViewID);
                }
            }
        }
    }

    public void Die(GameObject gameObj, int viewIDKiller)
    {
        //Die
        if (gameObject != null)
        {
            rpc.RPCDestroyGO(gameObj.GetPhotonView().ViewID, viewIDKiller);
        }
    }
}