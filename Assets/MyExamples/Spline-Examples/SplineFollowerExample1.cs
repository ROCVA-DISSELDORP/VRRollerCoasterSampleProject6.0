using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollowerExample1 : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float MaxSpeed = 40f;

    [SerializeField]
    SplineData<float> speed = new SplineData<float>(){DefaultValue = 1};

    private float currentOffset;
    private float currentSpeed;
    private float splineLength;
    private Spline spline;

    void Start()
    {
        spline        = splineContainer.Spline;
        splineLength  = spline.GetLength();
        currentOffset = 0f;
    }

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

    void Update()
    {
        if (splineContainer == null)
            return;
        
        currentOffset = (currentOffset + currentSpeed * Time.deltaTime / splineLength) % 1f;

        if (speed.Count > 0)
            currentSpeed = speed.Evaluate(spline, currentOffset, PathIndexUnit.Normalized, new UnityEngine.Splines.Interpolators.LerpFloat());
        else
            currentSpeed = speed.DefaultValue;

        Vector3 posOnSplineLocal  = SplineUtility.EvaluatePosition(spline, currentOffset);
        Vector3 direction         = SplineUtility.EvaluateTangent( spline, currentOffset);
        Vector3 upSplineDirection = SplineUtility.EvaluateUpVector(spline, currentOffset);

        transform.position = splineContainer.transform.TransformPoint(posOnSplineLocal); // transform from local to world position
        // transform directions from local to world so that when the splinecontainer's transform is rotated the follower will still be rotated in the track's direction.
        transform.rotation = Quaternion.LookRotation(splineContainer.transform.TransformDirection(direction), splineContainer.transform.TransformDirection(upSplineDirection));
    }
}
