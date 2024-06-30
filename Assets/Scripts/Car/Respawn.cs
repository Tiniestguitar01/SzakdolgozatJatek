using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform Player;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Player.position = GenerateRoad.Instance.segments[GenerateRoad.Instance.segments.Count / 2].transform.position;
            Player.rotation = GenerateRoad.Instance.segments[GenerateRoad.Instance.segments.Count / 2].bezier.GetRotation(0f);
        }
    }
}
