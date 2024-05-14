using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public float treeDensity = 1.5f;     // 나무 밀집도

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
       
    float maxNoiseHeight = float.MinValue;      // 최대 노이즈 높이
    float minNoiseHeight = float.MaxValue;      // 최소 노이즈 높이

    float[,] heights;

    public float falloffStart = 400f;
    public float falloffEnd = 500f;

    NavMeshSurface navMeshSurface;

    void Awake()
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

        BakeNavMesh();
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

    float[,] GenerateNoise(float scale)
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
                    float xCoord = (float)x / width * scale * frequency + offsetX;          // 펠린노이즈의 x,y 좌표 계산
                    float yCoord = (float)y / height * scale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;          // 해당 좌표의 노이즈값을 가져와 -1과 1사이 값으로 변환
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;           // 각 옥타브 마다 노이즈의 진폭 감소
                    frequency *= lacunarity;            // 각 옥타브 마다 노이즈의 주파수 증가
                }

                if (noiseHeight > maxNoiseHeight)       // 최소&최대 노이즈 높이 업데이트
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                heights[x, y] = noiseHeight * heightMultiplier;     // heightMultiplier로 높이 조정해서 해당 좌표에 높이값 설정
            }
        }

        return heights;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = GenerateNoise(scale);
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // 각 좌표의 높이를 최소노이즈높이와 최대노이즈높이 사이의 비율로 변환해서 정규화
                heights[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heights[x, y]);

                // terrain의 가장자리일수록 높이 감소
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width / 2, height / 2));
                float t = Mathf.InverseLerp(falloffEnd, falloffStart, distance);
                heights[x, y] = heights[x, y] * t;
            }
        }

        return heights;
    }

    //TerrainLayer PaintTerrain(){}


    void GenerateTrees(Terrain terrain)
    {
        float[,] heights = GenerateNoise(treeNoiseScale);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float treeheights = terrain.SampleHeight(new Vector3(x, 0, y));
                float v = Random.Range(-800, treeDensity);

                if (heights[x, y] < v)
                {
                    if (treeheights > 5)    // 수면 위에만 생성
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
        float[,] heights = GenerateNoise(trashNoiseScale);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float trashheights = terrain.SampleHeight(new Vector3(x, 0, y));
                float v = Random.Range(-800, trashDensity);

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
        float[,] heights = GenerateNoise(npcNoiseScale);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float npcheights = terrain.SampleHeight(new Vector3(x, 0, y));
                float v = Random.Range(-800, npcDensity);

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
        float[,] heights = GenerateNoise(trashBagNoiseScale);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float trashBagheights = terrain.SampleHeight(new Vector3(x, 0, y));
                float v = Random.Range(-800, trashBagDensity);

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

    private void BakeNavMesh()
    {
        NavMeshSurface navMeshSurface = GetComponent<NavMeshSurface>();

        navMeshSurface.RemoveData();

        navMeshSurface.BuildNavMesh();

        Debug.Log("BakedNavMesh");
    }

}

