using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class PathGeneration : MonoBehaviour
{
    
    private Vector3 _endPosition;
    private Spline spline;
    private BezierKnot enemyKnot;
    private BezierKnot towerKnot;
    

    private void Start()
    {
        spline = GetComponent<SplineContainer>().Spline;
    }

    private void Awake()
    {
        spline = GetComponent<SplineContainer>().Spline;
    }

    //Generate spline for path
    public void CreatePath(List<Vector3> positions)
    {
        SetupSpline(positions);
    }
    
    //Spawn splines
    private void SetupSpline(List<Vector3> pathPositions)
    {
        spline.Clear();

        for (int i = 0; i < pathPositions.Count; i++)
        {
            BezierKnot knot = new BezierKnot(pathPositions[i]);
            
            if (i > 0)
            {
                // Calculate tangents for smooth transitions
                Vector3 direction = (pathPositions[i] - pathPositions[i - 1]).normalized;
                knot.TangentIn = -direction * 0.5f; 
                knot.TangentOut = direction * 0.5f;
            }

            spline.Add(knot);
        }

        // Set tangents mode for a smoother path
        for (int i = 0; i < spline.Count; i++)
        {
            spline.SetTangentMode(i, TangentMode.Linear);
        }

        //Spawn the splines using the spline instantiation
        GetComponent<SplineInstantiate>().enabled = true;
    }
}

[Serializable]
public struct PathCreator
{
    public PathGeneration PathGenerator;
    public int Index;
}