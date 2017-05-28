using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AiAgent))]
public class SteeringBehaviour : MonoBehaviour
{
    public float waighting = 7.5f;
    [HideInInspector] public AiAgent owner;
	void Awake ()
    {
        owner = GetComponent<AiAgent>();
	}
	public virtual Vector3 GetForce()
    {
        return Vector3.zero;
    }
}
