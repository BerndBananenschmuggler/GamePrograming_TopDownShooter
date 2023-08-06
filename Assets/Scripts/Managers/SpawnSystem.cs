using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public event Action OnSpawnStarted;
    public event Action OnSpawnCompleted;

    public int CurrentWave { get; private set; } = 0;

    [Range(1,50)] 
    [SerializeField] private int _maxWaveCount = 10;
    [SerializeField] private List<Transform> _possibleSpawnPositions;
    [SerializeField] private GameObject _objectToSpawn;
    [Range(1, 50)]
    [SerializeField] private int _maxObjectsAliveCount = 1;

    private int _currentObjectsAliveCount = 0;
    private float _spawnDelay = 2.0f;
    private Coroutine _spawnRoutine;

    private void Awake()
    {
        if (_objectToSpawn == null)
            Debug.LogWarning("Spawning Object missing");
        if (_possibleSpawnPositions == null)
            Debug.LogWarning("Spawn Positions missing");
    }

    private void Start()
    {
        if (_spawnRoutine == null)
            _spawnRoutine = StartCoroutine(SpawnObjects(5));
    }

    // Needs to be started every new Wave
    private IEnumerator SpawnObjects(int objectCountPerWave)
    {
        OnSpawnStarted?.Invoke();

        int spawnedObjects = 0;
        int spawnPositionIndex = 0;

        while (spawnedObjects < objectCountPerWave)
        {
            // Objekt spawnen
            Instantiate(_objectToSpawn, _possibleSpawnPositions[spawnPositionIndex].position, Quaternion.identity);

            // spawnedObjects++
            spawnedObjects++;

            // spawnPositionIndex++
            // Reset when index out of bounds
            spawnPositionIndex++;
            if (spawnPositionIndex > _possibleSpawnPositions.Count - 1)
                spawnPositionIndex = 0;

            // If lastObject spawned -> no wait time
            // Else waitForSeconds
            if (spawnedObjects != objectCountPerWave)
            {
                yield return new WaitForSeconds(_spawnDelay);
            }

            yield return null;
        }

        OnSpawnCompleted?.Invoke();

        _spawnRoutine = null;
    }
}
