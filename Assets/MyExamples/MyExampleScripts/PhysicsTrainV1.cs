using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Splines;
using Unity.Mathematics;

public class PhysicsTrainV1 : MonoBehaviour
{
    public SplineContainer track;
    // public PhysicsTrainV1 attachedTo;
    public float force = 1000;

    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var native = new NativeSpline(track.Spline);
        float distance = SplineUtility.GetNearestPoint(native, transform.position, out float3 nearest, out float t, 4, 2 );

        transform.position = nearest;

        Vector3 forward = Vector3.Normalize(track.EvaluateTangent(t));
        Vector3 up = track.EvaluateUpVector(t);

        // var remappedForward = new Vector3(0,1,0);
        // var remappedUp = new Vector3(0,0,1);
        // var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward));

        // transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;
        transform.rotation = Quaternion.LookRotation(forward, up);

        Vector3 engineForward = transform.forward;

        if(Vector3.Dot(rb.linearVelocity, transform.forward) < 0)
        {
            engineForward *= -1;
        }

        rb.linearVelocity = rb.linearVelocity.magnitude * engineForward;

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward*force);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward*force);
        }
    }

    // Update is called once per frame
    // void FixedUpdate()
    // {
    //     // var native = new NativeSpline(track.Spline,new float4x4 (track.transform));
    //     var native = new NativeSpline(track.Spline);
    //     float distance = SplineUtility.GetNearestPoint(native, transform.localPosition, out float3 nearest, out float t, 4, 2 );

    //     transform.localPosition = nearest;
    //     // transform.position = transform.TransformPoint(nearest);
    //     // transform.position = track.transform.TransformPoint(nearest);
    //     // transform.position = track.transform.InverseTransformPoint(nearest);        
    //     // transform.position = transform.InverseTransformPoint(nearest);
    //     // transform.position = nearest;


    //     Vector3 forward = Vector3.Normalize(track.EvaluateTangent(t));
    //     Vector3 up = track.EvaluateUpVector(t);

    //     // var remappedForward = new Vector3(0,1,0);
    //     // var remappedUp = new Vector3(0,0,1);
    //     // var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward));

    //     // transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;
    //     transform.rotation = Quaternion.LookRotation(forward, up);

    //     Vector3 engineForward = transform.forward;

    //     if(Vector3.Dot(rb.velocity, transform.forward) < 0)
    //     {
    //         engineForward *= -1;
    //     }

    //     rb.velocity = rb.velocity.magnitude * engineForward;

    //     if (Input.GetKey(KeyCode.W))
    //     {
    //         rb.AddForce(transform.forward*force);
    //     }
    //     else if(Input.GetKey(KeyCode.S))
    //     {
    //         rb.AddForce(-transform.forward*force);
    //     }
    // }
}
