using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _cameraOffset = new Vector3(0,15,0);
    private Quaternion _cameraRotation = Quaternion.Euler(90, 0, 0);
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _cameraOriginTransform;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        if (_playerTransform == null)
            Debug.LogWarning("PlayerTransform is missing");
        if (_cameraOriginTransform == null)
            Debug.LogWarning("CameraOriginTransform is missing");
    }

    private void Start()
    {
        _cameraOriginTransform.rotation = _cameraRotation;
        UpdateCameraPosition();
    }

    private void OnEnable()
    {
        _playerMovement = _playerTransform.GetComponent<PlayerMovement>();
        _playerMovement.OnMoved += HandlePlayerMoved;
    }

    private void OnDisable()
    {
        _playerMovement.OnMoved -= HandlePlayerMoved;
    }

    private void HandlePlayerMoved(Vector3 moveDir)
    {
        UpdateCameraPosition();
    }    

    private void UpdateCameraPosition()
    {
        _cameraOriginTransform.position = _playerTransform.position + _cameraOffset;
    }
}
