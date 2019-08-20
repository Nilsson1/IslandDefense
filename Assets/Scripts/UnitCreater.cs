using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class UnitCreater : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    private RPC rpc;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rpc = RPC.instance;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        SpawnUnitEvent.RegisterListener(OnSpawnUnit);
    }

    private void OnSpawnUnit(SpawnUnitEvent spawnUnit)
    {
        //string mat = spawnUnit.spawner.gameObject.GetComponent<PlayerManager>().playerType.ToString() + spawnUnit.objectType.ToString();
        //Debug.Log(mat);
        rpc.RPCSpawnUnit(spawnUnit.uNIT_TYPE, spawnUnit.position, spawnUnit.viewID, spawnUnit.spawner.gameObject.GetComponent<PlayerManager>().playerType.ToString());
        //rpc.SetParent(spawnUnit.viewID, spawnedUnitViewID);

        //GameObject gameObj = (GameObject)PhotonNetwork.Instantiate(Path.Combine("Prefabs", spawnUnit.uNIT_TYPE.ToString()), spawnUnit.position, Quaternion.identity);
        //int spawnedUnitViewID = gameObj.GetComponent<PhotonView>().ViewID;
        //PV.RPC("RPCSetParent", RpcTarget.All, spawnUnit.viewID, spawnedUnitViewID);

        /*gameObj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerMainUnit"), new Vector3(-2, 1, 2), Quaternion.identity);
        spawnedUnitViewID = gameObj.GetComponent<PhotonView>().ViewID;
        PV.RPC("RPCSetParent", RpcTarget.All, spawnUnit.viewID, spawnedUnitViewID);*/
    }

    /*[PunRPC]
    private void RPCSetParent(int parentViewID, int spawnedUnitViewID, PhotonMessageInfo info)
    {
        Debug.Log("RPCSetParent called!" + info.photonView);
        Debug.Log("RPC ID: " + parentViewID);

        GameObject parent = PhotonView.Find(parentViewID).gameObject;
        GameObject spawnedObject = PhotonView.Find(spawnedUnitViewID).gameObject;
        spawnedObject.transform.SetParent(parent.transform);

    }*/
}
