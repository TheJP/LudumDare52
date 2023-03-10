using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyPrefab
{
    [field: SerializeField]
    public Enemy Prefab { get; set; }

    [field: SerializeField]
    public int DifficultyRating { get; set; }
}

public class ResourceFieldManager : MonoBehaviour
{
    [SerializeField]
    private ResourceField[] fieldPrefabs;

    [SerializeField]
    private float spawnRange = 100f;

    [SerializeField]
    private float protectedRange = 10f;

    [field: SerializeField]
    public int CurrentDifficulty { get; set; } = 10;

    [SerializeField]
    private Transform enemyParent;

    [SerializeField]
    private EnemyPrefab[] enemyPrefabs;

    [SerializeField]
    private float spawnCooldown = 100f;

    private Base playerBase;
    private Movement player;

    private readonly HashSet<ResourceField> fields = new();
    public IEnumerable<ResourceField> Fields => fields.AsEnumerable();
    private float lastSpawn;

    public void Start()
    {
        playerBase = FindObjectOfType<Base>();
        player = FindObjectOfType<Movement>();
        lastSpawn = Time.time;
    }

    public void Update()
    {
        foreach (var type in ResourceTypeExtensions.Types)
        {
            if (fields.Count(f => f.Type == type) == 0)
            {
                SpawnField(type);
            }
        }

        if (Time.time - lastSpawn < spawnCooldown) { return; }
        lastSpawn = Time.time;
        var types = ResourceTypeExtensions.Types;
        SpawnField(types[Random.Range(0, types.Length)]);
    }

    private void SpawnField(ResourceType type)
    {
        var prefab = fieldPrefabs[Random.Range(0, fieldPrefabs.Length)];
        Debug.Assert(prefab != null);
        Vector2 fieldPosition = RandomFieldPosition();
        var newField = Instantiate(prefab, fieldPosition, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform);

        newField.OnCollected += () =>
        {
            fields.Remove(newField);
            Destroy(newField.gameObject, 10); // Delay destroy so resources have time to arrive at base (visually).
        };
        newField.UpdateResourceType(type);
        fields.Add(newField);
        SpawnEnemies(newField);
    }

    private Vector2 RandomFieldPosition()
    {
        Vector2 middle = (playerBase.transform.position + player.transform.position) * 0.5f;

        const int Tries = 100;
        int currentTry = 0;
        Vector2 fieldPosition;
        do
        {
            ++currentTry;
            fieldPosition = middle + Random.insideUnitCircle * spawnRange;
        } while (
            currentTry < Tries && (
                (fieldPosition - (Vector2)player.transform.position).sqrMagnitude <= protectedRange * protectedRange ||
                (fieldPosition - (Vector2)playerBase.transform.position).sqrMagnitude <= protectedRange * protectedRange
        ));

        if (currentTry >= Tries) { Debug.LogWarning("Could not find a good position to spawn field (spawning at sub-optimal location now)"); }

        return fieldPosition;
    }

    private void SpawnEnemies(ResourceField field)
    {
        var spawnPointsParent = field.EnemySpawnParent;

        const int Tries = 100;
        int currentTry = 0;
        EnemyPrefab[] selected;
        do
        {
            ++currentTry;
            var amount = Random.Range(0, spawnPointsParent.childCount);
            selected = new EnemyPrefab[amount];
            for (int i = 0; i < amount; ++i)
            {
                selected[i] = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            }
        } while (currentTry < Tries && selected.Sum(p => p.DifficultyRating) > CurrentDifficulty);

        if (currentTry >= Tries) { Debug.LogWarning("Could not find enemy composition for field"); return; }

        var spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>().ToHashSet();
        spawnPoints.Remove(spawnPointsParent);
        Debug.Assert(spawnPoints.Count >= selected.Length);
        foreach (var prefab in selected)
        {
            var spawnPoint = spawnPoints.Skip(Random.Range(0, spawnPoints.Count)).First();
            spawnPoints.Remove(spawnPoint);
            var enemy = Instantiate(prefab.Prefab, spawnPoint.position, spawnPoint.rotation, enemyParent);
            enemy.Protectee = field;
            field.AddProtector(enemy);
        }
    }
}
