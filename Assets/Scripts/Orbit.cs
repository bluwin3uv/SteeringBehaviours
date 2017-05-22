using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

    public Transform target;
    public float orthoSize = 5f;
    public float dist = 5f;
    public float zoomSpeed = 5f;
    public float xSpeed = 120f;
    public float ySpeed = 120f;
    public float yMinLimit = 0;
    public float yMaxLimit = 80f;
    public float minDist = 5f;
    public float maxDist = 20f;
    public float minOrthoSize = 5f;
    public float maxOrthoSize = 20;

    private float x = 0;
    private float y = 0;


	void Start ()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;
	}
	
	void LateUpdate ()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
	    if(Camera.main.orthographic)
        {
            orthoSize = Mathf.Clamp(orthoSize -scroll,minOrthoSize,minOrthoSize);
        }
        else
        {
            dist = Mathf.Clamp(dist - scroll, minDist, maxDist);
        }
        if (target != null && Input.GetMouseButton(1))
        {
            float mouseX;
            float mouseY;
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            x += mouseX * xSpeed * Time.deltaTime;
            y += -mouseY * ySpeed * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 negDist = new Vector3(0, 0, -dist);
            Vector3 pos = rotation * negDist + target.position;
            transform.rotation = rotation;
            transform.position = pos;

        }
	}
}
