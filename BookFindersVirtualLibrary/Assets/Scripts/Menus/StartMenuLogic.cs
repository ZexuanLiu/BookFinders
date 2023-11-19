using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuLogic : MonoBehaviour
{
    public void StartVirtualLibrary()
    {
        SceneManager.LoadScene("VirtualLibrary");
    }

    public void Quit()
    {
        Application.Quit();
    }


}
