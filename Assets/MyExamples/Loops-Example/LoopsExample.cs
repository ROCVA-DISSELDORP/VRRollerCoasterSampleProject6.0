using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

public class LoopsExample : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 noisePos;
    public float noiseScale = 1.0f; 
    public int width = 10;
    public int height = 10;
    public int depth = 10;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        // Empty for loop

        // for (;;)
        // {

        // }

        // Loop ten times, counting up from 0 to 9
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(i);
        }

        // Loop ten times, counting down from 10 to 1
        for (int i = 10; i > 0; i--)
        {
            Debug.Log(i);
        }


        // Nested for loop. Counting ten times to ten. (think two dimensional square grids stuff...)
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Debug.Log(x + "," + y);
            }
        }

        // // Nested for loop. Counting ten times to ten. (think two dimensional square grids stuff...)
        // for (int y = 0; y < 10; y++)
        // {
        //     for (int x = 0; x < 10; x++)
        //     {
        //         GameObject spawnedPrefab = Instantiate(prefab);
        //         spawnedPrefab.transform.position = new Vector3(x, 0, y);
                
        //         yield return new WaitForSeconds(0.25f);
        //     }
        // }

        // // Nested for loop. Counting ten times, ten times, to ten. (think three dimensional voxel grids stuff, like minecraft...)
        // for (int z = 0; z < 10; z++)
        // {
        //     for (int y = 0; y < 10; y++)
        //     {
        //         for (int x = 0; x < 10; x++)
        //         {
        //             GameObject spawnedPrefab = Instantiate(prefab);
        //             spawnedPrefab.transform.position = new Vector3(x, y, z);
                    
        //             yield return new WaitForSeconds(0.1f);
        //         }
        //     }
        // }

        // Nested for loop. Counting ten times to ten. (think two dimensional square grids stuff...)
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float noise = Mathf.PerlinNoise(((float)x)/width, ((float)z)/depth) ;
                GameObject spawnedPrefab = Instantiate(prefab);
                spawnedPrefab.transform.position = new Vector3(x, noise*noiseScale, z);
                
                // yield return new WaitForSeconds(0.05f);
            }
        }

        int counter = 0;
        while (counter < 10)
        {
            Debug.Log(counter);
            counter ++;
        }


        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
