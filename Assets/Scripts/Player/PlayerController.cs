using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main-Usage: Singleton to find the Player
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("More than 1 Player exists!");
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
