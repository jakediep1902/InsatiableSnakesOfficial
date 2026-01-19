using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardCountDown : MonoBehaviour
{
    public static UnityEvent eventCountDone = new UnityEvent();
    public Text txtCountDown;
    public int duration = 2;
    IEnumerator countDown(int duration)
    {
        for (int i = 0; i < duration; i++)
        {
            txtCountDown.text = (duration-i).ToString();
            yield return new WaitForSeconds(1);
            if(i==duration-1)
            {
                eventCountDone?.Invoke();
                this.gameObject.SetActive(false);
            }
        }     
    }
    private void OnEnable()
    {
        StartCoroutine(countDown(duration));
    }
}
