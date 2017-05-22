using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node parent;
    public bool walkable;
    public Vector3 position;
    public int gridX;
    public int gridZ;
    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool a_walkable, Vector3 a_position,int gridX, int gridZ)
    {
        this.walkable = a_walkable;
        this.position = a_position;
        this.gridX = gridX;
        this.gridZ = gridZ;
    }
}
