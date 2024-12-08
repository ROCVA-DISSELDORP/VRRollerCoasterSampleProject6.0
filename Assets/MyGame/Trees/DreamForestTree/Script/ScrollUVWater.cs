using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUVWater : MonoBehaviour 
{
    public Vector2 AnimateRate = new Vector2(0.0f, 0.0f);
    Vector2 UVOffs = Vector2.zero;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
	
	void Update () 
    {
        UVOffs += (AnimateRate * Time.deltaTime);
        meshRenderer.materials[0].SetTextureOffset("_BaseMap", UVOffs);
        meshRenderer.materials[0].SetTextureOffset("_MumpMap", UVOffs);			
	}
}
