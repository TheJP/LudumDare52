using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private UIDocument waveUi;

    [SerializeField]
    private GameObject[] wavePrefabs;

    [SerializeField]
    public int NextWave { get; private set; } = 0;

    private Base playerBase;
    private Movement player;
    private float startWave;

    private Label waveHeading;
    private Label waveCountdown;

    public void Start()
    {
        waveHeading = waveUi.rootVisualElement.Q<Label>("wave-label");
        waveCountdown = waveUi.rootVisualElement.Q<Label>("countdown-label");
        waveHeading.text = "First Wave";

        playerBase = FindObjectOfType<Base>();
        player = FindObjectOfType<Movement>();
        startWave = Time.time + graceTime - waveCooldown;
    }

    private void Update()
    {
        if (NextWave >= wavePrefabs.Length)
        {
            // TODO: Victory Screen?
            waveCountdown.text = "";
            return;
        }

        float countdown = waveCooldown - (Time.time - startWave);
        waveCountdown.text = countdown < 10f ? $"{countdown:F1}" : $"{countdown:F0}";

        if (countdown > 0f) { return; }
        startWave = Time.time;

        Vector2 distance = playerBase.transform.position - player.transform.position;
        Vector2 middle = (Vector2)player.transform.position + 0.5f * distance;
        float spawnRadius = minimalSpawnDistance + 0.5f * distance.magnitude;
        Vector2 spawnPosition = middle + spawnRadius * (Vector2)Random.onUnitSphere;
        Instantiate(wavePrefabs[NextWave], spawnPosition, Quaternion.identity, enemyParent);
        ++NextWave;

        waveHeading.text = $"Wave {NextWave}/{wavePrefabs.Length}";
    }
}
