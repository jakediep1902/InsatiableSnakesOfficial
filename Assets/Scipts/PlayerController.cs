using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> segmentList = new List<GameObject>();
    Queue<GameObject> containerSegment = new Queue<GameObject>();

    public AudioClip clipAttacked;
    public AudioClip auAte;
 
    AudioSource auSource;

    public Button btnLaunch;
    public Button btnBoostTempDefaul;
    public Button btnBoostTempPlus;
    public Joystick joystick;
    public float moveSpeed = 1;
    public float maxSize = 3;
    
      
    public int status = 0;
    public int heartAte = 0;
    public int numTest;
    int countOnBtn = 0;

    private bool isBoost = false;

    public GameObject segmentPrefab;
    public GameObject blood;

    public Transform minPosA;
    public Transform maxPosB;
    Vector2 maxPos;
    Vector2 minPos;
    Vector2 moveSnake;
    Vector2 moveDirection;
    Vector2 boostHead;

    void Start()
    {
        auSource = GetComponent<AudioSource>();
        status = 0;    
        segmentList.Add(this.gameObject);
        Grow(numTest);

        btnLaunch.onClick.AddListener(() => Launch(2));
        btnBoostTempDefaul.onClick.AddListener(() => BoostTemp());
        btnBoostTempPlus.onClick.AddListener(() => BoostTemp());
        btnLaunch.gameObject.SetActive(true);

        if(GlobalData.isUserWatchAds)
        {
            AddBoostPlus(true);
            Debug.Log(GlobalData.isUserWatchAds);
            GlobalData.isUserWatchAds = false;
            
        }
        else
        {
            AddBoostPlus(false);
        }

    }  
    //void Update()
    //{      
             
    //    //transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    //    //if(Input.touchCount>0)
    //    //{
    //    //    print("touches");
    //    //    Touch touch = Input.GetTouch(0);
    //    //    Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
    //    //    touchPos.z = 0f;
    //    //    transform.position = touchPos;
    //    //}
    //}
    private void FixedUpdate()
    {
        float yMove = joystick.Vertical;
        float xMove = joystick.Horizontal;
        moveDirection = new Vector2(xMove, yMove);
        if (moveDirection != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);

        minPos = minPosA.position;
        maxPos = maxPosB.position;
        transform.position = new Vector2
         (Mathf.Clamp(transform.position.x, minPos.x, maxPos.x), Mathf.Clamp(transform.position.y, minPos.y, maxPos.y));

        switch (status)
        {
                case 1://Attacked
                    containerSegment.Clear();
                    for (int i = 0; i < segmentList.Count; i++)
                    {
                        if (segmentList[i] == null)
                        {
                            // Debug.Log("null");
                            auSource.clip = clipAttacked;
                            auSource.Play();
                            for (int j = segmentList.Count; j > 0; j--)//set cho phan dut ra
                            {
                                if (segmentList[j - 1] == null)
                                {
                                    break;
                                }
                                segmentList[j - 1].GetComponent<BodyPlayer>().isDead = true;
                                segmentList[j - 1].gameObject.layer = 0;
                                Destroy(segmentList[j - 1], 5f);
                            }
                            segmentList.Clear();
                            foreach (var item in containerSegment)
                            {
                                segmentList.Add(item);
                            }
                            status = 0;
                            break;
                        }
                        containerSegment.Enqueue(segmentList[i]);
                        if (containerSegment.Count == segmentList.Count)
                            status = 0;
                    }
                    break;
                default:   //hunting           
                    for (int i = segmentList.Count - 1; i > 0; i--)
                    {
                        Transform segment = segmentList[i].transform;
                        if (segment == null)
                        {
                            Debug.Log("null");
                        }
                        segment.position = segmentList[i - 1].transform.position;//move segment
                                                                                 //segment.Translate(Vector2.down,Space.Self);bug thu vi
                                                                                 //SegmentLookHead(segment, transform);
                        if (i % 2 == 0)
                        {
                            segment.GetComponent<SpriteRenderer>().color = Color.red;
                        }

                        segment.localScale = Vector2.one * ((segmentList.Count - i) / 50f);//scale segment                   
                        if (segment.localScale.x < 1f && !isBoost)
                        {
                            segment.localScale = Vector2.one; //- Vector2.up*0.6f;
                        }
                        if (segment.localScale.x > maxSize)
                        {
                            segment.localScale = Vector2.one * maxSize;
                        }
                        
                        if(isBoost)
                    {
                        gameObject.transform.localScale = boostHead;
                    }
                    else
                    {
                        gameObject.transform.localScale = segmentList[1].transform.localScale;//scale head
                    }
                            
                        

                         //gameObject.transform.localScale = segmentList[1].transform.localScale;//scale head
                        //gameObject.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.x);
                    }
                    break;
        }

            if (moveDirection == Vector2.zero && segmentList.Count > 15)
            {
                moveSnake = new Vector2(Mathf.Cos(Time.time * 5) * 10, moveSpeed);
            }
            else
                moveSnake = Vector2.up * moveSpeed;           
            //TestGame();       
        transform.Translate(moveSnake * Time.deltaTime);
    }  
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "WallTag")
    //    {
    //        transform.Rotate(Vector3.forward, Random.Range(90, 270));
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==8)
        {            
                string tag = collision.gameObject.tag;
                switch (tag)
                {
                    case "FoodTag":
                        Grow();
                    heartAte++;
                        break;
                    case "ObstacleTag":
                        //Destroy
                        break;
                }                       
        }      
        else if(collision.gameObject.layer!=6)
        {
            if(collision.gameObject.transform.localScale.x<=transform.localScale.x)
            {
                Grow();
                Destroy(collision.gameObject);
                GameObject bl = Instantiate(blood, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
                bl.SetActive(true);
                Destroy(bl, 5);
            }
        }
    }
    public void Grow(int grow=1)
    {
        if(this.gameObject!=null)
        {
            for (int i = 0; i < grow; i++)
            {
                GameObject segment = Instantiate(segmentPrefab, transform.position, Quaternion.identity) as GameObject;
                segment.SetActive(true);
                segmentList.Add(segment);              
                auSource.clip = auAte;
                if (GetComponent<AudioSource>().enabled ==true)
                GetComponent<AudioSource>().Play();
            }
        }                 
    }
    public void TestGame()
    {
        transform.Rotate(Vector3.forward, 1f);
    }
    public void Launch(int speed=10)
    {     
        if (countOnBtn<2)
        {
            if (countOnBtn == 1)
                btnLaunch.gameObject.SetActive(false);
            countOnBtn++;
            Time.fixedDeltaTime -= 0.005f;
            moveSpeed = speed * moveSpeed;           
        }             
             
    }
    public float SetMoveSpeed(float speed=10f)
    {
        float spe = speed;
        return spe;
    }
    //public void SegmentLookHead(Transform segment,Transform head)
    //{
    //    segment.rotation = head.rotation;
    //}
    public void BoostTemp()
    {
        StartCoroutine(nameof(CorBoosTemp));
    }
    IEnumerator CorBoosTemp()
    {
        isBoost = true;
        boostHead = gameObject.transform.localScale*2;       
        btnBoostTempDefaul.gameObject.SetActive(false);
        yield return new WaitForSeconds(10);       
        isBoost = false;
    }
    public void AddBoostPlus(bool status = true)
    {
        btnBoostTempPlus.gameObject.SetActive(status);
    }
}
