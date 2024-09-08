using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PerlinNoise
{
    public class PerlinNoiseTexture : MonoBehaviour
    {
        public int depth = 20;
        public int width = 256;
        public int height = 256;

        public float scale = 20;
        public float offsetX = 100;
        public float offsetY = 100;

        private MeshRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            //random positions
            offsetX = Random.Range(0, 999999);
            offsetY = Random.Range(0, 999999);
            
            
            _renderer.material.mainTexture = GeneratePerlinNoiseTexture();
        }

        void GenerateHeights()
        {
            float[,] heights = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    heights[x, y] = GenerateHeight(x, y);
                }    
            }
        }

        //Testing to see perlin noise on a texture
        Texture2D GeneratePerlinNoiseTexture()
        {
            Texture2D texture = new Texture2D(width, height);
        
            //Generate map using perlin noise
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = GenerateColour(x, y);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

        //Generate the height using perlin noise
        public float GenerateHeight(int x, int y)
        {
            //Offset is used to get a random area on the generated texture
            float xCoord = (float)x / width * scale + offsetX;
            float yCoord = (float)y / height * scale + offsetY;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }

        Color GenerateColour(int x, int y)
        {
            float xCoord = (float)x / width * scale + offsetX;
            float yCoord = (float)y / height * scale + offsetY;
            
            var colour = Mathf.PerlinNoise(xCoord, yCoord);
            return new Color(colour, colour, colour);
        }
    }
}


