using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance;    
    public static bool isUserWatchAds = false;
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
        isUserWatchAds = false;
        Admod.eventReward.AddListener(() => isUserWatchAds = true);
    } 
}
