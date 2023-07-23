using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("More than 1 Player exists!");
    }
}
