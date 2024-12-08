using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingExample1 : MonoBehaviour
{
    void FixedUpdate()
    {
        // if (Physics.Raycast(transform.position, transform.forward) == true)
        //     print("There is something in front of the object!");

        // if (Physics.Raycast(transform.position, transform.forward, 100) == true)
        //     print("There is something in front of the object!");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            print("Hit an object at position : " + hit.point);

            // Draw a green line from this object's position to the point where the ray hit.
            Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else
        {
            // Draw a red line from this object's position to 10 units in
            // the forward direction when the ray doesn't hit anyting.
            Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.red);
        }
    }
}
