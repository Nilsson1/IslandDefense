using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameHandler : MonoBehaviour
{
    public NavMeshSurface surface;
    PlayerManager pm;
    private void Start()
    {
        surface.BuildNavMesh();
        //pm = PlayerManager.instance;

    }

    private void Update()
    {
        surface.BuildNavMesh();
    }
}
