using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> WaveCountChanged;
    public event Action GamePaused;
    public event Action GameResumed;
    public event Action GameStarted;
    //public event Action GameFinished;
    public event Action GameFailed;
    public event Action WaveCleared;

    public int CurrentWave { get; private set; } = 0;
    public SpawnSystem SpawnSystem { get; private set; }


    [SerializeField] private GameObject _enemyPrefab;


    [SerializeField] private float _newWaveDelayTime = 2;
    [SerializeField] private List<WaveInfo> _enemyWaves = new List<WaveInfo>();

    private Coroutine _waveInvokationRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            SpawnSystem = GetComponent<SpawnSystem>();

            // Clear All existing waves
            _enemyWaves.Clear();
            
            // Create First Wave with 3 Enemies
            _enemyWaves.Add(new WaveInfo(1, new GameObject[] { _enemyPrefab, _enemyPrefab, _enemyPrefab }));            
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (SpawnSystem != null)
            SpawnSystem.WaveCleared += HandleWaveCleared;
        if (PlayerController.Instance != null)
            PlayerController.Instance.gameObject.GetComponent<Health>().Died += HandlePlayerDied;
    }    

    private void OnDisable()
    {
        if (SpawnSystem != null)
            SpawnSystem.WaveCleared -= HandleWaveCleared;
        if (PlayerController.Instance != null)
            PlayerController.Instance.gameObject.GetComponent<Health>().Died -= HandlePlayerDied;
    }    

    private void Start()
    {
        // Initial Resumed-Call to hide Menus
        GameStarted?.Invoke();

        if (Time.timeScale == 0)
            Time.timeScale = 1;

        CurrentWave = 1;
        WaveCountChanged?.Invoke(CurrentWave);


        // Simple Spawn Call
        SpawnSystem.SpawnWaveEntities(_enemyWaves[0].Entities);
    }

    public void TogglePause()
    {
        if (Time.timeScale != 0)
        {
            GamePaused?.Invoke();
            Time.timeScale = 0;
        }
        else
        {
            GameResumed?.Invoke();
            Time.timeScale = 1;
        }
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void HandleWaveCleared()
    {
        WaveCleared?.Invoke();

        HighscoreManager.CompareAndSaveHighscore(CurrentWave);

        Debug.Log($"Wave {CurrentWave} cleared");
        
        /*
        if (CurrentWave == _enemyWaves.Count)
        {
            Time.timeScale = 0;
            GameFinished?.Invoke();
        }
        else
        {
            if (_waveInvokationRoutine == null)
                StartCoroutine(InvokeNextWave());
        }
        */
        

        // Get Data from last Wave
        int prevIndex = _enemyWaves[_enemyWaves.Count - 1].WaveIndex;
        int prevNumberOfEnemies = _enemyWaves[_enemyWaves.Count - 1].EntityCount;
        
        // Add 3 Entities every Wave
        List<GameObject> entitiesToSpawn = new List<GameObject>();
        for(int i = 0; i < prevNumberOfEnemies + 3; i++)
        {
            entitiesToSpawn.Add(_enemyPrefab);
        }
        // Add new Wave
        _enemyWaves.Add(new WaveInfo(prevIndex + 1, entitiesToSpawn.ToArray()));

        // Spawn new Wave
        if (_waveInvokationRoutine == null)
            StartCoroutine(InvokeNextWave());
    }

    private IEnumerator InvokeNextWave()
    {
        yield return new WaitForSeconds(_newWaveDelayTime);

        CurrentWave++;
        WaveCountChanged?.Invoke(CurrentWave);

        SpawnSystem.SpawnWaveEntities(_enemyWaves[CurrentWave - 1].Entities);

        _waveInvokationRoutine = null;
    }

    private void HandlePlayerDied(object obj)
    {
        GameFailed?.Invoke();
    }
}
