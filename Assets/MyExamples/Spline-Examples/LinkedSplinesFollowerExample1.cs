using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class LinkedSplinesFollowerExample1 : MonoBehaviour
{
    public SplineContainer[] splineContainers;
    public float MaxSpeed = 40f;

    [SerializeField]
    SplineData<float> speed = new SplineData<float>(){DefaultValue = 1};

    private float currentOffset;
    private float currentSpeed;
    private float splineLength;
    private Spline spline;

    void OnValidate()
    {
        if (speed != null)
        {
            for (int index = 0; index < speed.Count; index++)
            {
                var data = speed[index];
                //We don't want to have a value that is negative or null as it might block the simulation
                if (data.Value <= 0)
                {
                    data.Value = Mathf.Max(0f, speed.DefaultValue);
                    speed[index] = data;
                }
            }
        }
    }

    void Start()
    {
        spline        = splineContainers[0].Spline;
        splineLength  = spline.GetLength();
        currentOffset = 0f;
    }

    void Update()
    {
        if (splineContainers == null)
            return;
        
        currentOffset = (currentOffset + currentSpeed * Time.deltaTime / splineLength) % 1f;

        // Debug.Log("Current Offset: " + currentOffset);

        if(currentOffset > splineLength)
        {
            // TODO: Fix this..
            Debug.Log("Reached end of spline!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        if (speed.Count > 0)
            currentSpeed = speed.Evaluate(spline, currentOffset, PathIndexUnit.Normalized, new UnityEngine.Splines.Interpolators.LerpFloat());
        else
            currentSpeed = speed.DefaultValue;

        Vector3 posOnSplineLocal  = SplineUtility.EvaluatePosition(spline, currentOffset);
        Vector3 direction         = SplineUtility.EvaluateTangent( spline, currentOffset);
        Vector3 upSplineDirection = SplineUtility.EvaluateUpVector(spline, currentOffset);

        transform.position = splineContainers[0].transform.TransformPoint(posOnSplineLocal); // transform from local to world position
        // transform directions from local to world so that when the splinecontainer's transform is rotated the follower will still be rotated in the track's direction.
        transform.rotation = Quaternion.LookRotation(splineContainers[0].transform.TransformDirection(direction), splineContainers[0].transform.TransformDirection(upSplineDirection));
    }

    public Vector3 GetNearesPoint(SplineContainer[] splineContainers)
    {
        var nearest = new float4(0, 0, 0, float.PositiveInfinity);

        foreach (var container in splineContainers)
        {
            using var native = new NativeSpline(container.Spline, container.transform.localToWorldMatrix);
            float d = SplineUtility.GetNearestPoint(native, transform.position, out float3 p, out float t);
            if (d < nearest.w)
                nearest = new float4(p, d);
        }
        return nearest.xyz;
    }
}
