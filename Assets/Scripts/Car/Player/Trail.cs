using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    Movement movement;

    public List<TrailRenderer> trails;

    void Start()
    {
        movement = GetComponent<Movement>();
    }
    void Update()
    {
        if(movement.GetIsTurning() || movement.GetIsStopping())
        {
            foreach(TrailRenderer trail in trails)
            {
                trail.emitting = true;
            }
        }
        else
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = false;
            }
        }
    }
}
