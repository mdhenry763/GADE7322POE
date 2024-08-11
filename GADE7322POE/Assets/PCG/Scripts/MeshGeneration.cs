using System;
using System.Collections.Generic;
using PerlinNoise;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace ProGen
{
    public class MeshGeneration : MonoBehaviour
    {
        [Header("Area Size")] 
        public int areaX;
        public int areaZ;
        public int heightMultiplier = 5;

        [Header("References")] 
        public PerlinNoiseTexture noise;

        public GameObject tower;
        public GameObject enemyStartPiece;

        private Mesh _generatedMesh;

        private List<Vector3> _startPositions = new();
        private Vector3 _endPosition;

        private int[] _triangles;
        private Vector3[] _vertices;

        private void Start()
        {
            _generatedMesh = new Mesh();
            _generatedMesh.name = "WorldMesh";
            
            GetComponent<MeshFilter>().mesh = _generatedMesh;
            GenerateMesh();
            UpdateMesh();
        }


        private void GenerateMesh()
        {
            //6 points to make a triangular shape
            _triangles = new int[areaX * areaZ * 6];
            //one extra vertex to make a quad
            _vertices = new Vector3[(areaX + 1) * (areaZ + 1)];

            var randEndIndex = UnityEngine.Random.Range(0, areaX);
            
            //Populate vertices
            for (int i = 0, z = 0; z <= areaZ; z++)
            {
                for (int x = 0; x <= areaX; x++)
                {
                    _vertices[i] = new Vector3(x, noise.GenerateHeight(z,x) * heightMultiplier , z);
                    
                    if(z == 1) _startPositions.Add(_vertices[i]);
                    if (z == areaZ - 1 && x == randEndIndex) _endPosition = _vertices[i];
                    
                    i++;
                }
            }

            int tris = 0;
            int verts = 0;
            
            //Generate Quad
            for (int z = 0; z < areaZ; z++)
            {
                for (int x = 0; x < areaX; x++)
                {
                    //One Triangle
                    _triangles[tris + 0] = verts + 0; //bottom left corner
                    _triangles[tris + 1] = verts + areaZ + 1; //top left corner
                    _triangles[tris + 2] = verts + 1; // bottom right corner
                    
                    _triangles[tris + 3] = verts + 1;
                    _triangles[tris + 4] = verts + areaZ + 1;
                    _triangles[tris + 5] = verts + areaZ + 2;
                    
                    verts++;
                    tris+=6;
                }

                verts++;
            }
            
            SpawnStartObjects();
        }

        private void SpawnStartObjects()
        {
            //Spawn Tower
            Instantiate(tower, _endPosition, Quaternion.identity);
            
            //Spawn StartPos
            for (int i = 0; i < 3; i++)
            {
                var spawnPos = _startPositions[UnityEngine.Random.Range(0, _startPositions.Count)];
                Instantiate(enemyStartPiece, spawnPos, Quaternion.identity);
                _startPositions.Remove(spawnPos);
            }
        }

        private void UpdateMesh()
        {
            _generatedMesh.Clear();
            
            _generatedMesh.vertices = _vertices;
            _generatedMesh.triangles = _triangles;
            
            _generatedMesh.RecalculateNormals();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            if(_vertices.Length == 0) return;
            
            foreach (var vertex in _vertices)
            {
                Gizmos.DrawSphere(vertex, 0.1f);
            }
        }
    }
}