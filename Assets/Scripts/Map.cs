using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static bool MapActivated = false;

    public GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        MapActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenMap();
    }

    private void TryOpenMap()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MapActivated = !MapActivated;

            if (MapActivated)
            {
                
                OpenMap();
            }
            else
            {
                CloseMap();
            }
        }
    }

    private void OpenMap()
    {
        map.gameObject.SetActive(true);
    }

    private void CloseMap()
    {
        map.gameObject.SetActive(false);
    }
}
