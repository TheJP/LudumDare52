using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private float graceTime = 0f;

    [SerializeField]
    private float waveCooldown = 100f;

    [SerializeField]
    private float minimalSpawnDistance = 20f;

    [SerializeField]
    private Transform enemyParent;

    [SerializeField]
    private GameObject[] wavePrefabs;

    [SerializeField]
    public int NextWave { get; private set; } = 0;

    private Base playerBase;
    private Movement player;
    private float startWave;

    public void Start()
    {
        playerBase = FindObjectOfType<Base>();
        player = FindObjectOfType<Movement>();
        startWave = Time.time + graceTime - waveCooldown;
    }

    private void Update()
    {
        if (NextWave >= wavePrefabs.Length) { return; } // TODO: Victory Screen?
        if (Time.time - startWave < waveCooldown) { return; }
        startWave = Time.time;

        Vector2 distance = playerBase.transform.position - player.transform.position;
        Vector2 middle = (Vector2)player.transform.position + 0.5f * distance;
        float spawnRadius = minimalSpawnDistance + 0.5f * distance.magnitude;
        Vector2 spawnPosition = middle + spawnRadius * (Vector2)Random.onUnitSphere;
        Instantiate(wavePrefabs[NextWave], spawnPosition, Quaternion.identity, enemyParent);
        ++NextWave;
    }
}
