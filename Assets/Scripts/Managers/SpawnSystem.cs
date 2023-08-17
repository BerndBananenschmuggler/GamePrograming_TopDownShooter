using Assets.Scripts.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public event Action<int> EnemyCountChanged;
    public event Action SpawnStarted;
    public event Action SpawnCompleted;
    public event Action WaveCleared;
    
    [SerializeField] private List<Transform> _possibleSpawnPositions;
    [SerializeField] private GameObject _objectToSpawn;
    //[Range(1, 50)]
    //[SerializeField] private int _maxObjectsAliveCount = 1;

    private int _currentObjectsAliveCount = 0;
    private float _spawnDelay = 2.0f;
    private Coroutine _spawnRoutine;

    private List<Health> _spawnedObjectsHealthList = new List<Health>();
    private bool _isWaveSpawnedEntirely = false;

    private void Awake()
    {
        if (_objectToSpawn == null)
            Debug.LogWarning("Spawning Object missing");
        if (_possibleSpawnPositions == null)
            Debug.LogWarning("Spawn Positions missing");
    }

    private void Start()
    {
        //if (_spawnRoutine == null)
        //    _spawnRoutine = StartCoroutine(SpawnObjects(5));
    }

    public void SpawnWaveEntities(GameObject[] objectsToSpawn)
    {
        if (_spawnRoutine == null)
            _spawnRoutine = StartCoroutine(SpawnObjects(objectsToSpawn));
    }

    // Needs to be started every new Wave
    private IEnumerator SpawnObjects(GameObject[] objectsToSpawn)
    {
        _isWaveSpawnedEntirely = false;

        // Unsub all healths Death event
        foreach (Health health in _spawnedObjectsHealthList)
            health.Died -= HandleEntityDied;

        // Clear List of HealthScripts
        _spawnedObjectsHealthList.Clear();

        // Randomize SpawnPositions
        _possibleSpawnPositions.Randomize();

        // Trigger Event
        SpawnStarted?.Invoke();

        // Init values
        int spawnedObjects = 0;
        int spawnPositionIndex = 0;

        // Spawn while not all objects are spawned
        while (spawnedObjects < objectsToSpawn.Length)
        {
            // Spawn Object
            GameObject spawnedEntity = Instantiate(objectsToSpawn[spawnedObjects], _possibleSpawnPositions[spawnPositionIndex].position, Quaternion.identity);

            // Add Objects health to list
            Health entityHealth = spawnedEntity.GetComponent<Health>();
            if(entityHealth != null)
            {
                _spawnedObjectsHealthList.Add(entityHealth);
                entityHealth.Died += HandleEntityDied;
            }

            // spawnedObjects++
            spawnedObjects++;

            // spawnPositionIndex++
            // Reset when index out of bounds
            spawnPositionIndex++;
            if (spawnPositionIndex > _possibleSpawnPositions.Count - 1)
                spawnPositionIndex = 0;

            _currentObjectsAliveCount++;
            EnemyCountChanged?.Invoke(_currentObjectsAliveCount);

            Debug.Log($"Enitity spawned - Count: {_currentObjectsAliveCount}");

            // If lastObject spawned -> no wait time
            // Else waitForSeconds
            if (spawnedObjects != objectsToSpawn.Length)
            {
                yield return new WaitForSeconds(_spawnDelay);
            }

            yield return null;
        }

        SpawnCompleted?.Invoke();

        _isWaveSpawnedEntirely = true;

        _spawnRoutine = null;
    }

    private void HandleEntityDied(object sender)
    {
        _currentObjectsAliveCount--;
        EnemyCountChanged?.Invoke(_currentObjectsAliveCount);

        Debug.Log($"Enitity died - Count: {_currentObjectsAliveCount}");

        _spawnedObjectsHealthList.Remove((Health)sender);

        if (_currentObjectsAliveCount <= 0 && _isWaveSpawnedEntirely) 
            WaveCleared?.Invoke();
    }
}
