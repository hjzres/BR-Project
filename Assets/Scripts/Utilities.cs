using UnityEngine;

namespace Assets.Scripts
{
    public static class Utilities
    {
        // Wtf is this shit bro I really don't wanna rework this...
        public struct Sorting
        {
            public enum SortType
            {
                Random,
                Horizontal,
                Vertical,
                Grid
            }

            public static Vector2 PositionSortingHelper(SortType type, Vector2 restriction, int amount, int iteration = -1)
            {
                Vector2 position = new Vector2();

                if (iteration == -1 && (type == SortType.Horizontal || type == SortType.Vertical || type == SortType.Grid))
                {
                    Debug.LogError("Since there is no iteration value inputed, function cannot accept Horizontal/Vertical sorting!");
                    return new Vector2();
                }

                if (type == SortType.Random)
                {
                    position = new Vector2(Random.Range(-restriction.x, restriction.x), Random.Range(-restriction.y, restriction.y));
                }

                else if (type == SortType.Horizontal || type == SortType.Vertical)
                {
                    int horizontalMultiplier = type == SortType.Horizontal ? 1 : 0; 
                    int verticalMultiplier = type == SortType.Vertical ? 1 : 0;
                        
                    float domain = ObtainRestrictionValue(restriction) * 2f;
                    float spacing = domain / amount;
                    float pos = (iteration - (amount - 1) / 2f) * spacing;

                    position = new Vector2(pos * horizontalMultiplier, pos * verticalMultiplier);

                    if (type == SortType.Grid)
                    {
                        // TODO: Finish ts if necessary I wanna move on.
                        if (amount % 2 != 0)
                        {
                            Debug.LogError("To use the grid sorting method, amount must be divisble by two!");
                            return new Vector2();
                        }
                    }
                }

                return position;
            }

            private static float ObtainRestrictionValue(Vector2 restriction)
            {
                if (restriction.x == 0)
                {
                    return restriction.y;
                }

                return restriction.x;
            }
        }

        // TODO: Implement easing functions that can be used any time.
        public struct Easing
        {
            public enum EasingType
            {

            }
        }

        public static float[,] PerlinNoiseMap(int width, int height, int seed = 0)
        {
            float[,] map = new float[width, height];
            System.Random prng = new System.Random(seed);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                }
            }

            return map;
        }

        [Tooltip("Converts a 2D float array data into a texture that is then set to the main texture of the returned material.")]
        public static Material NoiseToMaterial(float[,] mapToMaterialize)
        {
            Texture2D texture = new Texture2D(mapToMaterialize.GetLength(0), mapToMaterialize.GetLength(1));
            Color[] colourMap = new Color[mapToMaterialize.GetLength(0) * mapToMaterialize.GetLength(1)];

            for (int i = 0; i < mapToMaterialize.GetLength(0); i++)
            {
                for (int j = 0; j < mapToMaterialize.GetLength(1); j++)
                {
                    colourMap[j * mapToMaterialize.GetLength(0) + i] = Color.Lerp(Color.black, Color.white, mapToMaterialize[i, j]);
                }
            }

            texture.SetPixels(colourMap);
            texture.Apply();

            // TODO: Create material then return it.

            return null;
        }

        /*public struct Noise
        {
            [Min(1)] public int octaves;

            [Min(0.1f)] public float noiseScale;

            [Min(0)] public float frequency;

            [Min(0)] public float amplitude;

            [Range(0, 5)] public float persistance;

            [Range(0, 5)] public float lacunarity;

            [Range(0, 10)] public float heightMultiplier;

            public Noise(int octaves, float noiseScale, float frequency, float amplitude, float persistance, float lacunarity, float heightMultiplier)
            {
                if (octaves < 0) { octaves = 0; }
                if (noiseScale <= 0f) { noiseScale = 0.1f; }
                if (frequency < 0f) { frequency = 0f; }
                if (amplitude < 0f) { amplitude = 0f; }
                if (persistance < 0f || persistance > 2f) { persistance = 1f; }
                if (lacunarity < 0f || lacunarity > 2f) { lacunarity = 1f; }
                if (heightMultiplier <= 0f) { heightMultiplier = 1f; }

                this.octaves = octaves;
                this.noiseScale = noiseScale;
                this.frequency = frequency;
                this.amplitude = amplitude;
                this.persistance = persistance;
                this.lacunarity = lacunarity;
                this.heightMultiplier = heightMultiplier;
            }

            public float[,] PerlinNoise(int width, int length, int seed = 0)
            {
                float[,] noiseMap = new float[width, length];
                System.Random prng = GenerateSeed(seed);
                Vector2[] octaveOffsets = new Vector2[octaves];
                
                for (int i = 0; i < octaves; i++)
                {
                    float xOffset = prng.Next(-1000000, 1000000);
                    float yOffset = prng.Next(-1000000, 1000000);

                    octaveOffsets[i] = new Vector2(xOffset, yOffset);
                }

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < length; y++)
                    {
                        float frequency = this.frequency;
                        float amplitude = this.amplitude;
                        float height = 0;

                        for (int i = 0; i < octaves; i++)
                        {
                            float sampleX = (float)x / width * noiseScale * frequency + octaveOffsets[i].x;
                            float sampleY = (float)y / length * noiseScale * frequency + octaveOffsets[i].y;
                            float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * amplitude;

                            amplitude *= persistance;
                            frequency *= lacunarity;

                            height += perlinValue;
                        }

                        noiseMap[x, y] = height * heightMultiplier;
                    }
                }

                return noiseMap;
            }

            // TODO : Voronoi noise, Simplex noise, Brownian noise, Fractal noise
        }*/

        public static Transform[] RetrieveTransformsByTag(string tag)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            Transform[] transforms = new Transform[gameObjects.Length];

            if (gameObjects.Length <= 0 || gameObjects == null)
            {
                return null;
            }

            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] == null)
                {
                    transforms[i] = null;
                }

                transforms[i] = gameObjects[i].transform;
            }

            return transforms;
        }

        public static System.Random GenerateSeed(int seed)
        {
            return new System.Random(seed);
        }

        public static Vector3 RandomVector(float rangeMin, float rangeMax, int yValue, int seed)
        {
            float x = Random.Range(rangeMin, rangeMax);
            float y = Random.Range(rangeMin, rangeMax) * yValue;
            float z = Random.Range(rangeMin, rangeMax);

            return new Vector3(x, y, z);
        }
    }
}
