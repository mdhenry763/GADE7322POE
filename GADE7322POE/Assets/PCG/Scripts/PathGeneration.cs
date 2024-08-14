using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class PathGeneration : MonoBehaviour
{
    public NavMeshSurface surface;

    private Vector3 _endPosition;
    private Spline spline;
    private BezierKnot enemyKnot;
    private BezierKnot towerKnot;
    private NavMeshAgent agent;

    private void Start()
    {
        spline = GetComponent<SplineContainer>().Spline;
        agent = GetComponent<NavMeshAgent>();
    }

    public void GeneratePaths(PathData data)
    {
        spline.Add(enemyKnot);
        spline.Insert(1, towerKnot);
        _endPosition = data.TowerPosition;
        GetObjectPositions(data.StartPositions[0]);
        
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

    public void BuildPath()
    {
        surface.BuildNavMesh();
        agent.enabled = true;
        //agent.SetDestination(_endPosition);
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(_endPosition, path);

        List<Vector3> pathPositions = new List<Vector3>();

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            pathPositions.AddRange(path.corners);
        }

        foreach (var piece in pathPositions)
        {
            Debug.Log(piece);
        }
        SetupSpline(pathPositions);
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

        // Optional: Set tangents mode for a smoother path
        for (int i = 0; i < spline.Count; i++)
        {
            spline.SetTangentMode(i, TangentMode.Linear);
        }

        GetComponent<SplineInstantiate>().enabled = true;
    }
}