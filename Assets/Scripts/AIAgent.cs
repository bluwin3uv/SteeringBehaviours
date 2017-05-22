using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public Transform target;
    public float speed = 20f;
    public float stoppingDist = 0;
    private graph grap;
    private float remaningDistance = 0;
    private List<Node> path;

	void Start ()
    {
        grap = FindObjectOfType<graph>();
        if (grap == null)
        {
            Debug.LogError("ERROR: No generated graph found");
            Debug.Break();
        }	
	}
	
	void Update ()
    {
        remaningDistance = Vector3.Distance(transform.position, target.position);
        if(remaningDistance >= stoppingDist)
        {
            path = RunAstar(transform.position, target.position);
            if(path.Count > 0)
            {
                grap.path = path;
                Node current = path[0];
                transform.position = Vector3.MoveTowards(transform.position, current.position, speed * Time.deltaTime);
            }
        }
	}

    public List<Node> RunAstar(Vector3 startPos, Vector3 targetPos)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> clostedList = new HashSet<Node>();
        Node startNode = grap.GetNodeFromPosition(startPos);
        Node targetNode = grap.GetNodeFromPosition(targetPos);
        openList.Add(startNode);
        while(openList.Count > 0)
        {
            Node currentNode = FindShortestNode(openList);
            openList.Remove(currentNode);
            clostedList.Add(currentNode);
            if(currentNode == targetNode)
            {
                path = RetracePath(startNode, targetNode);
                return path;
            }
            foreach(Node neighbour in grap.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || clostedList.Contains(neighbour))
                {
                    continue;
                }
                int newCostToNeighbour = currentNode.gCost + GetHeuristic(currentNode, neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetHeuristic(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    if(!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
        return path;
    }
    Node FindShortestNode(List<Node> nodelist)
    {
        Node shortest = null;
        float minFCost = float.MaxValue;
        float minHcost = float.MaxValue;
        for(int i = 0; i < nodelist.Count; i++)
        {
            Node currentNode = nodelist[i];
            if (currentNode.fCost <= minFCost)
            { 
                if(currentNode.hCost <= minHcost)
                { 
                    minFCost = currentNode.fCost;
                    minHcost = currentNode.hCost;
                    shortest = currentNode;
                }
            }
        }
        return shortest; 
    }

    int GetHeuristic(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);
        if (dstX > dstZ)
        {
            return 14 * dstZ + 10 * (dstX - dstZ);
        }
        return 14 * dstX + 10 * (dstZ - dstX);
    }

    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;
        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;        
    }
}
