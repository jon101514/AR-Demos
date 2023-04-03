using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankManager : MonoBehaviour
{

    public string shootPrefabName, dontShootPrefabName;
    public List<Transform> spawnPositions; // Publicly populate this list.

    private float spawnInterval;
    private float spawnShootChance;

    private const float SPAWN_SHOOT_BASE = 1/2f;
    private const float MAX_SPAWN_INTERVAL = 4f;
    private const float MIN_SPAWN_INTERVAL = 3/2f;
    private const float TIME_TO_MAX_DIFF = 90f;
    private const float SPAWN_VARIANCE = 1f;

    private void Start() {
        spawnInterval = MAX_SPAWN_INTERVAL;
        spawnShootChance = SPAWN_SHOOT_BASE;
    }

    private void Update() {
        spawnInterval -= Time.deltaTime;
        if (spawnInterval <= 0f) {
            Spawn();
        }
    }

    private void Spawn() {
        spawnInterval = Mathf.Lerp(MAX_SPAWN_INTERVAL, MIN_SPAWN_INTERVAL, (Time.timeSinceLevelLoad / TIME_TO_MAX_DIFF));
        if (Random.Range(0f, 1f) < spawnShootChance) { // Spawn a shootable
            ObjectPoolManager.instance.SpawnObjectFromPool(shootPrefabName, spawnPositions[Random.Range(0, spawnPositions.Count)]);
            spawnShootChance = SPAWN_SHOOT_BASE;
        } else { // Spawn a don't-shoot object.
            ObjectPoolManager.instance.SpawnObjectFromPool(dontShootPrefabName, spawnPositions[Random.Range(0, spawnPositions.Count)]);
            spawnShootChance += (spawnShootChance / 2f);
        }
    }
}
