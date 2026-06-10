using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;

    private float spawnTimer;
    private float spawnWait = 0f;

    private float bonusSpawnWait = 0f;

    private float spawnY = 15f;
    public float[] spawnXs = new float[] {-4.5f, -3f, -1.5f, 0f, 1.5f, 3f, 4.5f };
    private int xSlots;

    private void Start()
    {
        spawnTimer = 1 / spawnRate;
        xSlots = spawnXs.Length;
    }

    void Update()
    {
        spawnWait += Time.deltaTime;
        bonusSpawnWait += Time.deltaTime;

        if (ObjectLoader.Instance.ready && bonusSpawnWait > 10)
        {
            bonusSpawnWait = 0f;
            Instantiate(
                ObjectLoader.Instance.bonusItem,
                new Vector3(Random.Range(-4.5f, 4.5f), Random.Range(-8f, 8f), 0),
                Quaternion.identity
            );
        }

        if (!ObjectLoader.Instance.ready || spawnWait < spawnTimer) return;

        spawnWait = 0f;

        GameObject toSpawn = ObjectLoader.Instance.scoreItems[
            Random.Range(0, ObjectLoader.Instance.scoreItems.Count)
        ];
        Instantiate(
            toSpawn,
            new Vector3(spawnXs[Random.Range(0, xSlots)], spawnY, 0),
            Quaternion.identity
        );
    }
}
