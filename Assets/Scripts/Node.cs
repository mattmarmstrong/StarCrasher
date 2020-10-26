using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Values for A* search
    private int hCost;  // Distance from this node to goal
    private int gCost;  // Distance from start to this node
    private int fCost;  // hCost + gCost
    public Vector3Int position;
    private Node parent;

    public Node(Vector3Int nodePosition)
    {
        position = nodePosition;
    }

    /*
     * Set the new H cost, automatically update F cost.
     */
    public void setHCost(int cost)
    {
        hCost = cost;
        fCost = gCost + hCost;
    }

    /*
     * Set the new G cost, automatically update F cost.
     */
    public void setGCost(int cost)
    {
        gCost = cost;
        fCost = gCost + hCost;
    }

    public int getHCost() { return hCost; }

    public int getGCost() { return gCost; }

    public int getFCost() { return fCost; }

    /*
     * If the given node has a lower F cost than the current parent, set
     * it to be the new parent.
     */
    public void setParent(Node newParent)
    {
        if ( (parent is null) || (newParent.getFCost() < parent.getFCost()))
        {
            parent = newParent;
        }
            
    }

    public void setPosition(Vector3Int newPosition) { position = newPosition; }

    public Node getParent() { return parent; }

    public Vector3Int getPosition() { return position; }

}
