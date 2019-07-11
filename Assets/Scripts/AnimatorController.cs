using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController
{

    private GameObject objectToPerfomAnimation;
    private Animator anim;
    public AnimatorController(GameObject gameObject)
    {
        objectToPerfomAnimation = gameObject;
        anim = objectToPerfomAnimation.GetComponent<Animator>();
    }

    public void Move()
    {
        anim.Play("Move");
    }

    public void Attack()
    {
        anim.Play("Attack");
    }

    public void Idle()
    {

    }

    public void SetMoveSpeed(float moveSpeed)
    {
        anim.SetFloat("moveSpeed", moveSpeed);
    }

    public void SetAttackSpeed(float attackSpeed)
    {
        anim.SetFloat("attackSpeed", attackSpeed);
    }

}
