using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorScaled : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    [SerializeField]
    private Transform floorPrefab;

    [SerializeField]
    private float cellScale = 5f; // Scale factor for the maze cells

    [SerializeField]
    private GameObject entrancePrefab; // Assign the entrance prefab in the Inspector

    [SerializeField]
    private GameObject exitPrefab; // Assign the exit prefab in the Inspector

    private MazeCell[,] _mazeGrid;

    void Start()
    {
        Debug.Log($"Starting maze generation: {_mazeWidth}x{_mazeDepth} with cell scale {cellScale}");
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // Adjust floor size and position
        Transform floor = Instantiate(floorPrefab, new Vector3((_mazeWidth - 1) * cellScale / 2f, 0, (_mazeDepth - 1) * cellScale / 2f), Quaternion.identity, transform);
        floor.localScale = new Vector3(_mazeWidth * cellScale, 1, _mazeDepth * cellScale);

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                Vector3 position = new Vector3(x * cellScale, 0, z * cellScale);
                MazeCell cell = Instantiate(_mazeCellPrefab, position, Quaternion.identity, transform);

                // Adjust the cell's scale
                cell.transform.localScale = Vector3.one * cellScale;

                _mazeGrid[x, z] = cell;
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);
        CreateEntranceandExit();
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();

        if (previousCell != null)
        {
            ClearWalls(previousCell, currentCell);
        }

        while (true)
        {
            MazeCell nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
            else
            {
                break;
            }
        }
    }

    private void CreateEntranceandExit()
    {
        Debug.Log("Placing entrance and exit...");

        // Get the bottom-left corner cell (0, 0)
        MazeCell entranceCell = _mazeGrid[0, 0];
        // Calculate the position for the entrance prefab
        Vector3 entrancePosition = entranceCell.transform.position;
        // Instantiate the entrance prefab and scale it
        GameObject entranceInstance = Instantiate(entrancePrefab, entrancePosition, Quaternion.identity);
        entranceInstance.transform.localScale = new Vector3(cellScale, cellScale, cellScale); // Match the cell scale

        // Get the top-right corner cell
        MazeCell exitCell = _mazeGrid[_mazeWidth - 1, _mazeDepth - 1];

        // Calculate the position for the exit prefab
        Vector3 exitPosition = exitCell.transform.position;

        // Instantiate the exit prefab and scale it
        GameObject exitInstance = Instantiate(exitPrefab, exitPosition, Quaternion.identity);
        exitInstance.transform.localScale = new Vector3(cellScale, cellScale, cellScale); // Match the cell scale

        Debug.Log("Exit placed at: " + exitPosition);
        Debug.Log("Entrance placed at: " + entrancePosition);

        Debug.Log("Entrance and Exit created.");
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        List<MazeCell> unvisited = new List<MazeCell>();

        int x = (int)(currentCell.transform.position.x / cellScale);
        int z = (int)(currentCell.transform.position.z / cellScale);

        // Check neighboring cells
        if (x > 0 && !_mazeGrid[x - 1, z].IsVisited) unvisited.Add(_mazeGrid[x - 1, z]);
        if (x < _mazeWidth - 1 && !_mazeGrid[x + 1, z].IsVisited) unvisited.Add(_mazeGrid[x + 1, z]);
        if (z > 0 && !_mazeGrid[x, z - 1].IsVisited) unvisited.Add(_mazeGrid[x, z - 1]);
        if (z < _mazeDepth - 1 && !_mazeGrid[x, z + 1].IsVisited) unvisited.Add(_mazeGrid[x, z + 1]);

        if (unvisited.Count == 0) return null;

        return unvisited[Random.Range(0, unvisited.Count)];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        Vector3 prevPos = previousCell.transform.position;
        Vector3 currPos = currentCell.transform.position;

        if (prevPos.x < currPos.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }
        else if (prevPos.x > currPos.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }
        else if (prevPos.z < currPos.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }
        else if (prevPos.z > currPos.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }
}
