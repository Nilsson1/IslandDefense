using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        SpawnUnitEvent.RegisterListener(OnSpawnUnit);
    }

    private void OnSpawnUnit(SpawnUnitEvent spawnUnit)
    {
        Debug.Log("Prefabs/" + spawnUnit.uNIT_TYPE);
        GameObject myPrefab = (GameObject)Resources.Load("Prefabs/" + spawnUnit.uNIT_TYPE, typeof(GameObject));

        GameObject newUnit = Instantiate(myPrefab, spawnUnit.position, Quaternion.identity);
        newUnit.transform.SetParent(spawnUnit.parent);
        newUnit.tag = spawnUnit.tag;

    }
}
