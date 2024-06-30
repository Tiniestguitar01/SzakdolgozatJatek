using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text respawnText;

    public TMP_Text FPSText;

    void Start()
    {
        Instance = this;
        StartCoroutine("FPS");
    }

    public IEnumerator FPS()
    {
        FPSText.text = "FPS: " + Mathf.RoundToInt(1f / Time.deltaTime);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("FPS");
    }

    public UIManager GetInstance()
    {
        return Instance;
    }
}
