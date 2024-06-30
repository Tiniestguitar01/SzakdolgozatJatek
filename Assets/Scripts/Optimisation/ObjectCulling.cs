using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCulling : MonoBehaviour
{
    public float cullDistance = 400f;

    public List<GameObject> props;
    Transform player;
    Transform camera;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }
    void Update()
    {
        props = GenerateRoad.Instance.instancedProps;

        Vector3 center = player.position + camera.forward * (cullDistance/2.7f);
        for(int i = 0; i < props.Count; i++)
        {
            Vector3 propPos = props[i].transform.position;
            float distance = Mathf.Sqrt(Mathf.Pow(center.x - propPos.x, 2) + Mathf.Pow(center.y - propPos.y, 2) + Mathf.Pow(center.z - propPos.z, 2));
            props[i].active = distance < cullDistance;
        }
    }
}
