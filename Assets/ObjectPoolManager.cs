using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // SINGLETON
	public static ObjectPoolManager instance;

    // PUBLIC
    public List<GameObject> objectsToPool;
    public List<string> objectKeys;
    public List<int> objectCounts;

    // PRIVATE
    private Dictionary<string, List<GameObject>> dict;

    private void Awake() {
		if (instance == null) {
			instance = this;
		}
        dict = new Dictionary<string, List<GameObject>>();
	}

    private void OnEnable() {
        SetupObjectPools();
    }


    /** Spawn a GameObject from a pool and return either that or null. */
    public GameObject SpawnObjectFromPool(string key, Transform tm) {
        Debug.Log("Try spawning: " + key);
        if (dict.ContainsKey(key)) {
            List<GameObject> currList = dict[key];
            for (int i = 0; i < currList.Count; i++) {
                if (!currList[i].activeInHierarchy) {
                    Debug.Log("Spawn Success!: " + key);

                    currList[i].SetActive(true);
                    currList[i].transform.position = tm.position;
                    // currList[i].transform.rotation = tm.rotation;

                    return currList[i];
                }
            }
        }
        return null;
    }

    private string DebugString() {
        string result = "";
        for (int i = 0; i < objectKeys.Count; i++) {
            result += objectKeys[i] + " - " + (CountActive(dict[objectKeys[i]]).ToString()) + " / " + objectCounts[i] + "\n";
        }
        if (result == "") {
            result = "nothing";
        }
        return result;
    }

    private int CountActive(List<GameObject> objs) {
        int result = 0;
        foreach (GameObject obj in objs) { 
            if (obj.activeInHierarchy) { result++; }
        }
        return result;
    }

    private void SetupObjectPools() {
        // Create object pools and assign them to the dictionary
        List<GameObject> currList;
        GameObject currObj;
        for (int i = 0; i < objectsToPool.Count; i++) {
            currList = new List<GameObject>();
            for (int j = 0; j < objectCounts[i]; j++) {
                currObj = (GameObject) Instantiate(objectsToPool[i]);
                currObj.SetActive(false);
                currList.Add(currObj);
            }
            dict.Add(objectKeys[i], currList);
        }
    }
}
