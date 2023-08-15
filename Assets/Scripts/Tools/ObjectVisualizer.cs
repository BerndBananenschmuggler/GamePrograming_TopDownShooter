using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectVisualizer : MonoBehaviour
{
    public float Radius = 1f;
    public Color Color = Color.yellow;


    private void OnDrawGizmos()
    {
        if (Handles.color != Color)
            Handles.color = Color;

        Handles.DrawSolidDisc(transform.position, transform.up, Radius);
    }
}
