using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPlayer : MonoBehaviour
{
    public GameObject head;
    public bool isDead = false;
    public GameObject blood;   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyTag" && !isDead)
        {
            if (head != null)
            {
                head.GetComponent<PlayerController>().status = 1;//set trang thai bi tan cong
            }
        }
        if (isDead)
            Destroy(gameObject);
    }
}
