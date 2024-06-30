using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField]
    [Range(0f,1f)]
    public float time;

    public BezierCurve bezier;

    /*private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + bezier.startPoint, 0.3f);
        Gizmos.DrawSphere(transform.position + bezier.startDirectionPoint, 0.3f);
        Gizmos.DrawSphere(transform.position + bezier.endDirectionPoint, 0.3f);
        Gizmos.DrawSphere(transform.position + bezier.endPoint, 0.3f);

        Gizmos.DrawLine(transform.position + bezier.startPoint, transform.position + bezier.startDirectionPoint);
        Gizmos.DrawLine(transform.position + bezier.endDirectionPoint, transform.position +  bezier.endPoint);

        for (int i = 0; i < meshOutline.vertices.Length; i++)
        {
            Gizmos.DrawSphere(transform.position + bezier.ReturnPositionBasedOnPosition(time, meshOutline.vertices[i].point), 0.1f);
        }

        for (int i = 0; i < meshOutline.lineIndicies.Length; i += 2)
        {
            Gizmos.DrawLine(transform.position + bezier.ReturnPositionBasedOnPosition(time, meshOutline.vertices[meshOutline.lineIndicies[i]].point), transform.position + bezier.ReturnPositionBasedOnPosition(time, meshOutline.vertices[meshOutline.lineIndicies[i + 1]].point));
        }


        Gizmos.DrawSphere(transform.position + bezier.ReturnPosition(time), 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + bezier.ReturnPositionBasedOnPosition(time,Vector3.right), 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + bezier.ReturnPositionBasedOnPosition(time, Vector3.forward), 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + bezier.ReturnPositionBasedOnPosition(time, Vector3.up), 0.1f);
    }*/
}
