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

    public void CreatePath(List<Vector3> positions)
    {
        
        SetupSpline(positions);
    }
    
    public void GeneratePaths()
    {
        spline.Add(enemyKnot);
        spline.Insert(1, towerKnot);
        //_endPosition = data.TowerPosition;
        //GetObjectPositions(data.StartPositions[0]);
        
    }

    void GetObjectPositions(Vector3 startPos)
    {
        enemyKnot.Position = startPos;
        towerKnot.Position = _endPosition;

        enemyKnot.TangentOut = new float3(0f, 0, 1f);
        towerKnot.TangentIn = new float3(0f, 0, -1f);
        
        spline.SetKnot(0, enemyKnot);
        spline.SetKnot(1, towerKnot);
        
        spline.SetTangentMode(0, mode: TangentMode.Linear, BezierTangent.Out);
        spline.SetTangentMode(1, mode: TangentMode.Linear, BezierTangent.In);

        GetComponent<SplineInstantiate>().enabled = true;
    }
    
    
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

        GetComponent<SplineInstantiate>().enabled = true;
    }
}

[Serializable]
public struct PathCreator
{
    public PathGeneration PathGenerator;
    public int Index;
}