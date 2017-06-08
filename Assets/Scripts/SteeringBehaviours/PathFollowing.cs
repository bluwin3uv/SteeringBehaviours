using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGL;

public class PathFollowing : SteeringBehaviour
{
    public Transform target;
    public float nodeRadius = 5f;
    public float targetRadius = 3f;
    private Graph grap;
    private int currentNode = 0;
    private bool isAtTarget = false;
    private List<Node> path;

	void Start ()
    {
        grap = FindObjectOfType<Graph>();
        if (grap == null)
        {
            Debug.LogError("Error");
            Debug.Break(); 
        }
	}

    public void UpdatePath()
    {
        path = grap.FindPath(transform.position, target.position);
        currentNode = 0;
    }

    #region SEEK
    Vector3 Seek (Vector3 target)
    {
        Vector3 force = Vector3.zero;
        Vector3 desiredForce = target - transform.position;
        desiredForce.y = 0;
        float distance = 0;
        if(isAtTarget)
        {
            distance = targetRadius;           
        }
        else
        {
            distance = nodeRadius;
        }

        distance = isAtTarget == true ? targetRadius : nodeRadius;
        if(desiredForce.magnitude > distance)
        {
            desiredForce = desiredForce.normalized * waighting;
            force = desiredForce - owner.velocity;
        }
        return force;
    }
    #endregion
    #region GETFORCE
    public override Vector3 GetForce()
    {
        Vector3 force = Vector3.zero;
        if(path != null && path.Count > 0)
        {
            Vector3 currentPosition = path[currentNode].position;
            if(Vector3.Distance(transform.position,currentPosition) <= nodeRadius)
            {
                currentNode++;
                if(currentNode <= path.Count)
                {
                    currentNode = path.Count - 1;
                }
            }
            force = Seek(currentPosition);
            #region GISMO
            Vector3 previousPosition = path[0].position;
            foreach (Node node in path)
            {
                GizmosGL.AddSphere(node.position, grap.nodeRadius, Quaternion.identity, Color.blue);
                GizmosGL.AddLine(previousPosition, node.position, 0.01f, 0.01f, Color.yellow);
                previousPosition = node.position;
            }
            #endregion
        }

        return force;
    }
    #endregion

}
