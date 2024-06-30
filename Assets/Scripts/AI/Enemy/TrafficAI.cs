using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficAI : MonoBehaviour
{
    [SerializeField]
    Segment[] segments;

    [SerializeField]
    int currentSegment = 0;
    [SerializeField]
    float currentTime = 0f;

    public float speed = 0.2f;
    [SerializeField]
    Vector3 offset;

    void Start()
    {
        segments = GameObject.Find("Segments").transform.GetComponentsInChildren<Segment>();

        currentSegment = segments.Length - 1;
        currentTime = 1f;

        offset = Vector3.right * Random.Range(-2f, 2f);
        speed = Random.Range(0.1f, 0.4f);
    }

    void Update()
    {

        if (currentTime <= 0f)
        {
            currentTime = 1f;
            currentSegment--;
            if(currentSegment < 0)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            currentTime -= Time.deltaTime * speed;
        }

        if(segments[currentSegment] !=null)
        {
            transform.position = segments[currentSegment].transform.position + segments[currentSegment].bezier.ReturnPositionBasedOnPosition(currentTime, offset);
            transform.eulerAngles = segments[currentSegment].bezier.GetRotation(currentTime).eulerAngles + new Vector3(0f, 180f, 0f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
