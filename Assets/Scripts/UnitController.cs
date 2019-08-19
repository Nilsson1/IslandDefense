using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;


public class UnitController : MonoBehaviourPunCallbacks
{
    public NavMeshAgent agent;
    public Animator anim;
    public Rigidbody rb;
    public CharacterStats stats;

    AnimatorController animatorController;

    private GameObject attackedObject;

    private Animation animation;

    private Vector3 movePoint;
    private bool rayMove = false;
    private float dist;
    private bool attack = false;
    private float stopDist;
    private bool rdyToAttack = false;
    private float timeSinceAttack = 0.0f;
    private bool gameObjectSelected = false;
    private bool objectHasAnimation = true;
    private bool objectHasStats = true;
    private bool objectHasRigidBody = true;
    private bool objectHasAgent = true;
    private bool stopAttacking = false;

    public static UnitController instance = null;

    private PhotonView PV;
    private CharacterController CC;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

        PV = GetComponent<PhotonView>();
        CC = GetComponent<CharacterController>();

        if (!(anim = this.GetComponent<Animator>()))
        {
            objectHasAnimation = false;
        }
        else
        {
            animatorController = new AnimatorController(gameObject);
            //anim.SetFloat("attackSpeed", 1.333f);
        }

        stats.CurrentHealth = stats.MaxHealth;
        Debug.Log(stats.CurrentHealth);
        StartCoroutine("HealOverTime");
    }

    // Update is called once per frame
    void Update()
    {
        stats.AttackSpeed = 1.333f / stats.AttackCooldown;
        if (objectHasAnimation)
        {
            UpdateAnimationSpeed();
        }
        //if(gameObjectSelected)

    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("S")) && gameObjectSelected)
        {
            print("S pressed!");
            SpawnUnitEvent spawnUnit = new SpawnUnitEvent();
            spawnUnit.parent = transform.parent;
            spawnUnit.spawner = transform;

            Transform parent = gameObject.transform.parent;
            spawnUnit.viewID = parent.GetComponent<PhotonView>().ViewID;
            spawnUnit.uNIT_TYPE = SpawnUnitEvent.UNIT_TYPE.WALL;
            spawnUnit.position = new Vector3(3f, 1f, 3f);
            Debug.Log(spawnUnit.uNIT_TYPE);
            spawnUnit.FireEvent();
            
        }

        if (Event.current.Equals(Event.KeyboardEvent("D")) && gameObjectSelected)
        {
            print("D pressed!");
            SpawnUnitEvent spawnUnit = new SpawnUnitEvent();
            spawnUnit.parent = transform.parent;
            spawnUnit.tag = "Player";
            spawnUnit.position = new Vector3(6f, 1f, 6f);
            spawnUnit.uNIT_TYPE = SpawnUnitEvent.UNIT_TYPE.YELLOW_UNIT;
            Debug.Log(spawnUnit.uNIT_TYPE);
            spawnUnit.FireEvent();
        }

    }

    void FixedUpdate()
    {
        //Move(movePoint, stopDist);
        //HandleAttack();
    }

    private void UpdateAnimationSpeed()
    {
        animatorController.SetMoveSpeed(stats.MoveSpeed);
        animatorController.SetAttackSpeed(stats.AttackSpeed);
    }

    private IEnumerator HealOverTime()
    {
        while (true)
        {
            if (stats.CurrentHealth < stats.MaxHealth)
            {
                yield return new WaitForSeconds(1);
                stats.CurrentHealth += 1;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void Move(Vector3 point, float radius)
    {
        if (rayMove)
        {
            dist = Vector3.Distance(transform.position, point);
            //Debug.Log(dist);
            if (dist > radius)
            {
                if (animatorController != null)
                {
                    animatorController.Move();
                }

            }
            else
            {
                Idle();
            }
        }
    }

    private IEnumerator IMove()
    {
        Debug.Log("Entered IMove");
        while (true)
        {
            dist = Vector3.Distance(transform.position, movePoint);
            if (dist > stopDist)
            {
                if (animatorController != null)
                {
                    animatorController.Move();
                }
                yield return new WaitForFixedUpdate();
            }
            else
            {
                Idle();
                yield return null;
            }
        }
    }

    private IEnumerator IHandleAttack()
    {
        stopAttacking = false;
        Debug.Log("Starting Attacking Coroutine on: " + attackedObject.name);
        while (!stopAttacking)
        {
            if (attackedObject != null && attack && rdyToAttack)
            {
                Vector3 lookAtVec = new Vector3(attackedObject.transform.position.x, transform.position.y, attackedObject.transform.position.z);
                transform.LookAt(lookAtVec);

                if (animatorController != null)
                    animatorController.Attack();

                PlayerAttackEvent unitAttackEvent = new PlayerAttackEvent();
                unitAttackEvent.Description = "Unit " + gameObject.name + " has attacked.";
                unitAttackEvent.UnitAttacked = attackedObject;
                unitAttackEvent.UnitAttacker = gameObject;
                unitAttackEvent.UnitStats = stats;
                unitAttackEvent.FireEvent();

                yield return new WaitForSeconds(stats.AttackCooldown);
            }
            else
            {
                yield return null;
            }
        }
        Debug.Log("Stopping Attacking Coroutine");
    }

   /* private void HandleAttack()
    {
        while(attackedObject != null)
        {
            if (rdyToAttack && attack)
            {
                //Debug.Log("Time since last attack: " + (Time.time - timeSinceAttack));
                if (Time.time - timeSinceAttack > stats.AttackCooldown)
                {
                    timeSinceAttack = Time.time;
                    Vector3 lookAtVec = new Vector3(attackedObject.transform.position.x, transform.position.y, attackedObject.transform.position.z);
                    transform.LookAt(lookAtVec);

                    if (animatorController != null)
                        animatorController.Attack();

                    if (attackedObject != null)
                    {
                        //AttackEvent(hit.transform.gameObject, damage);
                        PlayerAttackEvent unitAttackEvent = new PlayerAttackEvent();
                        unitAttackEvent.Description = "Unit " + gameObject.name + " has attacked.";
                        unitAttackEvent.UnitAttacked = attackedObject;
                        unitAttackEvent.UnitAttacker = gameObject;
                        unitAttackEvent.UnitStats = stats;
                        unitAttackEvent.FireEvent();

                    }
                }
            }
        }
    }*/



    void OnEnable()
    {
        UnitDeathEvent.RegisterListener(OnEnemyUnitDeath);
        EnemyAttackEvent.RegisterListener(OnEnemyUnitAttack);
        LeftMouseSelectEvent.RegisterListener(OnLeftMouseSelected);
        RightMouseSelectEvent.RegisterListener(OnRightMouseClick);
        PlayerAttackEvent.RegisterListener(OnAttacked);
    }

    void OnDisable()
    {
        UnitDeathEvent.UnregisterListener(OnEnemyUnitDeath);
        EnemyAttackEvent.UnregisterListener(OnEnemyUnitAttack);
        LeftMouseSelectEvent.UnregisterListener(OnLeftMouseSelected);
        RightMouseSelectEvent.UnregisterListener(OnRightMouseClick);
        PlayerAttackEvent.UnregisterListener(OnAttacked);
    }

    private void OnAttacked(PlayerAttackEvent attackedEvent)
    {
        if(attackedEvent.UnitAttacked == gameObject)
        {
            //Take damage.
            float actualDamageTaken = attackedEvent.UnitStats.Damage;
            actualDamageTaken -= stats.Armor;
            Debug.Log("LOL: " + actualDamageTaken);
            //stats.CurrentHealth -= Mathf.Clamp(actualDamageTaken, 0, float.MaxValue);
            stats.CurrentHealth = stats.CurrentHealth - actualDamageTaken;
            Debug.Log("CH: " + stats.CurrentHealth);
            Debug.Log(gameObject.name + " took " + actualDamageTaken + " damage from " + attackedEvent.UnitAttacker.name);
            if(stats.CurrentHealth <= 0)
            {
                Die(gameObject, attackedEvent.UnitAttacker);
            }
        }
    }

    private void Die(GameObject gameObj, GameObject killer)
    {
        UnitDeathEvent unitDeath = new UnitDeathEvent();
        unitDeath.expDropped = 50;
        unitDeath.UnitDied = gameObj;
        unitDeath.UnitKiller = killer;
        unitDeath.FireEvent();
        
        Destroy(gameObj);
    }

    private void OnRightMouseClick(RightMouseSelectEvent rightClick)
    {
        if (!gameObjectSelected)
        {
            return;
        }

        if (!PV.IsMine)
        {
            Debug.Log("Not the owner");
            return;
        }
        /*
        //Debug.Log(rightClick.clicker.name);
        if(rightClick.clicker.transform.parent == null)
        {
            Debug.Log("No parent found!");
            return;
        }

        Debug.Log(transform.name + " " + rightClick.clicker.transform.parent.name);

        if("Player1GameObject" != rightClick.clicker.transform.parent.name)
        {
            return;
        }*/

        attack = false;
        rdyToAttack = false;
        movePoint = Vector3.zero;
        rayMove = true;


        Debug.Log(rightClick.rightClickGameObject.name);


        if (rightClick.rightClickGameObject.GetPhotonView().IsMine && !rightClick.rightClickGameObject.GetPhotonView().IsSceneView)
        {
            Debug.Log("Clicked on Allied Unit");
            Vector3 lookAtVec = new Vector3(rightClick.rightClickGameObject.transform.position.x, transform.position.y, rightClick.rightClickGameObject.transform.position.z);
            transform.LookAt(lookAtVec);
            movePoint = rightClick.rightClickGameObject.transform.position;
            stopDist = 2.5f;
            stopAttacking = true;
            StartCoroutine("IMove");
        }
        else if (rightClick.rightClickGameObject.GetPhotonView().IsSceneView)
        {
            Debug.Log("Clicked on Environment");
            movePoint = rightClick.mousePosition;
            stopDist = 1.1f;
            stopAttacking = true;
            StartCoroutine("IMove");
        }
        else if (!rightClick.rightClickGameObject.GetPhotonView().IsMine && !rightClick.rightClickGameObject.GetPhotonView().IsSceneView)
        {
            attackedObject = rightClick.rightClickGameObject;

            attack = true;
            movePoint = rightClick.rightClickGameObject.transform.position;
            stopDist = 2.5f;
            StartCoroutine("IMove");
            StartCoroutine("IHandleAttack");

            InteractWithGameObject interact = new InteractWithGameObject();
            interact.InteractingWithThisGameObject = rightClick.rightClickGameObject;
            interact.FireEvent();
        }

        agent.ResetPath();
        agent.SetDestination(movePoint);

        /*
        if (rightClick.rightClickGameObject.transform.parent != transform.parent && rightClick.rightClickGameObject.tag != "Environment")
        {
            attackedObject = rightClick.rightClickGameObject;

            attack = true;
            movePoint = rightClick.rightClickGameObject.transform.position;
            stopDist = 2.5f;
            StartCoroutine("IMove");
            StartCoroutine("IHandleAttack");

            InteractWithGameObject interact = new InteractWithGameObject();
            interact.InteractingWithThisGameObject = rightClick.rightClickGameObject;
            interact.FireEvent();
        }
        else if(rightClick.rightClickGameObject.tag != "Unit")
        {
            Debug.Log("Clicked on Environment");
            movePoint = rightClick.mousePosition;
            stopDist = 1.1f;
            stopAttacking = true;
            StartCoroutine("IMove");// IMove(movePoint, stopDist));
        }
        else if(rightClick.rightClickGameObject.tag == "Unit")
        {
            Debug.Log("Clicked on Allied Unit");
            Vector3 lookAtVec = new Vector3(rightClick.rightClickGameObject.transform.position.x, transform.position.y, rightClick.rightClickGameObject.transform.position.z);
            transform.LookAt(lookAtVec);
            movePoint = rightClick.rightClickGameObject.transform.position;
            stopDist = 2.5f;
            stopAttacking = true;
            StartCoroutine("IMove");
        }
        agent.ResetPath();
        agent.SetDestination(movePoint);*/

    }

    private void OnLeftMouseSelected(LeftMouseSelectEvent objectSelected)
    {
        if(objectSelected.selectedGameObject == transform.gameObject)
        {
            gameObjectSelected = true;
        }
        else
        {
            gameObjectSelected = false;
        }
    }

    private void TakeDamage()
    {
        
    }

    private void OnEnemyUnitAttack(EnemyAttackEvent unitAttack)
    {
        float actualDamageTaken = unitAttack.UnitStats.Damage;
        actualDamageTaken -= stats.Armor;
        stats.CurrentHealth -= actualDamageTaken;

        Debug.Log(unitAttack.UnitAttacked.name + " took " + actualDamageTaken + " damage from " + unitAttack.UnitAttacker.name);
        Debug.Log(stats.CurrentHealth);
    }

    private void OnEnemyUnitDeath(UnitDeathEvent unitDeath)
    {
        if(unitDeath.UnitKiller == gameObject)
        {
            stats.CurrentExp += unitDeath.expDropped;
            if (stats.CurrentExp >= stats.ExpToNextLevel)
            {
                LevelUp();
            }
        }

        if (attackedObject == unitDeath.UnitDied)
        {
            //Debug.Log("stopAttacking = true");
            stopAttacking = true;
        }
    }

    private void LevelUp()
    {
        //Exp doesnt carry over on level up e.g. 70/100 -> 120/100 -> 0/150 instead of 20/150
        stats.CurrentLevel++;
        stats.CurrentExp = 0;
        stats.ExpToNextLevel += 25 * stats.CurrentLevel;
        stats.Damage += stats.CurrentLevel * 2;
        stats.MaxHealth += 25 + stats.CurrentLevel * 2;
        stats.CurrentHealth += 25 + stats.CurrentLevel * 2;
        stats.Armor += 1;
        Debug.Log("Level up!");
    }

    private void Idle()
    {
        agent.isStopped = true;
        rb.isKinematic = false;
        rayMove = false;
        rb.drag = 10;
        rdyToAttack = true;
        //anim.Play("Idle");
    }
}
