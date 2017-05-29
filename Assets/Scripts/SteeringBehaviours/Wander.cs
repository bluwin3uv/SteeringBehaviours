using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGL;

public class Wander : SteeringBehaviour
{

    public float offset = 1f;
    public float rad = 1;
    public float jitter = 0.2f;

    private Vector3 targetDir;
    private Vector3 randomDir;
    private Vector3 circlePos;
    private Vector3 desForce;

    public override Vector3 GetForce()
    {
        Vector3 force = Vector3.zero;
        float roundX = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);
        float roundZ = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);

        #region caculate randomDir
        // set randomDir to new vector3 x = randx and z = randz
        randomDir = new Vector3(roundX, 0,roundZ);
        // set randomDir to normalised randomDir
        randomDir = randomDir.normalized;
        // set randomDir to randomDir x jitter
        randomDir = randomDir * jitter;
        #endregion

        #region caculate target dir
        //set targetDir to targetDir + randomDir
        targetDir = targetDir + randomDir;
        //set targetDir to nomalise targetDir
        targetDir = targetDir.normalized;
        //set targetDit to targetDir x radius
        targetDir = targetDir * rad;
        #endregion

        #region cauculate force
        // set seekpos to owner's pos + targetDir
        Vector3 seekPos = owner.transform.position + targetDir;
        // set seekPos to seekpos + owners fowerd x offset
        seekPos = seekPos + owner.transform.forward * offset;
        #region GISMO
        Vector3 offsetPos = transform.position + transform.forward.normalized * offset;
        GizmosGL.AddCircle(offsetPos + Vector3.up * 0.1f, rad, Quaternion.LookRotation(Vector3.down), 16, Color.red);
        GizmosGL.AddCircle(seekPos + Vector3.up * 0.15f, rad * 0.6f, Quaternion.LookRotation(Vector3.down), 16, Color.blue);
        #endregion

        // set desForce to seekpos - pos
        desForce = seekPos - transform.position;
        // if decForce not zero
        if(desForce != Vector3.zero)
        {
            // set desForce to desforce nomalised x weiting
            desForce = desForce.normalized * waighting;
            // set force to desForce - owners velo
            force = desForce - owner.velocity;
        }

        #endregion
        return force;
    }

}
