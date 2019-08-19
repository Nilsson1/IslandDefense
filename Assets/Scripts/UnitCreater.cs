using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class UnitCreater : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
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
        
        GameObject gameObj = (GameObject)PhotonNetwork.Instantiate(Path.Combine("Prefabs", spawnUnit.uNIT_TYPE.ToString()), new Vector3((float)-2.57, (float)1.0, (float)5.13), Quaternion.identity);
        int spawnedUnitViewID = gameObj.GetComponent<PhotonView>().ViewID;
        PV.RPC("RPCSetParent", RpcTarget.All, spawnUnit.viewID, spawnedUnitViewID);

        gameObj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerMainUnit"), new Vector3(-2, 1, 2), Quaternion.identity);
        spawnedUnitViewID = gameObj.GetComponent<PhotonView>().ViewID;
        PV.RPC("RPCSetParent", RpcTarget.All, spawnUnit.viewID, spawnedUnitViewID);
    }

    [PunRPC]
    private void RPCSetParent(int parentViewID, int spawnedUnitViewID, PhotonMessageInfo info)
    {
        Debug.Log("RPCSetParent called!" + info.photonView);
        Debug.Log("RPC ID: " + parentViewID);

        GameObject parent = PhotonView.Find(parentViewID).gameObject;
        GameObject spawnedObject = PhotonView.Find(spawnedUnitViewID).gameObject;
        spawnedObject.transform.SetParent(parent.transform);

    }
}
