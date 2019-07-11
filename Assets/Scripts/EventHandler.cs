using System;
using UnityEngine;

public abstract class EventHandler<T> where T : EventHandler<T>
{
    /*
        * The base Event,
        * might have some generic text
        * for doing Debug.Log?
        */
    public string Description;

    private bool hasFired;
    public delegate void EventListener(T info);
    private static event EventListener Listeners;

    public static void RegisterListener(EventListener listener)
    {
        Listeners += listener;
    }

    public static void UnregisterListener(EventListener listener)
    {
        Listeners -= listener;
    }

    public void FireEvent()
    {
        if (hasFired)
        {
            throw new Exception("This event has already fired, to prevent infinite loops you can't refire an event");
        }
        hasFired = true;
        if (Listeners != null)
        {
            Listeners(this as T);
        }
    }
}

///////////////////////////////THESE ARE FIREABLE EVENTS
public class DebugEvent : EventHandler<DebugEvent>
{
    public int VerbosityLevel;
}

public class UnitDeathEvent : EventHandler<UnitDeathEvent>
{
    public GameObject UnitDied;
    public int expDropped;
}

public class PlayerAttackEvent : EventHandler<PlayerAttackEvent>
{
    public GameObject UnitAttacked;
    public GameObject UnitAttacker;
    public CharacterStats UnitStats;
}

public class EnemyAttackEvent : EventHandler<EnemyAttackEvent>
{
    public GameObject UnitAttacked;
    public GameObject UnitAttacker;
    public CharacterStats UnitStats;
}

public class UIUpdateEvent : EventHandler<UIUpdateEvent>
{
        
}

public class InteractWithGameObject : EventHandler<InteractWithGameObject>
{
    public GameObject InteractingWithThisGameObject;
}

public class LeftMouseSelectEvent : EventHandler<LeftMouseSelectEvent>
{
    public GameObject selectedGameObject;
}

public class RightMouseSelectEvent : EventHandler<RightMouseSelectEvent>
{
    public GameObject rightClickGameObject;
    public Vector3 mousePosition;
}



public class SpawnUnitEvent : EventHandler<SpawnUnitEvent>
{
    
    public enum UNIT_TYPE
    {
        BLUE_UNIT, OTHER_UNIT, PlayerOtherUnit, YELLOW_UNIT
    }

    public UNIT_TYPE uNIT_TYPE;
    public Vector3 position;
    public Transform parent;
    public string tag;
}