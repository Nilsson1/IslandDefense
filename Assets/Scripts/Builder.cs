using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Builder : MonoBehaviourPunCallbacks
{
    public float gridSize;

    private Vector3 truePos;
    private Vector3 mouseStartPos;
    private bool buildMode = false;

    private PhotonView PV;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void OnEnable()
    {
        BuilderEvent.RegisterListener(OnBuilderEvent);
    }

    void LateUpdate()
    {
        if(buildMode && Input.GetKeyDown(KeyCode.V))
        {
            buildMode = false;
        }

        if (Input.GetMouseButtonDown(0) && buildMode)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                mouseStartPos.x = hit.point.x;
                mouseStartPos.y = hit.collider.transform.position.y + 1;
                mouseStartPos.z = hit.point.z;
                Debug.Log("Hit: " + hit.collider + "x, y, z: " + mouseStartPos.x + " " + mouseStartPos.y + " " + mouseStartPos.z);
            }

            truePos.x = Mathf.Floor(mouseStartPos.x / gridSize) * gridSize;
            truePos.y = Mathf.Floor(mouseStartPos.y / gridSize) * gridSize;
            truePos.z = Mathf.Floor(mouseStartPos.z / gridSize) * gridSize;

            SpawnUnitEvent spawnUnit = new SpawnUnitEvent();
            Transform parent = gameObject.transform.parent;
            spawnUnit.viewID = parent.GetComponent<PhotonView>().ViewID;
            spawnUnit.uNIT_TYPE = SpawnUnitEvent.UNIT_TYPE.WALL;
            spawnUnit.position = truePos;
            spawnUnit.spawner = parent;
            spawnUnit.objectType = ObjectType.WALL;
            Debug.Log(spawnUnit.uNIT_TYPE);
            spawnUnit.FireEvent();

            truePos = Vector3.zero;
        }
    }

    private void OnBuilderEvent(BuilderEvent builderEvent)
    {
        if(PV.IsMine)
        {
            buildMode = builderEvent.buildMode;
        }
    }
}
