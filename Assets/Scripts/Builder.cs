using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Builder : MonoBehaviourPunCallbacks
{
    public float gridSize;

    public Canvas canvas;
    private Vector3 truePos;
    private Vector3 mouseStartPos;
    private bool buildMode = false;
    private bool buildObject = false;
    private bool buildMultipleObjects = false;

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

        if (Input.GetMouseButtonDown(0))
        {
            buildObject = true;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButtonDown(0))
            {
                buildMultipleObjects = true;
                Debug.Log("Multiple presssed!");
            }
        }
    }

    private IEnumerator BuildModeActivate()
    {
        buildMultipleObjects = false;
        buildObject = false;
        buildMode = true;
        bool runLoop = true;
        Cursor.visible = false;
        while (runLoop)
        {
            Debug.Log("Buildmode: " + buildMode);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 newPoint = hit.point;
                newPoint.x -= 0.005f;
                newPoint.z -= 0.005f;
                newPoint.y = hit.collider.transform.position.y + 0.51f;
                truePos = Vector3.zero;

                truePos.x = Mathf.Floor(newPoint.x / gridSize) * gridSize;
                truePos.y = newPoint.y;
                truePos.z = Mathf.Floor(newPoint.z / gridSize) * gridSize;

                canvas.transform.position = truePos;


                if (buildObject || buildMultipleObjects)
                {
                    Debug.Log("LeftMouse pressed!");
                    SpawnUnitEvent spawnUnit = new SpawnUnitEvent();
                    Transform parent = gameObject.transform.parent;
                    spawnUnit.viewID = parent.GetComponent<PhotonView>().ViewID;
                    spawnUnit.uNIT_TYPE = SpawnUnitEvent.UNIT_TYPE.WALL;
                    spawnUnit.position = new Vector3(truePos.x, hit.collider.transform.position.y +1, truePos.z);
                    spawnUnit.spawner = parent;
                    spawnUnit.objectType = ObjectType.WALL;
                    Debug.Log(spawnUnit.uNIT_TYPE);
                    spawnUnit.FireEvent();

                    truePos = Vector3.zero;
                    buildMode = false;
                    Cursor.visible = true;
                    runLoop = false;
                }
                if (buildMultipleObjects)
                {
                    Debug.Log("Start again!");
                    buildMode = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Stopping Builder");
        if(buildMode)
            StartCoroutine("BuildModeActivate");
    }

    private void OnBuilderEvent(BuilderEvent builderEvent)
    {
        if(PV.IsMine)
        {
            buildMode = builderEvent.buildMode;
            canvas.enabled = true;
            if (buildMode)
                StartCoroutine("BuildModeActivate");
        }
    }
}
