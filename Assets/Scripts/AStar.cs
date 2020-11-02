using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

public class AStar : MonoBehaviour
{
    // Tilemaps used for checking if areas are walkable
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;

    // Tiles used for colouring the ground for debugging and path display
    public Tile neighbourTile;
    public Tile curNodeTile;
    public Tile pathTile;
    public Tile groundTile;

    // Node data structures used in the A* search
    public static HashSet<Node> closedList;
    public static HashSet<Node> openList;
    public static Dictionary<Vector3Int, Node> nodePositions;

    public Chunk chunk;

    // Values for A* search
    public Node prevStartNode;
    public Node startNode;
    public Node endNode;
    public Vector3Int startPosition;
    public Vector3Int goalPosition;


    /*
     * Initialize the data structures for the A* search.
     * Save the start and goal positions and the start node.
     */
    private void initializePathfinding(Vector3Int startPos, Vector3Int goalPos, Chunk ch)
    {
        // Initialize open and closed lists
        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();

        // Initialize dictionary mapping positions to nodes
        nodePositions = new Dictionary<Vector3Int, Node>();

        // Save start and goal positions
        startPosition = startPos;
        goalPosition = goalPos;

        chunk = ch;

        // Get/create the starting node and set its costs to 0 and parent to null
        startNode = getNode(startPos);
        //Assert.IsNotNull(startNode, "startNode is null after initialization.");
        if (startNode is null)
        {
            Debug.Log("StartNode was null");
            startNode = prevStartNode;
        }
            
        else
            prevStartNode = startNode;
        startNode.setGCost(0);
        startNode.setHCost(0);
        startNode.setParent(null);
        Assert.IsNotNull(startNode, "Error: Cannot start pathfinding from an unwalkable position.");

        // Add start node to the open list
        openList.Add(startNode);
    }


    /*
     * Perform the A* search for the shortest path between startPos and goalPos.
     * Return a stack containing position coordinates for each step along the
     * shortest path with the first step being on the top.
     */
    public Stack<Vector3Int> aStarGetPath(Vector3Int startPos, Vector3Int goalPos, Chunk chunk)
    {
        // Initialize open and closed lists, add start node to open list
        initializePathfinding(startPos, goalPos, chunk);

        while (openList.Count > 0)
        {
            Node curNode = chooseBestNode();
            Assert.IsNotNull(curNode, "Error: chooseBestNode() returned null");

            // Get a list of neighbours
            List<Node> neighbours = getNeighbours(curNode);
            foreach(Node n in neighbours)
            {
                if (n.position == goalPos)
                {
                    n.setParent(curNode);
                    return savePath(n);
                }

                // Set the values of the neighbour nodes before adding to the open list
                n.setParent(curNode);
                setNodeCosts(n);
                openList.Add(n);
            }

            // Remove the current node from the open list and add to closed so it isn't used again
            openList.Remove(curNode);
            closedList.Add(curNode);
        }

        return null;
    }

    /*
     * Return the node from the open list with the lowest F cost.
     */
    private Node chooseBestNode()
    {
        Node bestNode = null;
        int minFCost = int.MaxValue;
        foreach (Node n in openList)
        {
            if (n.getFCost() < minFCost)
            {
                minFCost = n.getFCost();
                bestNode = n;
            }
        }
        return bestNode;
    }

