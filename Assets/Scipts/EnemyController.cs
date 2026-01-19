using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected List<GameObject> segmentList = new List<GameObject>();
    protected Queue<GameObject> containerSegment = new Queue<GameObject>();
    protected GameController gameController;


    public float moveSpeed = 1;
    public float upTime =2f;
    protected  float lastUpTime;
    public float frequence = 5;
    public float amplitude = 3;
    public float growing = 1;
    public float maxSize = 3;

    protected int length = 10;
    public int minLength = 5;
    public int maxLength = 20;

    protected string snakeID;

    protected  bool isDeath = false;
    public bool isAttacked = false;
    protected bool isSound = false;

    public GameObject segmentPrefab;
    public GameObject blood;

    public Transform minPosA;
    public Transform maxPosB;

    protected Vector2 maxPos;
    protected Vector2 minPos;
    protected Vector2 moveSnake;

    Color segmentColor1;
    Color segmentColor2;

    void Start()
    {
        gameController = GameController.Instance;
        length = (int)Random.Range(minLength, maxLength);
        isDeath = false;     
        lastUpTime = Time.time;     
        segmentList.Add(this.gameObject);
        Grow(length);
        segmentColor1 = Color.yellow;
        segmentColor2 = Color.black;

        
    }  
    private void FixedUpdate()
    {     
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

                    //for (int j = 0; j < containerSegment.Count; j++) //bi loi chi add duoc phan nua
                    //{
                    //    segmentList.Add(containerSegment.Dequeue());                 
                    //}
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
                    segment.GetComponent<SpriteRenderer>().color = segmentColor1;
                }
                else
                {
                    segment.GetComponent<SpriteRenderer>().color = segmentColor2;
                }
                //segment.GetComponent<SpriteRenderer>().sortingOrder = -i;//set xen ke mau sac
                segment.position = segmentList[i - 1].transform.position;//move segment                                                        
                segment.localScale = growing*Vector2.one * ((segmentList.Count - i) / 50f);//scale segment                   
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
                moveSnake = new Vector2(Mathf.Cos(Time.time * frequence * 3) * amplitude, moveSpeed * 1.8f);
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

    private void OnDisable()
    {
        for (int j = segmentList.Count; j > 0; j--)
        {
            if (segmentList[j - 1] == null)
            {
                break;
            }
            Destroy(segmentList[j - 1], 5f);
        }
        //if (gameController != null)
        //{
        //    gameController.countSpaw--;
        //}
        //else
        //{
        //    Debug.Log("GameControler is null");//bao null tai day
        //    //bug chua hieu,co the lien quan den singleton do nhieu thang cung goi den gamecontroller khi disable        
        //}          
    }   
    private void OnTriggerEnter2D(Collider2D collision)
    {                  
        if (collision.gameObject.name!=snakeID)
        {                 
            if (collision.gameObject.layer==8)
            {
                string tag = collision.gameObject.tag;               
                switch (tag)
                { 
                    case "FoodTag":
                        Grow();
                        break;
                    case "ObstacleTag":
                        //Destroy
                        break;
                }                                
            }            
            else
            {
                if(collision.gameObject.transform.localScale.x <= transform.localScale.x)
                {
                    Destroy(collision.gameObject);
                    CreateBlood(blood);
                    Grow();
                }                             
            }                                   
        }
    }   
    public void Grow(int grow = 1)
    {
        for (int i = 0; i < grow; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, transform.position, Quaternion.identity) as GameObject;
            segment.name = snakeID;
            segment.SetActive(true);
            segmentList.Add(segment);
            gameController.listTotalSegment.Add(segment);
        }
    }
    public void SoundRun()
    {
        isSound = true;
        GetComponent<AudioSource>().Play();
    }
    public void CreateBlood(GameObject blood)
    {
        GameObject bl = Instantiate(blood, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
        bl.SetActive(true);
        Destroy(bl, 5);
    }
}
