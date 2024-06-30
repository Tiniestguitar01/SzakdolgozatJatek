using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    public Transform Target;



    void FixedUpdate()
    {
        transform.LookAt(new Vector3(Target.position.x, Target.position.y + 2f, Target.position.z + 2f));
        transform.position = Vector3.Lerp(transform.position, Target.position + new Vector3(-Target.forward.x, 1f, -Target.forward.z) * 10f, moveSpeed * Time.deltaTime);
    }
}
