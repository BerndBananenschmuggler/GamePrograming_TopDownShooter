using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SpawnSystem _spawnSystem;

    // Start is called before the first frame update
    void Start()
    {
        _spawnSystem.OnSpawnStarted += HandleSpawnStarted;
        _spawnSystem.OnSpawnCompleted += HandleSpawnCompleted;
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
