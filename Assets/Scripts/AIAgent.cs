using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public Vector3 force;
    public Vector3 velocity;
    public float maxVelo;
    private SteeringBehaviour[] behaviours;
	void Start ()
    {
        behaviours = GetComponents<SteeringBehaviour>();
	}

	void Update ()
    {
        ComputeForces();
        ApplyVelocity();
	}

    void ComputeForces()
    {
        force = Vector3.zero;
        for(int i = 0; i < behaviours.Length;i++)
        {
            if(behaviours[i].enabled == false)
            {
                continue;
            }
            force = force + behaviours[i].GetForce();
            if(force.magnitude > maxVelo)
            {
                force = force.normalized * maxVelo;
                break;
            }
        }
    }

    void ApplyVelocity()
    {
        velocity = velocity + force * Time.deltaTime;
        if(velocity.magnitude > maxVelo)
        {
            velocity = velocity.normalized * maxVelo;
        }
        if(velocity.magnitude > 0)
        {
            transform.position = transform.position + velocity * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
