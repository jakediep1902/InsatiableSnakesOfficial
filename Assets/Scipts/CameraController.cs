using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    GameController gameController;
    private float xScale=1;
    private float cameraSize=20;
    public int startSize = 20;
    private void Start()
    {
        gameController = GameController.Instance;
    }
    void Update()
    {
        
       

        if (player != null)
        {
            Vector3 tempPos = player.position;
            tempPos.z = transform.position.z;
            transform.position = tempPos;

            xScale = player.transform.localScale.x;
            cameraSize = startSize + xScale*2;
            Camera.main.orthographicSize = cameraSize;
        }
        else
        {
            gameController.isEndGame = true;
        }
            
        
    }
}
