using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderQueue : MonoBehaviour
{
    public int renderQueue = 3000;

    void Awake()
    {
        GetComponent<Renderer>().material.renderQueue = renderQueue;
    }
}
