using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public GameObject[] trashPrefabs;
    public GameObject[] npcPrefabs;
    public GameObject[] trashBagPrefabs;
    public GameObject[] boxPrefabs;         // 디버그용 

    public int depth = 20;               // 맵 높이

    int width = 1000;             // 맵 가로크기
    int height = 1000;            // 맵 세로크기

    public float scale = 4f;             // 지형 펠린노이즈 스케일
    public float treeNoiseScale = 6f;    // 나무 펠린노이즈 스케일
    public float treeDensity = 0.8f;     // 나무 밀집도

    public float trashNoiseScale = 6f;   // 쓰레기 펠린노이즈 스케일
    public float trashDensity = 0.8f;    // 쓰레기 밀집도

    public float npcNoiseScale = 6f;     // npc 펠린노이즈 스케일
    public float npcDensity = 0.8f;      // npc 밀집도

    public float trashBagNoiseScale = 6f;
    public float trashBagDensity = 0.5f;

    public float offsetX = 100f;         // x좌표
    public float offsetY = 100f;         // z좌표

    public float octaves = 4;            // 펠린노이즈 레이어 갯수
    public float persistance = 0.5f;     // 
    public float lacunarity = 2;         // 노이즈의 주파수

    public float heightMultiplier = 1.5f;

    float maxNoiseHeight = float.MinValue;
    float minNoiseHeight = float.MaxValue;

    float[,] heights;

    public float falloffStart = 400f;
    public float falloffEnd = 500f;

    void Start()
    {
        offsetX = Random.Range(0f, 9999f);                              // 랜덤한 지형 좌표 설정
        offsetY = Random.Range(0f, 9999f);

        Terrain terrain = GetComponent<Terrain>();
        // TerrainLayer[] tLayers = terrain.terrainData.terrainLayers;
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        GenerateTrees(terrain);                             // 터레인 데이터로 나무 생성
        GenerateTrash(terrain);
        GenerateNpc(terrain);
        GenerateTrashBag(terrain);

        //GenerateBox(terrain);   ////////////////////////////////////////////////////////////////////디버그용
    }

    void Update()
    {

    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());        // SetHeights(int x축 시작위치, int y축 시작위치, float[,] 좌표마다 높이값)

        return terrainData;
    }

    //float[,] GenerateIsland()
    //{

    //    for (int y = 0; y < height; y++)            // terrain의 가장자리일수록 높이 감소
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            float distance = Vector2.Distance(new Vector2(x, y), new Vector2(500, 500));
    //            float t = Mathf.InverseLerp(falloffEnd, falloffStart, distance);
    //            heights[x, y] = heights[x, y] * t;
    //        }
    //    }

    //    return heights;
    //}

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;            // 진폭
                float frequency = 1;            // 주파수 : 노이즈패턴이 얼마나 빠르게 변하는지
                float noiseHeight = 0;          // 노이즈 높이

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (float)x / width * scale * frequency + offsetX;
                    float yCoord = (float)y / height * scale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                heights[x, y] = noiseHeight * heightMultiplier;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                heights[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heights[x, y]);
            }
        }

        for (int y = 0; y < height; y++)            // terrain의 가장자리일수록 높이 감소
        {
            for (int x = 0; x < width; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(500, 500));
                float t = Mathf.InverseLerp(falloffEnd, falloffStart, distance);
                heights[x, y] = heights[x, y] * t;
            }
        }

        return heights;
    }

    //TerrainLayer PaintTerrain(){}


    void GenerateTrees(Terrain terrain)
    {
        float[,] heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;            // 진폭
                float frequency = 1;            // 주파수 : 노이즈패턴이 얼마나 빠르게 변하는지
                float noiseHeight = 0;          // 노이즈 높이

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (float)x / width * treeNoiseScale * frequency + offsetX;
                    float yCoord = (float)y / height * treeNoiseScale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                heights[x, y] = noiseHeight * heightMultiplier;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float treeheights = terrain.SampleHeight(new Vector3(x, 0, y));
                float v = Random.Range(-800, treeDensity);

                if (heights[x, y] < v)
                {
                    if (treeheights > 5)
                    {
                        GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                        GameObject tree = Instantiate(prefab);
                        tree.transform.position = new Vector3(x, treeheights, y);
                        tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        tree.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                    }
                }
            }
        }
    }

    void GenerateTrash(Terrain terrain)
    {
        float[,] heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;            // 진폭
                float frequency = 1;            // 주파수 : 노이즈패턴이 얼마나 빠르게 변하는지
                float noiseHeight = 0;          // 노이즈 높이

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (float)x / width * trashNoiseScale * frequency + offsetX;
                    float yCoord = (float)y / height * trashNoiseScale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                heights[x, y] = noiseHeight * heightMultiplier;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float trashheights = terrain.SampleHeight(new Vector3(x, 0, y));
                float v = Random.Range(-800, treeDensity);

                if (heights[x, y] < v)
                {
                    if (trashheights > 5)
                    {
                        GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
                        GameObject trash = Instantiate(prefab);
                        trash.transform.position = new Vector3(x, trashheights+1, y);
                        trash.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        trash.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                    }

                }
            }
        }

    }

    void GenerateNpc(Terrain terrain)
    {
        float[,] heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;            // 진폭
                float frequency = 1;            // 주파수 : 노이즈패턴이 얼마나 빠르게 변하는지
                float noiseHeight = 0;          // 노이즈 높이

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (float)x / width * npcNoiseScale * frequency + offsetX;
                    float yCoord = (float)y / height * npcNoiseScale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                heights[x, y] = noiseHeight * heightMultiplier;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float npcheights = terrain.SampleHeight(new Vector3(x, 0, y));
                float v = Random.Range(-800, treeDensity);

                if (heights[x, y] < v)
                {
                    if (npcheights > 5)
                    {
                        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
                        GameObject trash = Instantiate(prefab);
                        trash.transform.position = new Vector3(x, npcheights, y);
                        trash.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        trash.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                    }

                }
            }
        }

    }

    void GenerateTrashBag(Terrain terrain)
    {
        float[,] heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;            // 진폭
                float frequency = 1;            // 주파수 : 노이즈패턴이 얼마나 빠르게 변하는지
                float noiseHeight = 0;          // 노이즈 높이

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (float)x / width * trashBagNoiseScale * frequency + offsetX;
                    float yCoord = (float)y / height * trashBagNoiseScale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                heights[x, y] = noiseHeight * heightMultiplier;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float trashBagheights = terrain.SampleHeight(new Vector3(x, 0, y));
                float v = Random.Range(-800, treeDensity);

                if (heights[x, y] < v)
                {
                    if (trashBagheights > 5)
                    {
                        GameObject prefab = trashBagPrefabs[Random.Range(0, trashBagPrefabs.Length)];
                        GameObject trash = Instantiate(prefab);
                        trash.transform.position = new Vector3(x, trashBagheights, y);
                        trash.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        //trash.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                    }

                }
            }
        }

    }

    void GenerateBox(Terrain terrain)
    {
        float[,] heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;            // 진폭
                float frequency = 1;            // 주파수 : 노이즈패턴이 얼마나 빠르게 변하는지
                float noiseHeight = 0;          // 노이즈 높이

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (float)x / width * treeNoiseScale * frequency + offsetX;
                    float yCoord = (float)y / height * treeNoiseScale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                heights[x, y] = noiseHeight * heightMultiplier;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float treeheights = terrain.SampleHeight(new Vector3(x, 0, y));

                float v = Random.Range(-1000, treeDensity);
                if (true)
                {
                    if(heights[x, y] < v)//heights[x, y] < v)
                    {
                        if (true)   //(treeheights > 5)
                        {
                            GameObject prefab = boxPrefabs[Random.Range(0, boxPrefabs.Length)];
                            GameObject box = Instantiate(prefab, transform);
                            box.transform.position = new Vector3(x, treeheights, y);
                            //Debug.Log(heights);
                        }
                    }
                    

                }
            }
        }

    }

}

