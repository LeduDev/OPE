using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectPrefabs;

    //[SerializeField]
    //private Transform[] spawnPoints;

    public Transform enemies;
    public Transform towers;

    public GameObject GetObject(string type)
    {
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == type)
            {
                //GameObject newObject = Instantiate(objectPrefabs[i]);
                //newObject.name = type;
                switch (objectPrefabs[i].tag)
                {
                    case "enemy":
                        GameObject newObject = Instantiate(objectPrefabs[i]);
                        newObject.name = type;
                        newObject.transform.SetParent(enemies);
                        return newObject;
                        //break;
                }
                
            }
        }
        return null;
    }

    public void SpawnEnemyPos()
    {

    }
}
