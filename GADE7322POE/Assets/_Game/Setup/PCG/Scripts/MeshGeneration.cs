using System;
using System.Collections.Generic;
using System.Linq;
using PerlinNoise;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = System.Random;

namespace ProGen
{
    public class MeshGeneration : MonoBehaviour
    {
        [Header("Area Size")] 
        public int areaX;
        public int areaZ;
        public float heightMultiplier = 1;
        public float pathHeight = 0.5f;

        [Header("References")] 
        public PerlinNoiseTexture noise;
        public PathCreator[] pathGenerator;
        public GridManager grid;
        [FormerlySerializedAs("enemyAI")] public EnemySpawner enemySpawner;
        public MeshCollider meshCollider;
        
        [Header("Path Pieces")]
        public GameObject tower;
        public GameObject enemyStartPiece;

        private Mesh _generatedMesh;

        private List<Vector3> _startPositions = new();
        private List<Vector3> _spawnPositions = new List<Vector3>();
        private Vector3 _endPosition;

        private int[] _triangles;
        private Vector3[] _vertices;
        
        public UnityEvent onMeshCreated;

        private void Start()
        {
            _generatedMesh = new Mesh();
            _generatedMesh.name = "WorldMesh";

            heightMultiplier = UnityEngine.Random.Range(1.0f, 1.5f);
            
            GetComponent<MeshFilter>().mesh = _generatedMesh;
            //Build the mesh
            GenerateMesh();
            //UpdateMesh();
        }


        private void GenerateMesh() //Handles creating vertices of the mesh
        {
            //6 points to make a triangular shape
            _triangles = new int[areaX * areaZ * 6];
            //one extra vertex to make a quad
            _vertices = new Vector3[(areaX + 1) * (areaZ + 1)];

            var randEndIndex = UnityEngine.Random.Range(0, areaX);
            var randXIndex = UnityEngine.Random.Range(0, areaX);
            
            //Populate vertices
            for (int i = 0, z = 0; z <= areaZ; z++)
            {
                for (int x = 0; x <= areaX; x++)
                {
                    _vertices[i] = new Vector3(x, noise.GenerateHeight(z,x) * heightMultiplier , z);
                    var position = new Vector3(_vertices[i].x, pathHeight, _vertices[i].z);
                    //Add node to grid for BFS
                    grid.AddNodeToGrid(new Vector2Int(x,z), position);
                    
                    if(z == 1 && x % 2 == 0) _startPositions.Add(_vertices[i]);
                    if (z == areaZ - 1 && x == randEndIndex) _endPosition = _vertices[i];
                    
                    i++;
                }
            }

            GenerateQuad();

            SpawnStartObjects(); //Set spawn positions and tower position
            GeneratePaths(); //Generate paths
        }

        private void GenerateQuad() //handles generation of quads from creating triangles
        {
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
        }

        private void SpawnStartObjects()
        {
            //Spawn Tower
            _endPosition.y = pathHeight;
            Instantiate(tower, _endPosition, Quaternion.identity);
            
            //Spawn 3 different spawn positions and update the spawn positions list
            for (int i = 0; i < 3; i++)
            {
                var spawnPos = _startPositions[UnityEngine.Random.Range(0, _startPositions.Count)];
                var startSpawnPos = new Vector3(spawnPos.x, pathHeight, spawnPos.z);
                
                _spawnPositions.Add(startSpawnPos);
                //Spawn enemy spawner
                Instantiate(enemyStartPiece, startSpawnPos, Quaternion.identity);
                _startPositions.Remove(spawnPos);
            }
            
        }

        private void UpdateMesh() // Pass the generated mesh into the mesh 
        {
            _generatedMesh.Clear();
            
            _generatedMesh.vertices = _vertices;
            _generatedMesh.triangles = _triangles;
            
            _generatedMesh.RecalculateNormals();
            meshCollider.sharedMesh = _generatedMesh;
            onMeshCreated?.Invoke();
        }

        void GeneratePaths() //Create the paths for the enemies to travel on
        {
            
            if(_spawnPositions.Count == 0) return;

            //Store the paths
            Dictionary<int, List<Vector3>> paths = new();
            List<Vector3> pathTest = new();

            int index = 0;
            //Start Node
            foreach (var position in _spawnPositions)
            {
                //Get the start node
                var startPosition = position;
                Vector2Int startNodeCoords = new Vector2Int((int)startPosition.x, (int)startPosition.z);
                Node startNode = new Node(startNodeCoords, true, startPosition);
            
                //Set the EndNode
                Vector2Int endNodeCoords = new Vector2Int((int)_endPosition.x, (int)_endPosition.z);
                Node endNode = new Node(endNodeCoords, true, _endPosition);

                //Use the path finding class to generate a path using breadth first search
                var pathFinder = new PathFinder(grid, startNode, endNode);
                pathFinder.BreadthFirstSearch();
                var path = pathFinder.BuildPath();
            
                //Get all the points in the path
                List<Vector3> pathPositions = path.Select(node => node.position).ToList();
                paths.Add(index, pathPositions);
                //use the path generators to generate 3 different paths
                pathGenerator.FirstOrDefault((path) => path.Index == index).PathGenerator.CreatePath(pathPositions);
                
                index++;
            }
            
            //After path created generate a mesh with level path ways
            UpdateVertices(paths);
            enemySpawner.SpawnEnemies(paths);
        }

        //Update mesh vertices for the path
        private void UpdateVertices(Dictionary<int, List<Vector3>> paths)
        {
            foreach (KeyValuePair<int, List<Vector3>> path in paths)
            {
                int key = path.Key;
                List<Vector3> points = path.Value;

                for (int i = 0; i < points.Count;i++)
                {
                    for (int j = 0; j < _vertices.Length; j++)
                    {
                        if (points[i].x == _vertices[j].x && points[i].z == _vertices[j].z)
                        {
                            _vertices[j].y = pathHeight;
                        }
                    }
                }
            }
            //Regenerate the mesh 
            GenerateQuad();
            UpdateMesh();
        }
    }
}

