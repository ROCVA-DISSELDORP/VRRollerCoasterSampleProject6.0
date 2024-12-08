using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentingExample : MonoBehaviour
{
    private GameObject myParent;
    private GameObject myGrandParent;

    // Start is called before the first frame update
    void Start()
    {
        myParent = transform.parent.gameObject;
        myGrandParent = myParent.transform.parent.gameObject;

        Debug.Log("The name of my parent is " + myParent.name);
        Debug.Log("The name of my parents parent is " + myGrandParent.name);
    }
}
