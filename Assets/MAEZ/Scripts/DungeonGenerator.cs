using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;  // Import for NavMeshSurface

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            // 0 - cannot spawn 1 - can spawn 2 - HAS to spawn

            if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }

            return 0;
        }
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;
    public NavMeshSurface navMeshSurface; // Add NavMeshSurface reference

    List<Cell> board;

    public GameObject DungeonRooms;
    private int PathCount = 0;
    private int RoomCount = 0;
    private int lastCell; // Variable to store the index of the last cell visited

    // Start is called before the first frame update
    void Start()
    {
        size = GameVariables.DungeonSize;

        PathCount = 0;
        RoomCount = 0;

        MazeGenerator();

        Debug.Log("PathCount: " + PathCount);
        Debug.Log("RoomCount: " + RoomCount);
        Debug.Log("Dungeon Size: " + size);
    }

    void ResetDungeon()
    {
        foreach (Transform child in DungeonRooms.transform)
        {
            Destroy(child.gameObject);
        }

        PathCount = 0;
        RoomCount = 0;

        board = null;
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[(i + j * size.x)];
                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    // Check if this is the last cell that was visited
                    bool isLastCell = (i + j * size.x) == lastCell;

                    if (isLastCell)
                    {
                        // Force the last room in the array to be placed
                        randomRoom = rooms.Length - 1; // Index of the last room in the array
                    }
                    else
                    {
                        // Otherwise, choose a room based on the rules
                        for (int k = 0; k < rooms.Length; k++)
                        {
                            int p = rooms[k].ProbabilityOfSpawning(i, j);

                            if (p == 2)
                            {
                                randomRoom = k;
                                break;
                            }
                            else if (p == 1)
                            {
                                availableRooms.Add(k);
                            }
                        }

                        if (randomRoom == -1)
                        {
                            if (availableRooms.Count > 0)
                            {
                                randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                            }
                            else
                            {
                                randomRoom = 0;
                            }
                        }
                    }

                    // Instantiate the room under DungeonRooms game object for cleaner hierarchy
                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, DungeonRooms.transform).GetComponent<RoomBehaviour>();
                    RoomCount++;

                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name += " " + i + "-" + j;
                }
            }
        }

        // Now that all rooms are generated, build the NavMesh
        BuildDungeonNavMesh();
    }

    void BuildDungeonNavMesh()
    {
        // This method builds the NavMesh dynamically after the dungeon is generated
        navMeshSurface.BuildNavMesh();
    }

    void MazeGenerator()
    {
        board = new List<Cell>();
        PathCount = 0;
        RoomCount = 0;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int k = 0;

        while (k < 100000)
        {
            k++;
            board[currentCell].visited = true;

            // Update the lastCell whenever a new cell is visited
            lastCell = currentCell;

            if (currentCell == board.Count - 1)
            {
                break;
            }

            // Check the cell's neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count <= 1)
            {
                if (path.Count <= 1)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                    PathCount++;
                }
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    // Down or right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    // Up or left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        // Check up neighbor
        if (cell - size.x >= 0 && !board[(cell - size.x)].visited)
        {
            neighbors.Add((cell - size.x));
        }

        // Check down neighbor
        if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
        {
            neighbors.Add((cell + size.x));
        }

        // Check right neighbor
        if ((cell + 1) % size.x != 0 && !board[(cell + 1)].visited)
        {
            neighbors.Add((cell + 1));
        }

        // Check left neighbor
        if (cell % size.x != 0 && !board[(cell - 1)].visited)
        {
            neighbors.Add((cell - 1));
        }

        return neighbors;
    }
}
