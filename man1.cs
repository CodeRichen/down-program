using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public class ScoreEntry
{
    public int score;          // 分數
     public int Fraction;          // 關卡
    public string date;        // 日期，以字符串格式存儲
}

[System.Serializable]
public class ScoreData
{
    public List<ScoreEntry> scores;
}
public class man1 : MonoBehaviour
{
    GameObject currentfloor;
    public float Rmove = 3f;
    public float Lmove = -3f;
    public float Hmove = 5f;
    public float timee = 2f;
    private string filePath;
    [SerializeField] int hp;
    [SerializeField] GameObject hpall;
    [SerializeField] Text scoreText;
    [SerializeField] Text scoreText2;
    [SerializeField] Text scoreText3;
    float scoretime;
    public int score;
    [SerializeField] GameObject replay;
    [SerializeField] GameObject back;
    public string hurt3;
    public string hurt2;
    public string Player;
    float hurtime;
    public int can;
    public string RedObject;
    public int Fraction;
    [SerializeField] Text FractionText;
    private List<ScoreEntry> scores; // 定义 scores 变量
    void Start()
    {
        can=1;
        hp = 15;
        score = 1;
        scoretime = 0;
        timee = 2f;
        Fraction=10;
        filePath = Path.Combine(Application.persistentDataPath, "scores.json");
        LoadScores();
        if (replay != null)
        {
            EventSystem.current.SetSelectedGameObject(replay);
        }
        scores = LoadScores(); // 在 Start 或者其他合适的方法中初始化 scores
    }

