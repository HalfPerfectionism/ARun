using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float smoothing = 0.1f;
    public Transform maxPos;
    public Transform minPos;

    private void FixedUpdate()
    {
        if(Target != null)
        {
            //Vector3 targetPos = Target.position;
            //targetPos.z = -1;
            //if (transform.position != targetPos)
            //{
            //    transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            //}


        }
    }
}
