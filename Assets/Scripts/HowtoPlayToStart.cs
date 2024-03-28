using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowtoPlayToStart : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("StartScene");
    }
}
