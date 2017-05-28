using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehaviour
{
    public Transform target;
    public float stoppingDist= 5f;
    public override Vector3 GetForce()
    {
        Vector3 force = Vector3.zero;
        if(target == null)
        {
            return force;
        }
        Vector3 desForce = target.position - owner.transform.position;
        desForce.y = 0;

        if(desForce.magnitude > stoppingDist)
        {
            desForce = desForce.normalized * waighting;
            force = desForce - owner.velocity;
        } 
        return force;
    }

     
}
