using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class CubeEditor : MonoBehaviour
{
    
    Vector3 gridPos;
    Waypoint waypoint;
    
    private void Start()
    {
        
    }
    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    void Update()
    {
        SnapToGrid();
        UpdatesLabel();

    }

    private void SnapToGrid()
    {
        int gridSize = waypoint.GetGridSize();
        transform.position = new Vector3(
            waypoint.GetGridPos().x * gridSize, 
            0f,
            waypoint.GetGridPos().y * gridSize
            );
    }

    private void UpdatesLabel()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        string labelText = 
            waypoint.GetGridPos().x + 
            "," + 
            waypoint.GetGridPos().y;
        textMesh.text = labelText;
        gameObject.name = labelText;
    }
}
