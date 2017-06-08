using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGL;

public class AgentDirector : MonoBehaviour
{
    public Transform selectedTarget;
    public float rayDist = 1000f;
    public LayerMask selectionLayer;
    private AiAgent[] agents;
	
	void Start ()
    {
        agents = FindObjectsOfType<AiAgent>();
	}
	
	void Update ()
    {
        CheckSelection();
	}

    void ApplySelection()
    {
        agents = FindObjectsOfType<AiAgent>();
        foreach (AiAgent agent in agents)
        {
            PathFollowing pathFollowing = agent.GetComponent<PathFollowing>();
            if(pathFollowing != null)
            {
                pathFollowing.target = selectedTarget;
                pathFollowing.UpdatePath();
            }
        }
    }

    void CheckSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray,out hit, rayDist,selectionLayer))
        {
            GizmosGL.AddSphere(hit.point, 5f, Quaternion.identity, Color.red);
            if(Input.GetMouseButtonDown(0))
            {
                selectedTarget = hit.collider.transform;
                ApplySelection();
            }
        }
    }
}
