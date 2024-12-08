using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Splines;
using Unity.Mathematics;

public class PhysicsTrainV2 : MonoBehaviour
{
    public SplineContainer track;
    // public PhysicsTrainV2 attachedTo;
    public float force = 1000;
    public float minVelocity = 5;

    Rigidbody rb;

    [Header("Debud")]
    public GameObject velocityDirectionGizmo;
    public float velocity;
    public float kmph;
    public float roll;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // var native = new NativeSpline(track.Spline,new float4x4 (track.transform));
        var native = new NativeSpline(track.Spline);
        float distance = SplineUtility.GetNearestPoint(native, transform.localPosition, out float3 nearest, out float t, 4, 2 );

        transform.localPosition = nearest;
        // transform.position = transform.TransformPoint(nearest);
        // transform.position = track.transform.TransformPoint(nearest);
        // transform.position = track.transform.InverseTransformPoint(nearest);        
        // transform.position = transform.InverseTransformPoint(nearest);
        // transform.position = nearest;


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
        
        if (minVelocity > 0 && (rb.linearVelocity.magnitude * engineForward).magnitude < minVelocity)
        {
            rb.AddForce(transform.forward*force);
        }

        /// Trying to calculate drag for when the train rolls
        // rb.velocity *= Mathf.Exp(-(Vector3.Dot(Vector3.right, transform.right) * rb.angularDrag) * Time.fixedDeltaTime);
        // rb.velocity *= Mathf.Exp(-(transform.right.magnitude * rb.angularDrag) * Time.fixedDeltaTime);

        velocity = rb.linearVelocity.magnitude;
        kmph = rb.linearVelocity.magnitude * 3.6f;
        // roll = Vector3.Dot(Vector3.right, transform.right);
        // velocityDirectionGizmo.transform.rotation = Quaternion.LookRotation(rb.velocity);
    }
}
