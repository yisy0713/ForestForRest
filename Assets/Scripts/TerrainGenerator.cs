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
    public GameObject[] boxPrefabs;         // ����׿� 

    public int depth = 20;               // �� ����

    int width = 1000;             // �� ����ũ��
    int height = 1000;            // �� ����ũ��

    public float scale = 4f;             // ���� �縰������ ������
    public float treeNoiseScale = 6f;    // ���� �縰������ ������
    public float treeDensity = 1.5f;     // ���� ������

    public float trashNoiseScale = 6f;   // ������ �縰������ ������
    public float trashDensity = 0.8f;    // ������ ������

    public float npcNoiseScale = 6f;     // npc �縰������ ������
    public float npcDensity = 0.8f;      // npc ������

    public float trashBagNoiseScale = 6f;
    public float trashBagDensity = 0.5f;

    public float offsetX = 100f;         // x��ǥ
    public float offsetY = 100f;         // z��ǥ

    public float octaves = 4;            // �縰������ ���̾� ����
    public float persistance = 0.5f;     // 
    public float lacunarity = 2;         // �������� ���ļ�

    public float heightMultiplier = 1.5f;
       
    float maxNoiseHeight = float.MinValue;      // �ִ� ������ ����
    float minNoiseHeight = float.MaxValue;      // �ּ� ������ ����

    float[,] heights;

    public float falloffStart = 400f;
    public float falloffEnd = 500f;

    NavMeshSurface navMeshSurface;

    void Awake()
    {
        offsetX = Random.Range(0f, 9999f);                              // ������ ���� ��ǥ ����
        offsetY = Random.Range(0f, 9999f);

        Terrain terrain = GetComponent<Terrain>();
        // TerrainLayer[] tLayers = terrain.terrainData.terrainLayers;
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        GenerateTrees(terrain);                             // �ͷ��� �����ͷ� ���� ����
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
        terrainData.SetHeights(0, 0, GenerateHeights());        // SetHeights(int x�� ������ġ, int y�� ������ġ, float[,] ��ǥ���� ���̰�)

        return terrainData;
    }

    float[,] GenerateNoise(float scale)
    {
        float[,] heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;            // ����
                float frequency = 1;            // ���ļ� : ������������ �󸶳� ������ ���ϴ���
                float noiseHeight = 0;          // ������ ����

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (float)x / width * scale * frequency + offsetX;          // �縰�������� x,y ��ǥ ���
                    float yCoord = (float)y / height * scale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;          // �ش� ��ǥ�� ������� ������ -1�� 1���� ������ ��ȯ
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;           // �� ��Ÿ�� ���� �������� ���� ����
                    frequency *= lacunarity;            // �� ��Ÿ�� ���� �������� ���ļ� ����
                }

                if (noiseHeight > maxNoiseHeight)       // �ּ�&�ִ� ������ ���� ������Ʈ
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                heights[x, y] = noiseHeight * heightMultiplier;     // heightMultiplier�� ���� �����ؼ� �ش� ��ǥ�� ���̰� ����
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
                // �� ��ǥ�� ���̸� �ּҳ�������̿� �ִ��������� ������ ������ ��ȯ�ؼ� ����ȭ
                heights[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heights[x, y]);

                // terrain�� �����ڸ��ϼ��� ���� ����
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
                    if (treeheights > 5)    // ���� ������ ����
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

