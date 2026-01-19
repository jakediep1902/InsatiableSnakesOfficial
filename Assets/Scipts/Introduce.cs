using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Introduce : MonoBehaviour
{  
    VideoPlayer introduce; 
    void Start()
    {
        introduce = GetComponent<VideoPlayer>();
        Debug.Log(introduce.frameCount);
    }
    void Update()
    {
        if ((long)introduce.frame == (long)(introduce.frameCount - 1))
        {
            SceneManager.LoadScene(1);
        }         
    }
}
