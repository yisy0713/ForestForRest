using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentManager : MonoBehaviour
{
    private int CleanUpLevel;

    public Terrain myTerrain;
    public Texture2D mossTexture;
    public Texture2D PoisonTexture;

    public GameObject water;
    public Material waterMaterialBlue;
    public Material waterMaterialPoison;

    public Material skyMaterialPoison;
    public Material skyMaterialBlue;

    private bool DoneEnvireonmentCleanUp;
    public bool DoneCollectTrash;

    // Start is called before the first frame update
    void Start()
    {
        CleanUpLevel = 0;
        DoneEnvireonmentCleanUp = false;
        DoneCollectTrash = false;

        myTerrain.terrainData.terrainLayers[0].diffuseTexture = PoisonTexture;  // ������ �� �ͷ��η��̾�
        //myTerrain.terrainData.terrainLayers[2].diffuseTexture = texture2;
        //myTerrain.Flush();

        RenderSettings.skybox = skyMaterialPoison;  // ������ �ϴ� ��ī�̹ڽ� 

        water.GetComponent<MeshRenderer>().material = waterMaterialPoison; // ������ �� ���׸���
    }

    private void Update()
    {
        if(DoneEnvireonmentCleanUp && DoneCollectTrash)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }

    public void CleanUp()
    {
        switch (CleanUpLevel)
        {
            case 0:
                myTerrain.terrainData.terrainLayers[0].diffuseTexture = mossTexture;
                CleanUpLevel++;
                break;
            case 1:
                CleanUpLevel++;
                RenderSettings.skybox = skyMaterialBlue;
                break;
            case 2:
                CleanUpLevel++;
                water.GetComponent<MeshRenderer>().material = waterMaterialBlue;
                DoneEnvireonmentCleanUp = true;
                break;
            default:
                break;
        }
    }
}