    /*
     * Return a list of all nodes that neighbour the given node. Nodes in the 
     * closed list are ignored along with nodes from unwalkable positions.
     */
    private List<Node> getNeighbours(Node curNode)
    {
        List<Node> neighbours = new List<Node>();

        // Loop over the 8 grid positions surrounding the current node
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Don't add the current node to the neighbours list
                if (!(x == 0 && y == 0))// && !(Mathf.Abs(x) == Mathf.Abs(y)))
                {
                    Vector3Int neighbourPosition = new Vector3Int(curNode.position.x - x, curNode.position.y - y, curNode.position.z);
                    Node n = getNode(neighbourPosition);

                    // Do not include any node in the closed list as a neighbour
                    if (!closedList.Contains(n))
                    {
                        // Node will be null if the position is an unwalkable area
                        if (!(n is null))
                        {
                            neighbours.Add(n);
                            //groundTilemap.SetTile(neighbourPosition, neighbourTile);
                        }
                    }
                    
                }

            }
        }
        return neighbours;
    }

    /*
     * Set the G, H, and F costs of the given node.
     */
    private void setNodeCosts(Node curNode)
    {
        Node parent = curNode.getParent();
        int parentGCost;
        if (parent is null)
            parentGCost = 0;
        else
            parentGCost = parent.getGCost();

        // Set G cost based on position of curNode to parent node (diagonal or not)

        // If both x and y are different, the move is on a diagonal and costs 14 (1.4 * 10 to be an int)
        if ((curNode.position.x != parent.position.x) && (curNode.position.y != parent.position.y))
        {
            curNode.setGCost(parentGCost + 14); // diagonal steps cost 1.4 (~sqrt(1^2 + 1^2)*10) (multiply by 10 to use integers
        }
        else
        {
            curNode.setGCost(parentGCost + 10); // each step costs 1 (multiply by 10 to match diagonal case)
        }

        // Set H cost based on euclidean distance from the current node to the goal position (c^2 = a^2 + b^2)
        float distanceFromGoal = Mathf.Sqrt(Mathf.Pow(goalPosition.x - curNode.position.x, 2) + Mathf.Pow(goalPosition.y - curNode.position.y, 2));
        curNode.setHCost(Mathf.RoundToInt(distanceFromGoal));

        // F cost is calculated automatically in the Node class

    }

    /*
     * Return the shortest path saved in a stack given the final node in the path.
     */
    private Stack<Vector3Int> savePath(Node endNode)
    {
        Stack<Vector3Int> path = new Stack<Vector3Int>();

        //while (!(endNode is null) && endNode.position != startPosition)
        while (endNode.position != startPosition)
        {
            //Debug.Log("Path point (Chunk) = " + endNode.position);
            path.Push(endNode.position);
            endNode = endNode.getParent();
        }
        return path;
    }

    /*
     * Return the node for the given position.
     * If the node doesn't exist, create it and save it to the nodePositions dictionary.
     * If the position is unwalkable (a wall or out of bounds) do not create a node.
     */
    private Node getNode(Vector3Int position)
    {
        // If the position is a wall, return null because it is not walkable
        if (isWall(position))
            return null;

        // Return the node if it is in the dictionary, otherwise create it
        if (nodePositions.ContainsKey(position))
        {
            return nodePositions[position];
        }
        else
        {
            // Create a new node if it doesn't already exist
            Node newNode = new Node(position);
            //if (isWall(position))
            //    newNode.setWallCosts();
            nodePositions.Add(position, newNode);
            return newNode;
        }
    }

    private bool isWall(Vector3Int position)
    {
        int[,] wallMap = chunk.GetMapArray();

        return (wallMap[position.x, position.y] == 1) || (wallMap[position.x, position.y+1] == 1);

        //for (int x = -1; x <= 1; x++)
        //{
        //    for (int y = -1; y <= 1; y++)
        //    {
        //        try
        //        {
        //            if (wallMap[position.x + x, position.y + y] == 1)
        //                return true;
        //        }
        //        catch (System.IndexOutOfRangeException)
        //        {   // Return true if the position is not even on the map
        //            return true;
        //        }

        //    }
        //}
        //return false;
    }

    /*
     * Colour the given path on the groundTilemap.
     */
    public void colourPath(Stack<Vector3Int> path)
    {
        if (path is null)
            return;

        foreach (Vector3Int point in path)
        {
            groundTilemap.SetTile(point, pathTile);
        }
    }

    /*
     * Clear the colouring of the given path on the ground tilemap.
     */
    public void clearPath(Stack<Vector3Int> path)
    {
        if (path is null)
            return;

        foreach (Vector3Int point in path)
        {
            groundTilemap.SetTile(point, groundTile);
        }
    }
}
