using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance;    
    //public static bool isUserWatchAt = false;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        //isUserWatchAt = false;
        //Atmob.eventReward.AddListener(() => isUserWatchAt = true);
    } 
}
