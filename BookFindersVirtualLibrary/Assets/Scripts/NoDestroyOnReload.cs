using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestroyOnReload : MonoBehaviour
{
    private GameObject[] music;

    void Start()
    {
        music = GameObject.FindGameObjectsWithTag("gameMusic");
        Destroy(music[1]);
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
