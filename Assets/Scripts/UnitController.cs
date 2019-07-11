using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class UnitController : MonoBehaviour
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

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!(anim = this.GetComponent<Animator>()))
            objectHasAnimation = false;
        else
        {
            animatorController = new AnimatorController(gameObject);
        }

        if (!(stats = this.GetComponent<CharacterStats>()))
            objectHasStats = false;

        if (!(agent = this.GetComponent<NavMeshAgent>()))
        {
            objectHasAgent = false;
        }
        else
        {
            agent.updateRotation = true;
        }


        if (!(rb = this.GetComponent<Rigidbody>()))
            objectHasRigidBody = false;

        stats.CurrentHealth = stats.MaxHealth;
        StartCoroutine("HealOverTime");
        if (objectHasAnimation)
        {
            anim.SetFloat("attackSpeed", 1.333f);
        }

    }

    // Update is called once per frame
    void Update()
    {

        stats.AttackSpeed = 1.333f / stats.AttackCooldown;
        if (objectHasAnimation)
            UpdateAnimationSpeed();
    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("S")) && gameObjectSelected)
        {
            print("S pressed!");
            SpawnUnitEvent spawnUnit = new SpawnUnitEvent();
            spawnUnit.parent = transform.parent;
            spawnUnit.position = new Vector3(3f, 1f, 3f);
            spawnUnit.uNIT_TYPE = SpawnUnitEvent.UNIT_TYPE.PlayerOtherUnit;
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
        Move(movePoint, stopDist);
        //HandleAttack();
    }

    private void UpdateAnimationSpeed()
    {
        animatorController.SetMoveSpeed(stats.MoveSpeed);
        animatorController.SetMoveSpeed(stats.AttackSpeed);
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
                //anim.Play("Move");
                if(animatorController != null)
                    animatorController.Move();
            }
            else
            {
                Idle();
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

    private void HandleAttack()
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
    }



    void OnEnable()
    {
        UnitDeathEvent.RegisterListener(OnEnemyUnitDeath);
        EnemyAttackEvent.RegisterListener(OnEnemyUnitAttack);
        LeftMouseSelectEvent.RegisterListener(OnLeftMouseSelected);
        RightMouseSelectEvent.RegisterListener(OnRightMouseClick);
    }

    private void OnRightMouseClick(RightMouseSelectEvent rightClick)
    {
        if (!gameObjectSelected)
        {
            return;
        }


        attack = false;
        rdyToAttack = false;
        movePoint = Vector3.zero;
        rayMove = true;

        if (!rightClick.rightClickGameObject.transform.IsChildOf(transform.parent) && rightClick.rightClickGameObject.tag == "AttackAble")
        {
            attackedObject = rightClick.rightClickGameObject;

            attack = true;
            movePoint = rightClick.rightClickGameObject.transform.position;
            stopDist = 2.5f;

            StartCoroutine("IHandleAttack");

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
        if(objectSelected.selectedGameObject == transform.gameObject)
        {
            gameObjectSelected = true;
        }
        else
        {
            gameObjectSelected = false;
        }
    }

    private void OnEnemyUnitAttack(EnemyAttackEvent unitAttack)
    {
        int actualDamageTaken = unitAttack.UnitStats.Damage;
        actualDamageTaken -= stats.Armor;
        actualDamageTaken = Mathf.Clamp(actualDamageTaken, 0, int.MaxValue);
        stats.CurrentHealth -= actualDamageTaken;

        Debug.Log(unitAttack.UnitAttacked.name + " took " + actualDamageTaken + " damage from " + unitAttack.UnitAttacker.name);
    }

    private void OnEnemyUnitDeath(UnitDeathEvent unitDeath)
    {
        stats.CurrentExp += unitDeath.expDropped;
        if(stats.CurrentExp >= stats.ExpToNextLevel)
        {

            LevelUp();
        }
        if (attackedObject == unitDeath.UnitDied)
        {
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
