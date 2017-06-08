using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graph : MonoBehaviour
{
    public float nodeRad = 1f;
    public LayerMask unWalkableMask;
    public Node[,] nodes;
    private float nodeDiameter;
    private int gridSizeX, gridSizeZ;
    private Vector3 scale;
    private Vector3 halfScale;
    public List<Node> path;

    public Node GetNodeFromPosition(Vector3 position)
    {
        float percentX = (position.x + halfScale.x) / scale.x;
        float percentZ = (position.z + halfScale.z) / scale.z;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return nodes[x, z];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> naighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for(int z = -1; z <= 1; z++)
            {
                if(x == 0 && z == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;
                if(checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    naighbours.Add(nodes[checkX, checkZ]);
                }
            }
        }
        return naighbours;
    }


    void Start()
    {
        CreateGrid();
    }

    void Update()
    {
        CheckWakable();
    }

    void CheckWakable()
    {
        for(int x = 0; x < nodes.GetLength(0);x++)
        {
            for(int z = 0; z < nodes.GetLength(0);z++)
            {
                Node node = nodes[x, z];
                node.walkable = !Physics.CheckSphere(node.position, nodeRad, unWalkableMask);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale); 
        if(nodes != null)
        {
            for(int x =0; x < nodes.GetLength(0);x++)
            {
                for(int z = 0; z < nodes.GetLength(1);z++)
                {   
                    Node node = nodes[x, z];
                    Gizmos.color = node.walkable ? new Color(0, 0, 1, 0.5f) : new Color(1, 0, 0, 0.5f);
                    if(path != null && path.Contains(node))
                    {
                        Gizmos.color = Color.yellow;
                    }
                    Gizmos.DrawSphere(node.position, nodeRad);
                }   
            }
        }
    }

    public void CreateGrid()
    {
        nodeDiameter = nodeRad * 2f;
        scale = transform.localScale;
        halfScale = scale / 2;
        gridSizeX = Mathf.RoundToInt(halfScale.x / nodeRad);
        gridSizeZ = Mathf.RoundToInt(halfScale.z / nodeRad);
        nodes = new Node[gridSizeX, gridSizeZ];
        Vector3 bottomLeft = transform.position - Vector3.right * halfScale.x - Vector3.forward * halfScale.z;
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                float xOffset = x * nodeDiameter + nodeRad;
                float zOffset = z * nodeDiameter + nodeRad;
                Vector3 nodePoint = bottomLeft + Vector3.right * xOffset + Vector3.forward * zOffset;
                bool walkable = !Physics.CheckSphere(nodePoint, nodeRad, unWalkableMask);
                nodes[x, z] = new Node(walkable, nodePoint,x,z);

            }
        }
    }
}
