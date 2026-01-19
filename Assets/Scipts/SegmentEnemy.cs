using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentEnemy : MonoBehaviour
{
    public GameObject head;  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name!=gameObject.name)
        {
            if(head!=null)
            {
                head.GetComponent<EnemyController>().isAttacked = true;
                //Debug.Log("colide in enemy");
            }                         
        }
    }
}
