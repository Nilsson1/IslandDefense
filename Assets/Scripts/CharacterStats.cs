using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5.0f);
    }

    void Start()
    {
        CurrentExp = 0;
        //Temporary way of assigning the Player 20 base dmg.
        if (gameObject.GetComponent<UnitController>() != null)
        {
            Damage = 20;
            Armor = 4;
            MoveSpeed = 1.0f;
            CurrentLevel = 1;
        }
        else
        {
            Damage = 7;
        }
    }

    public int Damage { get; set; }
    public int CurrentExp { get; set; } = 0;
    public int CurrentLevel { get; set; }
    public int MaxHealth { get; set; } = 100;
    public int CurrentHealth { get; set; } = 100;
    public float AttackCooldown { get; set; } = 1.0f;
    public int Armor { get; set; } = 0;
    public float AttackSpeed { get; set; }
    public float MoveSpeed { get; set; }
    public int ExpToNextLevel { get; set; } = 100;

}
