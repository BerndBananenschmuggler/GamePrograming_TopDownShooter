using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CurrentWave { get; private set; } = 0;

    [SerializeField] private GameObject TestEntity;

    [SerializeField] private List<WaveInfo> _enemyWaves = new List<WaveInfo>();
    
    private SpawnSystem _spawnSystem;

    private void OnEnable()
    {
        _spawnSystem = GetComponent<SpawnSystem>();

        // Track whether the spawned wave has been cleared
        _spawnSystem.WaveCleared += HandleWaveCleared;

        // Create Info for 
        _enemyWaves.Clear();
        _enemyWaves.Add(new WaveInfo(1, new GameObject[] { TestEntity, TestEntity, TestEntity }));
        _enemyWaves.Add(new WaveInfo(2, new GameObject[] { TestEntity, TestEntity, TestEntity, TestEntity }));
        _enemyWaves.Add(new WaveInfo(3, new GameObject[] { TestEntity, TestEntity, TestEntity, TestEntity, TestEntity }));
        _enemyWaves.Add(new WaveInfo(4, new GameObject[] { TestEntity, TestEntity, TestEntity, TestEntity, TestEntity, TestEntity }));
        _enemyWaves.Add(new WaveInfo(5, new GameObject[] { TestEntity, TestEntity, TestEntity, TestEntity, TestEntity, TestEntity, TestEntity }));
        _enemyWaves.Add(new WaveInfo(6, new GameObject[] { TestEntity, TestEntity, TestEntity, TestEntity, TestEntity, TestEntity, TestEntity, TestEntity }));

        // Simple Spawn Call
        _spawnSystem.SpawnWaveEntities(_enemyWaves[0].Entities);
    }

    private void OnDisable()
    {
        _spawnSystem.WaveCleared -= HandleWaveCleared;
    }

    private void HandleWaveCleared()
    {
        Debug.Log($"Wave {CurrentWave + 1} cleared");

        CurrentWave++;
        _spawnSystem.SpawnWaveEntities(_enemyWaves[CurrentWave].Entities);
    }

    private void HandleSpawnCompleted()
    {
        throw new System.NotImplementedException();
    }

    private void HandleSpawnStarted()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
