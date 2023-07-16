using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CustomColliderCreator), true)]
public class CustomColliderEditor : Editor
{
    private SerializedProperty _colliderShapeProp;
    private SerializedProperty _colliderSizeProp;
    private SerializedProperty _colliderRadiusProp;
    private SerializedProperty _colliderHeightProp;
    private SerializedProperty _colliderOffsetProp;
    private SerializedProperty _isColliderActiveProp;

    // Derived Class Values
    private SerializedProperty _playerHealthProp;
    private SerializedProperty _enemyHealthProp;
    private SerializedProperty _hitableLayersProp;

    private void OnEnable()
    {
        // Get Properties of SerializedObject
        // Base Class Values
        _colliderShapeProp = serializedObject.FindProperty("_colliderShape");
        _colliderSizeProp = serializedObject.FindProperty("_colliderSize");
        _colliderRadiusProp = serializedObject.FindProperty("_colliderRadius");
        _colliderHeightProp = serializedObject.FindProperty("_colliderHeight");
        _colliderOffsetProp = serializedObject.FindProperty("_colliderOffset");
        _isColliderActiveProp = serializedObject.FindProperty("_isColliderActive");

        // Derived Class Values
        _playerHealthProp = serializedObject.FindProperty("_playerHealth");
        _enemyHealthProp = serializedObject.FindProperty("_enemyHealth");
        _hitableLayersProp = serializedObject.FindProperty("_hitableLayers");
    }

    public override void OnInspectorGUI()
    {
        // Update UI
        serializedObject.Update();

        // Check for UI Changes
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_colliderShapeProp);
        if (EditorGUI.EndChangeCheck())
        {
            // Apply Changes from Editor
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // Get ColliderShape
        CustomColliderCreator.ColliderShapes colliderShape = (CustomColliderCreator.ColliderShapes)_colliderShapeProp.enumValueIndex;

        // Display value fields for associated ColliderShape 
        switch (colliderShape)
        {
            case CustomColliderCreator.ColliderShapes.Box:
                EditorGUILayout.PropertyField(_colliderSizeProp);
                EditorGUILayout.PropertyField(_colliderOffsetProp);
                break;
            case CustomColliderCreator.ColliderShapes.Sphere:
                EditorGUILayout.PropertyField(_colliderRadiusProp);
                EditorGUILayout.PropertyField(_colliderOffsetProp);
                break;
            case CustomColliderCreator.ColliderShapes.Capsule:
                EditorGUILayout.PropertyField(_colliderHeightProp);
                EditorGUILayout.PropertyField(_colliderRadiusProp);
                EditorGUILayout.PropertyField(_colliderOffsetProp);
                break;
        }

        // Display IsColliderActive
        EditorGUILayout.PropertyField(_isColliderActiveProp);

        // Display derived class values if possible
        if (_playerHealthProp != null)
            EditorGUILayout.PropertyField(_playerHealthProp);
        if (_enemyHealthProp != null)
            EditorGUILayout.PropertyField(_enemyHealthProp);
        if (_hitableLayersProp != null)
            EditorGUILayout.PropertyField(_hitableLayersProp);

        //DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }
}
