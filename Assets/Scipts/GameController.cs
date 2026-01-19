
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private static volatile GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {               
                instance = Object.FindFirstObjectByType<GameController>();
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<GameController>();
                }
            }
            return instance;
        }
    }

    AudioSource auSource;
    public AudioSource auSource2;
    public AudioClip auDeath;
    public AudioClip auBackGround;
    public AudioClip auPnlEnd;
    public AudioClip auPnlWin;
    public AudioClip auHitBtn;

    public List<GameObject> listTotalSegment = new List<GameObject>();
    public GameObject snakeBasic;
    public GameObject snakcLv1;
    public GameObject snakcLv2;
    public GameObject snakcLv3;

    public GameObject Cup1;
    public GameObject Cup2;
    public GameObject Cup3;

    public GameObject pnlWin;
    public GameObject pnlEndGame;
    public GameObject pnlMenu;
    public GameObject player;
    public Transform minA;
    public Transform maxB;

    public Button btnMenu;
    public Button btnReturn;
    public Button btnRestartGame;
    public Button btnRestartGameLost;
    public Button btnBackToHomeMenu;
    public Button btnBackToHomeLost;
    public Button btnReplay;
    public Button btnBackToHome;
    public Button btnPlayGame;
    public Button btnGetReward;
    public Button btnSkipAds;
    public Text score;
    public Text heartAte;
    public Text bestScore;
    public Slider targetBar;

    private PlayerController playerCtr;
    public Admod ad;

    public static UnityEvent eventSkipAds = new UnityEvent();
    public static UnityEvent eventGetReward = new UnityEvent();

    Vector2 spawPos;
    string bestScoreString;

    float spawTime = 5;
    float lastSpawTime;
    public float fixedUp = 0.03f;

    //public int countSpaw = 9;
    public int totalSegment = 0;
    int checkList = 0;
    

    public bool isEndGame = false;
    private bool stopGame = false;
    private bool isWin = false;
  
    void Start()
    {
        Time.timeScale = 1;
        lastSpawTime = Time.time;
        Time.fixedDeltaTime= fixedUp;

        btnMenu.onClick.AddListener(() => SetMenu());
        btnReturn.onClick.AddListener(() => SetReturn());
        btnBackToHomeMenu.onClick.AddListener(() => SetBackToHome());
        btnBackToHome.onClick.AddListener(() => SetBackToHome());
        btnRestartGame.onClick.AddListener(() => SetRestartGame());
        btnBackToHomeLost.onClick.AddListener(() => SetBackToHome());
        btnRestartGameLost.onClick.AddListener(() => SetRestartGame());
        btnReplay.onClick.AddListener(() => SetRestartGame());
        btnPlayGame.onClick.AddListener(() => PlayGame());
        btnGetReward.onClick.AddListener(() => { eventGetReward?.Invoke();});
        btnSkipAds.onClick.AddListener(() => { eventSkipAds?.Invoke(); });

        auSource = GetComponent<AudioSource>();
        playerCtr = player.GetComponent<PlayerController>();
      
    }
    void Update()
    {
        if (stopGame)
            return;

        if (isEndGame)
        {
            EndGame();
        }
        if (Time.time > lastSpawTime + spawTime )
        {
            //SnakeTest();
            SortSegment();
            checkList = listTotalSegment.Count;
            //Debug.Log("Total: "+checkList);
            if (checkList < 500)
            {
                int lvSnake = playerCtr.segmentList.Count;
                if (lvSnake > 10)
                {
                    Quaternion random = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    spawPos = new Vector2(Random.Range(minA.position.x, maxB.position.x), Random.Range(minA.position.y, maxB.position.y));
                    GameObject snake = Instantiate(snakeBasic, spawPos, random) as GameObject;                
                    snake.SetActive(true);
                    //countSpaw++;
                }
                if (lvSnake > 50)
                {
                    Quaternion random = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    spawPos = new Vector2(Random.Range(minA.position.x, maxB.position.x), Random.Range(minA.position.y, maxB.position.y));
                    GameObject snake = Instantiate(snakcLv1, spawPos, random) as GameObject;
                    snake.SetActive(true);
                    //countSpaw++;
                }
                if (lvSnake > 150)
                {
                    Quaternion random = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    spawPos = new Vector2(Random.Range(minA.position.x, maxB.position.x), Random.Range(minA.position.y, maxB.position.y));
                    GameObject snake = Instantiate(snakcLv2, spawPos, random) as GameObject;
                    snake.SetActive(true);
                    //countSpaw++;
                }
                if (lvSnake > 240)
                {
                    Quaternion random = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    spawPos = new Vector2(Random.Range(minA.position.x, maxB.position.x), Random.Range(minA.position.y, maxB.position.y));
                    GameObject snake = Instantiate(snakcLv3, spawPos, random) as GameObject;
                    snake.SetActive(true);
                    //countSpaw++;
                }
                //if (countSpaw < 0)
                //    countSpaw = 10;
                lastSpawTime = Time.time;
                ClearTrash();
            }          
        }
        else
        {           
            SortSegment();
            checkList = listTotalSegment.Count;
            //Debug.Log("Total After Sort: " + listTotalSegment.Count);            
        }
        if (player != null && !isWin)
            SetScore();
    }
    public void EndGame()
    {
        auSource2.Stop();
        auSource.clip = auDeath;
        auSource.Play();
        auSource.loop = false;
        stopGame = true;
        Invoke("LoadScene", 3f);
    }
    public void WinGame()
    {
        heartAte.text = playerCtr.heartAte.ToString();
        SetCup();
        lastSpawTime = 9999999999999f;
        pnlWin.SetActive(true);
        player.SetActive(false);
        auSource2.Stop();
        auSource.clip = auPnlWin;
        auSource.volume = 1f;
        auSource.Play();
        auSource.loop = false;
    }
    public void SetScore()
    {

        int countLenght = player.GetComponent<PlayerController>().segmentList.Count;
        if (countLenght >= targetBar.maxValue)
        {
            targetBar.value = targetBar.maxValue;
            score.text = "100%";
            WinGame();
            isWin = true;
        }
        else
        {           
            int temScore = (int)(countLenght * 100 / targetBar.maxValue);
            score.text = temScore.ToString() + "%";
            bestScoreString = ((int)(countLenght * 100 / targetBar.maxValue)).ToString() + "%";
            targetBar.value = countLenght;
        }

    }
    public void SetMenu()
    {
        pnlMenu.SetActive(true);
        HitButton();
        Time.timeScale = 0;
    }
    public void SetReturn()
    {
        pnlMenu.SetActive(false);
        HitButton();
        Time.timeScale = 1;
    }
    public void SetBackToHome()
    {
        SceneManager.LoadScene(1);
        HitButton();
    }
    public void SetRestartGame()
    {
        HitButton();
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
        Time.timeScale = 1;
    }
    public void PlayGame()
    {
        HitButton();
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
    public void LoadScene()
    {      
        pnlEndGame.SetActive(true);
        bestScore.text = "Best Score: " + bestScoreString;
        auSource.clip = auPnlEnd;
        auSource.volume = 1f;
        auSource.Play();
        auSource.loop = false;
    }
    public void HitButton()
    {
        auSource.clip = auHitBtn;
        auSource.volume = 1f;
        auSource.Play();
        auSource.loop = false;
    }
    protected void SetCup()
    {
        if (playerCtr.heartAte >= 100)
        {
            Cup1.SetActive(true);
            Cup2.SetActive(false);
            Cup3.SetActive(false);
        }
        if (playerCtr.heartAte >= 50&& playerCtr.heartAte < 100)
        {
            Cup1.SetActive(false);
            Cup2.SetActive(true);
            Cup3.SetActive(false);
        }
        if (playerCtr.heartAte <50)
        {
            Cup1.SetActive(false);
            Cup2.SetActive(false);
            Cup3.SetActive(true);
        }

    }
    void SnakeTest()
    {
        Quaternion random = Quaternion.Euler(0, 0, Random.Range(0, 360));
        spawPos = new Vector2(Random.Range(minA.position.x, maxB.position.x), Random.Range(minA.position.y, maxB.position.y));
        GameObject snake = Instantiate(snakcLv3, spawPos, random) as GameObject;
        snake.SetActive(true);
        //countSpaw++;
    }
    void SortSegment()
    {
        for (int i = 0; i < listTotalSegment.Count; i++)
        {
            if (listTotalSegment[i] == null)
            {
                listTotalSegment.Remove(listTotalSegment[i]);
            }
        }
    }
    void ClearTrash()
    {                    
        SegmentEnemy[] trash = Object.FindObjectsByType<SegmentEnemy>(FindObjectsSortMode.InstanceID);
        BodyPlayer[] trash2 = FindObjectsByType<BodyPlayer>(FindObjectsSortMode.InstanceID);
        for (int i = 0; i < trash.Length; i++)
        {
            //Debug.Log("Trash: "+trash.Length);
            if (trash[i]!=null&& trash[i].GetComponent<SegmentEnemy>().enabled == false)
            {            
                    Destroy(trash[i].gameObject);
            }          
        }
        for (int i = 0; i < trash2.Length; i++)
        {
            //Debug.Log("Trash: "+trash.Length);
            if (trash2[i] != null && trash2[i].GetComponent<BodyPlayer>().enabled == false)
            {
                Destroy(trash2[i].gameObject);
            }
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}

