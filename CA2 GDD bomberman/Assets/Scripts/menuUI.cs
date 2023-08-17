using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuUI : MonoBehaviour
{
    


    public void ButtonChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void ButtonQuit()
    {
        Application.Quit();
    }


}
