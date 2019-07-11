using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private GameObject gameObjectSelected;
    private bool objectToMove = false;
    private Vector3 movePoint;
    private float stopDist;

    Material highlightMat;
    Material originalMat;

    void Awake()
    {
        instance = this;
    }

   /* void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("D")) && gameObjectSelected)
        {
            print("D pressed!");
            SpawnUnitEvent spawnUnit = new SpawnUnitEvent();
            spawnUnit.parent = transform;
            spawnUnit.tag = "AttackAble";
            spawnUnit.position = new Vector3(6f, 1f, 6f);
            spawnUnit.uNIT_TYPE = SpawnUnitEvent.UNIT_TYPE.YELLOW_UNIT;
            Debug.Log(spawnUnit.uNIT_TYPE);
            spawnUnit.FireEvent();
        }
    }

    void OnEnable()
    {
        LeftMouseSelectEvent.RegisterListener(OnLeftMouseSelected);
        RightMouseSelectEvent.RegisterListener(OnRightClick);
    }

    private void OnRightClick(RightMouseSelectEvent rightClick)
    {
        if()

        if (!rightClick.rightClickGameObject.transform.IsChildOf(transform.parent) && rightClick.rightClickGameObject.tag == "AttackAble")
        {
            attackedObject = rightClick.rightClickGameObject;

            attack = true;
            movePoint = rightClick.rightClickGameObject.transform.position;
            stopDist = 2.5f;

            InteractWithGameObject interact = new InteractWithGameObject();
            interact.InteractingWithThisGameObject = rightClick.rightClickGameObject;
            interact.FireEvent();
        }
        else
        {
            movePoint = rightClick.mousePosition;
            stopDist = 1.1f;
        }
        agent.ResetPath();
        agent.SetDestination(movePoint);
    }

    private void OnLeftMouseSelected(LeftMouseSelectEvent objectSelected)
    {
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
                Debug.Log("new Mat: " + originalMat.name);
                MeshRenderer meshRenderer = enemy.GetComponent<MeshRenderer>();
                // Get the current material applied on the GameObject
                Material oldMaterial = meshRenderer.material;
                Debug.Log("Applied Material: " + oldMaterial.name);
                // Set the new material on the GameObject
                meshRenderer.material = originalMat;
            }
        }
        if (gameObjectSelected != null)
        {

            highlightMat = Resources.Load("Mat/HighlightMat", typeof(Material)) as Material;
            Debug.Log("new Mat: " + highlightMat.name);
            MeshRenderer meshRenderer = gameObjectSelected.GetComponent<MeshRenderer>();
            // Get the current material applied on the GameObject
            Material oldMaterial = meshRenderer.material;
            Debug.Log("Applied Material: " + oldMaterial.name);
            // Set the new material on the GameObject
            meshRenderer.material = highlightMat;

            objectToMove = true;
        }
        else
        {
            objectToMove = false;
        }
    }*/
}
