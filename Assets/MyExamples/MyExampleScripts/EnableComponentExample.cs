using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentExample: MonoBehaviour
{
    public GameObject otherGameObject;

    // Start is called before the first frame update
    void Start()
    {
        //Get the BoxCollider component that is attached to this GameObject and disable it.
        gameObject.GetComponent<BoxCollider>().enabled = false;

        //Get the BoxCollider component that is attached to otherGameObject and disable it.
        otherGameObject.GetComponent<BoxCollider>().enabled = false;

        // Get the EnableComponentExample component (this script) 
        // that is attached to this GameObject and disable it.
        gameObject.GetComponent<EnableComponentExample>().enabled = false;

        // To set 'this' script/component's enabled bool to false, like in line 20, we don't 
        // have to 'get' this component first because we can access its enabled bool directly.
        enabled = false;
    }
}