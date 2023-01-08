using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TurretPrefab
{
    [field: SerializeField]
    public TurretType Type { get; set; }

    [field: SerializeField]
    public Turret Prefab { get; set; }
}

public class TurretManager : MonoBehaviour
{
    [SerializeField]
    private TurretPrefab[] turretPrefabs;

    [SerializeField]
    private Transform spawnPointParent;

    private readonly List<Turret> turrets = new();
    private readonly HashSet<Transform> freeSpawns = new();

    public void Start()
    {
        foreach (var child in spawnPointParent.GetComponentsInChildren<Transform>())
        {
            freeSpawns.Add(child);
        }
        freeSpawns.Remove(spawnPointParent);
    }

    public bool SpawnTurret(TurretType type)
    {
        if (freeSpawns.Count == 0) { return false; }
        var turret = turretPrefabs.FirstOrDefault(t => t.Type == type);
        if (turret == null) { return false; }

        var spawn = freeSpawns.Skip(Random.Range(0, freeSpawns.Count)).First();
        var newTurret = Instantiate(turret.Prefab, spawn.position, Quaternion.identity, transform);
        turrets.Add(newTurret);
        freeSpawns.Remove(spawn);

        return true;
    }
}
