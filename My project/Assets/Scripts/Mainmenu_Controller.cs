using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu_Controller : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
