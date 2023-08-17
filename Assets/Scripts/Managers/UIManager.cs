using Assets.Scripts.Player;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentWaveText;
    [SerializeField] private TextMeshProUGUI _remainingEnemiesText;
    [SerializeField] private TextMeshProUGUI _totalAmmoText;
    [SerializeField] private TextMeshProUGUI _magAmmoText;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private TextMeshProUGUI _gameOverMenuHeaderText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TextMeshProUGUI _reloadIndicatorText;

    [SerializeField] private GameObject _waveClearedInfoContainer;
    
    private PlayerAttack _playerAttack;
    private RangeWeapon _activeWeapon;

    private void Start()
    {
        _playerAttack = PlayerController.Instance.gameObject.GetComponent<PlayerAttack>();

        // Null Check
        if (_playerAttack == null)
            Debug.LogWarning("PlayerAttack is missing");
        if(_remainingEnemiesText == null)
            Debug.LogWarning("RemainingEnemiesText is missing");
        if(_currentWaveText == null)
            Debug.LogWarning("CurrentWaveText is missing");
        if (_totalAmmoText == null)
            Debug.LogWarning("TotalAmmoText is missing");
        if (_magAmmoText == null)
            Debug.LogWarning("MagAmmoText is missing");
        if (_pauseMenu == null)
            Debug.LogWarning("PauseMenu is missing");
        if (_gameOverMenu == null)
            Debug.LogWarning("GameOverMenu is missing");
        if (_gameOverMenuHeaderText == null)
            Debug.LogWarning("GameOverMenuHeaderText is missing");
        if (_waveClearedInfoContainer == null)
            Debug.LogWarning("WaveClearedInfoContainer is missing");
        if (_highscoreText == null)
            Debug.LogWarning("HighscoreText is missing");
        if (_scoreText == null)
            Debug.LogWarning("ScoreText is missing");
        if (_reloadIndicatorText == null)
            Debug.LogWarning("ReloadIndicatorText is missing");

        // Get Active Weapon
        _activeWeapon = _playerAttack.GetEquippedWeapon();

        // Sub Attack event and with UIUpdate
        _playerAttack.Attacked += HandlePlayerAmmoChanged;
        _playerAttack.Reloaded += HandlePlayerAmmoChanged;
        _playerAttack.ReloadStarted += HandlePlayerReloadStarted;

        // Initial Update
        UpdateAmmoDisplay();

        GameManager.Instance.SpawnSystem.EnemyCountChanged += HandleEnemyCountChanged;
        GameManager.Instance.WaveCountChanged += HandleWaveCountChanged;
        GameManager.Instance.WaveCleared += HandleWaveCleared;
        GameManager.Instance.GamePaused += HandleGamePaused;
        GameManager.Instance.GameResumed += HandleGameResumed;
        GameManager.Instance.GameStarted += HandleGameStarted;
        GameManager.Instance.GameFailed += HandleGameFailed;
    }

    private void HandleWaveCleared()
    {
        _waveClearedInfoContainer.SetActive(true);
    }

    private void HandleGameStarted()
    {
        Debug.Log("Game started");
        _pauseMenu.SetActive(false);
        _gameOverMenu.SetActive(false);
        _waveClearedInfoContainer.SetActive(false);
    }

    private void HandleGameFailed()
    {
        _gameOverMenu.SetActive(true);
        _gameOverMenuHeaderText.text = "Game Over";
        _scoreText.text = $"Your Score: {GameManager.Instance.CurrentWave.ToString()}";
        _highscoreText.text = $"Your Highscore: {HighscoreManager.GetHighscore().ToString()}";
    }

    private void HandleGameResumed()
    {
        _pauseMenu.SetActive(false);
    }

    private void HandleGamePaused()
    {
        _pauseMenu?.SetActive(true);
    }

    private void HandleWaveCountChanged(int count)
    {
        _currentWaveText.text = $"Wave {count}";
        _waveClearedInfoContainer.SetActive(false);
    }

    private void HandleEnemyCountChanged(int count)
    {
        _remainingEnemiesText.text = $"{count} Enemies";
    }

    private void HandlePlayerAmmoChanged()
    {
        if (_reloadIndicatorText.gameObject.activeSelf)
            _reloadIndicatorText.gameObject.SetActive(false);

        UpdateAmmoDisplay();
    }

    private void HandlePlayerReloadStarted()
    {
        _reloadIndicatorText.gameObject.SetActive(true);
    }

    private void UpdateAmmoDisplay()
    {
        _totalAmmoText.text = $"Total Ammo: {_activeWeapon.CurrentAmmo} / {_activeWeapon.MaxAmmo}";
        _magAmmoText.text = $"Current Mag: {_activeWeapon.CurrentMagFill} / {_activeWeapon.MaxMagSize}";
    }
}
