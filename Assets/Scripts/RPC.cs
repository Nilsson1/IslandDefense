using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using ExitGames.Client.Photon;

public class RPC : MonoBehaviourPunCallbacks
{
    public static RPC instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;
    }

    public void Attack(int attackerViewID, int attackedViewID, float[] stats)
    {
        this.photonView.RPC("RPCAttack", RpcTarget.All, attackerViewID, attackedViewID, stats);
    }

    public void RPCSpawnUnit(SpawnUnitEvent.UNIT_TYPE type, Vector3 position, int parentViewID, string material)
    {
        GameObject gameObj = (GameObject)PhotonNetwork.Instantiate(Path.Combine("Prefabs", type.ToString()), position, Quaternion.identity);
        int spawnedUnitViewID = gameObj.GetComponent<PhotonView>().ViewID;
        RPCSetParent(parentViewID, spawnedUnitViewID);
        UpdateMaterial(spawnedUnitViewID, material);
    }

    public void RPCSetParent(int parentViewID, int childViewID)
    {
        this.photonView.RPC("RPCSetParent", RpcTarget.All, parentViewID, childViewID);
    }

    public void RPCDestroyGO(int viewIDToDestroy, int viewIDKiller)
    {
        this.photonView.RPC("RPCDestroyGO", RpcTarget.All, viewIDToDestroy, viewIDKiller);
    }

    public void UpdateMaterial(int viewID, string material)
    {
        this.photonView.RPC("RPCUpdateMaterial", RpcTarget.All, viewID, material);
    }

    //PunRPCs start here

    [PunRPC]
    private void RPCUpdateMaterial(int viewID, string material)
    {
        MeshRenderer renderer = PhotonView.Find(viewID).GetComponent<MeshRenderer>();
        Material newMat = Resources.Load("Mat/" + material, typeof(Material)) as Material;
        renderer.material = newMat;
    }

    [PunRPC]
    private void RPCAttack(int attackerViewID, int attackedViewID, float[] stats)
    {
        PlayerAttackEvent attackEvent = new PlayerAttackEvent()
        {
            attackedViewID = attackedViewID,
            attackerViewID = attackerViewID,
            UnitStatsArray = stats
        };
        attackEvent.FireEvent();
    }

    [PunRPC]
    private void RPCSetParent(int parentViewID, int spawnedUnitViewID, PhotonMessageInfo info)
    {
        GameObject parent = PhotonView.Find(parentViewID).gameObject;
        GameObject spawnedObject = PhotonView.Find(spawnedUnitViewID).gameObject;
        spawnedObject.transform.SetParent(parent.transform);
    }

    [PunRPC]
    private void RPCDestroyGO(int viewIDToDestroy, int viewIDKiller, PhotonMessageInfo info)
    {
        PhotonView pv = PhotonView.Find(viewIDToDestroy);
        if (pv == null)
            return; 

        if (pv.IsMine)
        {
            UnitDeathEvent deathEvent = new UnitDeathEvent()
            {
                expDropped = 50,
                diedPhotonViewID = pv.ViewID,
                diedPosition = pv.transform.position,
                killerPhotonViewID = viewIDKiller
            };
            deathEvent.FireEvent();

            PhotonNetwork.Destroy(PhotonView.Find(viewIDToDestroy));
        }
    }
}
