using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardAdsReward : MonoBehaviour
{   
    private void Start()
    {
        GameController.eventSkipAds.AddListener(() => this.gameObject.SetActive(false));
        GameController.eventGetReward.AddListener(() => this.gameObject.SetActive(false));
    }
}
