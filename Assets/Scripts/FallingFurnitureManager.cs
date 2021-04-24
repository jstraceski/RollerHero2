using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFurnitureManager : MonoBehaviour
{
    // Rigid body furniture items
    public GameObject[] furnitureItems;
    public GameObject keyPrefab;

    int numKeysCollected;
    GameObject[] keys;
    public Transform[] keyTransforms;
    GameObject griffen;
    GameObject entrance;
    GameObject exit;
    bool isLevelStarted;
    int numFurniturePrefabs;

    // Start is called before the first frame update
    void Start()
    {
        keys = GameObject.FindGameObjectsWithTag("Key");
        numKeysCollected = 0;
        griffen = GameObject.FindGameObjectWithTag("Player");
        FallingItemBehavior.originalPosition = griffen.transform.position;
        entrance = GameObject.FindGameObjectWithTag("Entrance");
        exit = GameObject.FindGameObjectWithTag("Exit");
        isLevelStarted = false;
        numFurniturePrefabs = furnitureItems.Length;
        keyTransforms = new Transform[keys.Length];
        for (int ii = 0; ii < keys.Length; ii++)
        {
            keyTransforms[ii] = keys[ii].transform;
        }
        Debug.Log("Keys = " + keys.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(griffen.transform.position, entrance.transform.position) > 5 && !isLevelStarted)
        {
            isLevelStarted = true;
            InvokeRepeating("SpawnFurniture", 1, 1);
        }
    }

    public void KeyCollected()
    {
        Debug.Log("Key Collected");
        numKeysCollected++;
        if (numKeysCollected == keys.Length)
        {
            Debug.Log("All Keys Collected");
            FindObjectOfType<ObjectiveSliderBehavior>().KeyCollected();
        }
    }

    void SpawnFurniture()
    {
        Spawn();
       //Spawn();
    }

    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(160, 174), 20, Random.Range(0, 23));
        GameObject furniturePrefab = furnitureItems[Random.Range(0, numFurniturePrefabs - 1)];
        GameObject spawnedFurniture = Instantiate(furniturePrefab, spawnPosition, transform.rotation)
            as GameObject;
        spawnedFurniture.transform.parent = gameObject.transform;
    }

    public void RespawnKeys()
    {
        for (int ii = 0; ii < keys.Length; ii++)
            {
                Destroy(keys[ii]);
                GameObject newKey = Instantiate<GameObject>(keyPrefab, keyTransforms[ii].position, keyTransforms[ii].rotation) as GameObject;
            newKey.transform.position = keyTransforms[ii].position;
            Debug.Log("New key position (" + newKey.transform.position.x + ", " + newKey.transform.position.y + ", " + newKey.transform.position.z + ")");
            }

    }
}
