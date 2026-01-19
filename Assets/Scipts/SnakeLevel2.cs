using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLevel2 : SnakeLevel1
{
    void Start()
    {
        gameController = GameController.Instance;
        length = (int)Random.Range(20, 40);
        isDeath = false;
        lastUpTime = Time.time;
        segmentList.Add(this.gameObject);
        Grow(length);
    }
    //void Update()
    //{
       
    //}
    private void FixedUpdate()
    {
        // transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        minPos = minPosA.position;
        maxPos = maxPosB.position;
        transform.position = new Vector2
         (Mathf.Clamp(transform.position.x, minPos.x, maxPos.x), Mathf.Clamp(transform.position.y, minPos.y, maxPos.y));

        if (isAttacked)
        {
            containerSegment.Clear();
            for (int i = 0; i < segmentList.Count; i++)
            {
                if (segmentList[i] == null)
                {
                    for (int j = segmentList.Count; j > 0; j--)
                    {
                        if (segmentList[j - 1] == null)
                        {
                            break;
                        }
                        segmentList[j - 1].gameObject.name = "Food";
                        Destroy(segmentList[j - 1], 4f);
                    }
                    segmentList.Clear();

                    foreach (var item in containerSegment)
                    {
                        segmentList.Add(item);
                    }

                    isAttacked = false;
                    containerSegment.Clear();
                    break;
                }
                containerSegment.Enqueue(segmentList[i]);
            }
        }

        if (!isDeath)
        {
            if (Time.time > lastUpTime + upTime)
            {
                transform.Rotate(Vector3.forward, Random.Range(0, 360));
                lastUpTime = Time.time;
            }

            for (int i = segmentList.Count - 1; i > 0; i--)
            {
                Transform segment = segmentList[i].transform;
                if (i % 2 == 0)
                {
                    segment.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    segment.GetComponent<SpriteRenderer>().color = Color.black;
                }
                segment.position = segmentList[i - 1].transform.position;//move segment                                                        
                segment.localScale = growing * Vector2.one * ((segmentList.Count - i) / 50f);//scale segment                   
                if (segment.localScale.x < 1f)
                {
                    segment.localScale = Vector2.one;
                }
                if (segment.localScale.x > maxSize)
                {
                    segment.localScale = Vector2.one * maxSize;
                }
                gameObject.transform.localScale = segmentList[1].transform.localScale;//scale head
            }
            if (segmentList.Count > 10)
            {
                moveSnake = new Vector2(Mathf.Cos(Time.time * frequence) * amplitude, moveSpeed);
                isSound = true;
            }                
            else
            {
                //moveSnake = Vector2.up * moveSpeed * 1.5f;
                moveSnake = new Vector2(Mathf.Cos(Time.time * frequence * 4) * amplitude, moveSpeed * 1.5f);
                if (isSound)
                {
                    SoundRun();
                    isSound = false;
                }
            }
            transform.Translate(moveSnake * Time.deltaTime);
        }
    }
    private void OnEnable()
    {
        snakeID = gameObject.GetInstanceID().ToString();
        gameObject.name = snakeID;
    }

    private void OnDisable()//set segments left time life
    {
        for (int j = segmentList.Count; j > 0; j--)
        {
            if (segmentList[j - 1] == null)
            {
                break;
            }
            Destroy(segmentList[j - 1], 5f);
        }
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != snakeID)
        {
            if (collision.gameObject.layer == 8)
            {
                string tag = collision.gameObject.tag;
                switch (tag)
                {
                    case "FoodTag":
                        Grow();
                        break;
                    case "ObstacleTag":
                        //Detroy gameObject
                        break;
                }
            }
            else
            {
                if (collision.gameObject.transform.localScale.x <= transform.localScale.x)
                {
                    Destroy(collision.gameObject);
                    CreateBlood(blood);
                    Grow();
                }
            }
        }
    }
}
