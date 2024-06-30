using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public float time = 5f;

    void Start()
    {
        StartCoroutine("Spawn");
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(time);
        Instantiate(enemy);
        StartCoroutine("Spawn");
    }
}
