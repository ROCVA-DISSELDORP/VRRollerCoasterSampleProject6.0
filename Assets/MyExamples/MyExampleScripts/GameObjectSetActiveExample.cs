using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public GameObject otherGameObject;

    // Start is called before the first frame update
    void Start()
    {
        // Turn off the GameObject that this script is attached to.
        gameObject.SetActive(false);
        
        // Turn off some other GameObject that is assigned 
        // to the public GameObject otherGameObject variable in the inspector.
        otherGameObject.SetActive(false);

        // Deactivate a GameObject in the scene by searching for it by name.
        GameObject.Find("VRPlayer").SetActive(false);

        // Deactivate a GameObject in the scene by searching for it by tag.
        GameObject.FindWithTag("Player").SetActive(false);
    }
}
