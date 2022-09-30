using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Waypoint startWaypoint, endWaypoint;
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    bool isRunning = true; // todo make private
    Waypoint searchCenter; // current searchCenter
    List<Waypoint> path = new List<Waypoint>(); 

    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
    };

    public List<Waypoint> GetPath()
	{
        if (path.Count == 0)
		{
			CalculatePath();
		}
		return path;

    }

	private void CalculatePath()
	{
		LoadBlocks();
		ColorStartAndEnd();
		BreadthFirstSearch();
		CreatePath();
	}

	// Start is called before the first frame update
	void Start() // todo maybe this happend when we ask for the path?
    {
        
    }

	private void CreatePath()
	{
        SetAsPath(endWaypoint);
		

        Waypoint previous = endWaypoint.exploredFrom;
        while (previous != startWaypoint)
		{
            SetAsPath(previous);
            previous = previous.exploredFrom;
            
        }

        SetAsPath(startWaypoint);
        path.Reverse();
	}

    private void SetAsPath(Waypoint waypoint)
	{
        path.Add(waypoint);
        waypoint.isPlaceable = false;
    }

	private void BreadthFirstSearch()
	{
		queue.Enqueue(startWaypoint);

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbours();
            searchCenter.isExplored = true;
        }
        
	}

	private void HaltIfEndFound()
	{
        if (searchCenter == endWaypoint)
        {
            isRunning = false;
        }
	}

	private void ExploreNeighbours()
    {
        if (!isRunning) { return;  }
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + direction;
            if (grid.ContainsKey(neighbourCoordinates))
			{
				QueueNewNeighbours(neighbourCoordinates);
			}
	
            
        }
    }

	private void QueueNewNeighbours(Vector2Int neighbourCoordinates)
	{
		Waypoint neighbour = grid[neighbourCoordinates];
        if (neighbour.isExplored || queue.Contains(neighbour))
		{

		}
		else
		{
            
            queue.Enqueue(neighbour);
            neighbour.exploredFrom = searchCenter;
        }
		
	}

	private void ColorStartAndEnd()
    {
        // todo consider moving out
       startWaypoint.SetTopColor(Color.green);
       endWaypoint.SetTopColor(Color.red);
    }

    private void LoadBlocks()
    {
        //var waypoints = FindObjectsOfType<Waypoint>();
        var waypoints = GetComponentsInChildren<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            var gridPos = waypoint.GetGridPos();
            bool isOverlapping = grid.ContainsKey(gridPos);
            if (isOverlapping)
            {
                Debug.LogWarning("Skipping overlapping block " + waypoint);
            }
            else
            {
                grid.Add(gridPos, waypoint);
                
            }
            
        }
        print("Loaded " + grid.Count + " blocks");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
