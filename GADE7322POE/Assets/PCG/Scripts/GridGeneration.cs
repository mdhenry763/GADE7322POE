using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProGen
{
    public class GridGeneration : MonoBehaviour
    {
        public GameObject worldPiece;
        [Space]
        public int gridSizeX = 10;
        public int gridSizeZ = 10;
        public float noiseHeight = 3f;

        private float gridOffset;
        private float offsetX;
        private float offsetZ;

        private void Start()
        {
            gridOffset = worldPiece.transform.localScale.x;
            offsetX = UnityEngine.Random.Range(0, 9999999);
            offsetZ = UnityEngine.Random.Range(0, 9999999);
        
            GenerateGrid();
        }

        void GenerateGrid()
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    var position = GeneratePosition(x, z);
                    Instantiate(worldPiece, position, Quaternion.identity, transform);
                }
            }
        }

        Vector3 GeneratePosition(int x, int z)
        {
            Vector3 pos = new Vector3(x + gridOffset, GenerateNoise(x, z , 10f), z + gridOffset);
            return pos;
        }

        private float GenerateNoise(int x, int z, float detailScale)
        {
            float xNoise = (x + transform.position.x) / detailScale;
            float zNoise = (z + transform.position.y) / detailScale;

            return Mathf.PerlinNoise(xNoise, zNoise);
        }
    }
}

