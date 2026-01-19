using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    Vector2 randomPos;
    public Transform markA;
    public Transform markB;
    Vector2 posA;
    Vector2 posB;   
    private void Start()
    {
        posA = markA.position;
        posB = markB.position;

       // _startPosition = transform.position;
    }
    //private Vector3 _startPosition;
    //private Vector3 _newPosition;
   
    //void Update()
    //{
    //    _newPosition = transform.position;
    //    _newPosition.x += Mathf.Sin(Time.time*5) * Time.deltaTime;
    //    transform.position = _newPosition;
    //    transform.Translate(transform.position.x, 5 * Time.deltaTime, transform.position.z);
    //    //transform.position = _startPosition + new Vector3(Mathf.Sin(Time.time*5f), 0.1f, 0.0f);
    //    //Debug.Log(Time.time);
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        randomPos = new Vector2(Random.Range(posA.x,posB.x), Random.Range(posA.y, posB.y));
        if (collision.gameObject.tag=="PlayerTag")
        {
            Move();
            GetComponent<AudioSource>().Play();
        }
    }
    public void Move()
    {
        this.transform.position = randomPos;
    }
}
