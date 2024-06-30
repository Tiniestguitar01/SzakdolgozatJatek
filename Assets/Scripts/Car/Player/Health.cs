using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    int health = 3;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            health--;
            if (health == 0)
            {
                //gameover
                Debug.Log("gameover wewrawrasfawtdgvsefg");
            }
        }
    }
}
