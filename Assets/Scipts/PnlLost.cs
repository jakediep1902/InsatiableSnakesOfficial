using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnlLost : MonoBehaviour
{
    [SerializeField] GameObject boardCountDown;
    [SerializeField] GameObject boardAtReward;
    private void OnEnable()
    {
        boardAtReward.SetActive(true);
    }
    public void CheckRandomShowBoardCountDown()//decrease rate show add 50%;
    {
        int temp = Random.Range(0, 2);     
        if (temp % 2 == 0)
        {
            boardCountDown.SetActive(true);
        }
    }
}