    void Update()
    {
        FractionText.text = Fraction.ToString() ;
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Rmove * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Animator>().SetBool("move", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Lmove * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<Animator>().SetBool("move", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("move", false);
        }
        // 如果有手機觸碰事件
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = touch.position;

                if (touchPosition.x < Screen.width / 2)
                {
                    // 向左移動
                    transform.Translate(Lmove * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<Animator>().SetBool("move", true);
                }
                else if (touchPosition.x >= Screen.width / 2)
                {
                    // 向右移動
                   transform.Translate(Rmove * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Animator>().SetBool("move", true);
                }
                else
        {
            GetComponent<Animator>().SetBool("move", false);
        }
            }
        }
         if (Input.GetKeyDown(KeyCode.Space))
        {
            // 如果按下Space，選中replay按鈕
            Button currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                if (currentButton != null && scoreText2.gameObject.activeSelf)
                {
                    currentButton.onClick.Invoke();
                }
        }
        updatescore();
        if(10<score && score<=15)
        {
            hurtime+=Time.deltaTime;
            GameObject obj = GameObject.FindGameObjectWithTag(hurt3);
            Transform objTransform = obj.transform;
            GameObject obj2 = GameObject.FindGameObjectWithTag(hurt2);
            Transform objTransform2 = obj2.transform;
            if (hurtime>3)
            {
                hurtime=0;
            int r = UnityEngine.Random.Range(0, 2); 
            //编译器不确定应该使用 UnityEngine.Random 还是 System.Random，因为这两个命名空间中都有 Random 类
            if (r==0){
                StartCoroutine(MoveOverTime(objTransform,new Vector3(-0.5f, 0, 0), 1));}
            else {
              
                StartCoroutine(MoveOverTime(objTransform2, new Vector3(0.5f, 0, 0), 1));}
            }
    }
        if(25+3==score){
           Time.timeScale =  0.7f;
        }
        if(25+4==score){
           Time.timeScale =  0.5f;
        }
        if(25+5==score){
           Time.timeScale =  0.4f;
        }
    }

    
 IEnumerator MoveOverTime(Transform obj, Vector3 endPos, float time)
    {
        Vector3 startPos = obj.position;
        Vector3 endPos2 = obj.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            obj.position = Vector3.Lerp(startPos, endPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一幀
        }

        obj.position = endPos; // 確保物件最終位置是精確的目標位置
        yield return new WaitForSeconds(1);

        startPos = obj.position;
        elapsedTime = 0;

        while (elapsedTime < time)
        {
            obj.position = Vector3.Lerp(startPos, endPos2, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一幀
        }

        obj.position = endPos2; // 確保物件最終位置是精確的目標位置
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "floor1")
        {
            if (other.contacts[0].normal == new Vector2(0, 1f) || (score>15 && score<=25))
            {
                currentfloor = other.gameObject;
                Modifyhp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "floor2")
        {
            if (other.contacts[0].normal == new Vector2(0, 1f) || (score>15 && score<=25))
            {
                currentfloor = other.gameObject;
                Modifyhp(-1);
                GetComponent<Animator>().SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "hurt" || other.gameObject.tag == "hurt2" 
        || other.gameObject.tag == "hurt3" )
        {
            if (currentfloor != null)
            {
                BoxCollider2D boxCollider = currentfloor.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }
            }
            GetComponent<Animator>().SetTrigger("hurt");
            Modifyhp(-1);
            other.gameObject.GetComponent<AudioSource>().Play();
            CFraction(-5);
        }
        else if (other.gameObject.tag == "death")
        {
            die();
            
        }
         
    }

    void OnTriggerEnter2D(Collider2D  other)
{
    if (other.gameObject.tag == "RedObject") 
        {
            GetComponent<Animator>().SetTrigger("hurt");
            Modifyhp(-1); 
            other.gameObject.GetComponent<AudioSource>().Play();
            CFraction(-15);
        }
}
    public void Modifyhp(int num)
    {
        hp += num;
        if (num>0){
            CFraction(3);
        }
        if (hp > 20)
        {
            hp = 20;
            CFraction(5);
        }
        else if (hp < 1)
        {
            hp = 0;
            die();
            
        }
        updatehp();
    }

    void updatehp()
    {
        for (int i = 0; i < hpall.transform.childCount; i++)
        {
            if (hp > i)
            {
                hpall.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                hpall.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void updatescore()
    {
        scoretime += Time.deltaTime;
        if (scoretime > 4f)
        {
            CFraction(5);
            score++;
            scoretime = 0;
            scoreText.text = "地下" + score.ToString() + "層";
            Time.timeScale += 0.02f; 
            timee += 1f;
             if (score % 5 == 1 && score > 5)
            {
                float tims = (timee - 3) / 4f;
                float ttims = (Time.timeScale - 1) / 4f;
                StartCoroutine(LoopWithDelay(4, tims, ttims)); // 傳入所需參數

            }
        }
    }

    IEnumerator LoopWithDelay(int iterations, float tims, float ttims)
    {
        
        if (iterations==4){
        for (int t = 0; t < iterations; t++)
        {
            timee -= tims;
            Time.timeScale -= ttims;
            yield return new WaitForSeconds(0.5f); // 每秒延遲
        }
        }
        if (iterations==2){
            for (int t = 0; t < iterations; t++)
        {
            timee += tims;
            Time.timeScale += ttims;
            yield return new WaitForSeconds(0.5f);
        }
        }
    }

    void die()
    {
        CFraction(-10);
        SaveScore(score,Fraction);
        GetComponent<AudioSource>().Play();
        Time.timeScale = 0; // 遊戲速度
        replay.SetActive(true);
        back.SetActive(true);
        scoreText2.text = "地下" + score.ToString() + "層\n"+ Fraction.ToString()+"分";
        scoreText2.gameObject.SetActive(true);
         // 获取当前的分数条目
        
        scoreText3.text = ""; // 清空之前的文本
        for (int i = 0; i < scores.Count; i++)
        {
            ScoreEntry entry = scores[i];
             // 更新文本，包括分数和日期
                 string colorTag = "";

        // 根据名次设置颜色
        switch (i)
        {
            case 0:
                colorTag = "<color=#FFD700>"; // 金色
                break;
            case 1:
                colorTag = "<color=#C0C0C0>"; // 银色
                break;
            case 2:
                colorTag = "<color=#CD7F32>"; // 铜色
                break;
            default:
                colorTag = "<color=#FFFFFF>"; // 白色（默认颜色）
                break;
        }
        scoreText3.text += colorTag + entry.Fraction + "分(第"+entry.score+"層)" + entry.date + "\n" + "</color>";
        }
        scoreText3.gameObject.SetActive(true);
        
    }

    public void Replay()
    {
        CFraction(-23);
        hp = 5;
        score=score-((score-1)%5);
        replay.SetActive(false);
        back.SetActive(false);
        scoreText2.gameObject.SetActive(false);
        scoreText3.gameObject.SetActive(false);
        GameObject obj3 = GameObject.Find("man1");
        obj3.transform.position = new Vector3(0, 3, 0);
        scoretime = 0;
        scoreText.text = "地下" + score.ToString() + "層";
        timee=1;
        Time.timeScale=0.1f;
        float tims = 2/2f;
        float ttims = (0.9f)/2;
        StartCoroutine(LoopWithDelay(2, tims, ttims)); // 傳入所需參數

        }
    void CFraction(int Frac){
        Fraction+=Frac;
    }
void SaveScore(int score,int Fraction)
    {
        List<ScoreEntry> scores = LoadScores();

        // 创建新的分數條目并添加当前日期
        ScoreEntry newEntry = new ScoreEntry
        {
            score = score,
            Fraction =Fraction,
            date = DateTime.Now.ToString("MM/dd HH:mm") // 設置日期格式
        };

        // 添加新的分數條目並排序（根据分数降序）
        scores.Add(newEntry);
        scores.Sort((a, b) => b.Fraction.CompareTo(a.Fraction)); // 根据分数降序排序

        // 只保留前三名
        if (scores.Count > 10)
        {
            scores.RemoveRange(10, scores.Count - 10);
        }

        // 将数据转换为JSON并保存到文件中
        string json = JsonUtility.ToJson(new ScoreData { scores = scores });
        File.WriteAllText(filePath, json);
    }

    public List<ScoreEntry> LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json);
            return scoreData.scores;
        }
        return new List<ScoreEntry>();
    }
}

