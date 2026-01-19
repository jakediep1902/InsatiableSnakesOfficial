using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PnlStartManage : MonoBehaviour
{
    public Button btnExit;
    public Button btnPlay;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        btnExit.onClick.AddListener(() => ExitGame());
        btnPlay.onClick.AddListener(() => PlayGame());
    }
    void ExitGame()
    {
        Debug.Log("Exit Game");
        audioSource.Play();
        Application.Quit();
    }
    void PlayGame()
    {
        audioSource.Play();
        SceneManager.LoadScene(2);
    }
    
}
