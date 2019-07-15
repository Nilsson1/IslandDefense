using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public CharacterStats stats;
    public Camera cam;

    private List<GameObject> listOfChildren = new List<GameObject>();
    private float timeSinceAttack = 0.0f;
    UnitController player;
    private GameObject gameObjectSelected;
    private bool objectToMove = false;
    private Vector3 movePoint;
    private float stopDist;

    Material highlightMat;
    Material originalMat;

    public static EnemyController instance = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = UnitController.instance;
    }

    void Update()
    {


        //stats.AttackSpeed = 1.333f / stats.AttackCooldown;
        //UpdateAnimationSpeed();
    }

    void OnGUI()
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
        InteractWithGameObject.RegisterListener(OnInteracting);
        PlayerAttackEvent.RegisterListener(OnUnitAttacked);
        LeftMouseSelectEvent.RegisterListener(OnLeftMouseSelected);
        RightMouseSelectEvent.RegisterListener(OnRightClick);
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
    }

    private void OnInteracting(InteractWithGameObject interact)
    {
        foreach (Transform enemy in transform)
        {
            if (!listOfChildren.Contains(enemy.gameObject))
            {
                listOfChildren.Add(enemy.gameObject);
            }

            if (enemy.gameObject == interact.InteractingWithThisGameObject)
            {
                //TODO: Fix:This overwrites the stats, so if 2 players attack different objects the last attacked object takes damage from both.
                stats = enemy.GetComponent<CharacterStats>();

            }
        }
    }

    private void OnUnitAttacked(PlayerAttackEvent unitAttack)
    {
        if (stats != null)
        {
            DoDamage(stats.Damage);

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
                enemyAttack.UnitAttacked = player.gameObject;
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

            Destroy(gameObj);
            Debug.Log(gameObj.name + " died");
        }
    }
}
