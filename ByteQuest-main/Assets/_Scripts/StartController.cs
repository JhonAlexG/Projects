using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    private void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void clickJugar(){
        SceneManager.LoadScene("SampleScene");
    }

    public void clickSalir(){
        Application.Quit();
    }   
}
