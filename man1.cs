using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    public GameManager gameManager;
    float hurtime;
    void Start()
    {
        hp = 15;
        score = 1;
        scoretime = 0;
        timee = 2f;
        filePath = Path.Combine(Application.persistentDataPath, "scores.json");
        LoadScores();
    }

    void Update()
    {
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
            int r = Random.Range(0, 2);
            if (r==0){
                StartCoroutine(MoveOverTime(objTransform,new Vector3(-0.5f, 0, 0), 1));}
            else {
                StartCoroutine(MoveOverTime(objTransform2, new Vector3(0.5f, 0, 0), 1));}
            }
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
        else if (other.gameObject.tag == "hurt" || other.gameObject.tag == "hurt2" || other.gameObject.tag == "hurt3")
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
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "death")
        {
            die();
        }
    }

    void Modifyhp(int num)
    {
        hp += num;
        if (hp > 20)
        {
            hp = 20;
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
            score++;
            scoretime = 0;
            scoreText.text = "地下" + score.ToString() + "層";
            Time.timeScale += 0.05f; 
            timee += 0.7f;
            
             if (score % 5 == 1 && score > 5)
            {
                float tims = (timee - 2) / 4;
                float ttims = (Time.timeScale - 1) / 4;
                StartCoroutine(LoopWithDelay(3, tims, ttims)); // 傳入所需參數
                gameManager.SaveGame();
            }
        }
    }

    IEnumerator LoopWithDelay(int iterations, float tims, float ttims)
    {
        for (int t = 0; t < iterations; t++)
        {
            timee -= tims;
            Time.timeScale -= ttims;
            yield return new WaitForSeconds(1f); // 每秒延遲
        }
    }

    void die()
    {
        GetComponent<AudioSource>().Play();
        Time.timeScale = 0; // 遊戲速度
        replay.SetActive(true);
        back.SetActive(true);
        scoreText2.text = "本次紀錄\n地下" + score.ToString() + "層";
        SaveScore(score);
        scoreText2.gameObject.SetActive(true);
        List<int> scores = LoadScores();
        scoreText3.text = ""; // 清空之前的文本
        for (int i = 0; i < scores.Count; i++)
        {
            scoreText3.text += "第"+(i + 1) + "名: 地下" + scores[i] + "層\n";
        }
        scoreText3.gameObject.SetActive(true);
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        hp = 10;
        // SceneManager.LoadScene("SampleScene");
        gameManager.LoadGame();
    }

    void SaveScore(int score)
    {
        List<int> scores = LoadScores();

        // 添加新的分數並排序
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a)); // 降序排序

        // 只保留前三名
        if (scores.Count > 3)
        {
            scores.RemoveRange(3, scores.Count - 3);
        }

        string json = JsonUtility.ToJson(new ScoreData { scores = scores });
        File.WriteAllText(filePath, json);
    }

    public List<int> LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json);
            return scoreData.scores;
        }
        return new List<int>();
    }
     
}

[System.Serializable]
public class ScoreData
{
    public List<int> scores;
}
public class GameManager : MonoBehaviour
{
    public Transform player;  // 參考玩家的 Transform
public void SaveGame()
    {
        GameData data = new GameData();
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.playerPosition = player.position;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("遊戲已存檔");
    }
     public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            UnityEngine.SceneManagement.SceneManager.LoadScene(data.sceneName);
            player.position = data.playerPosition;
            Debug.Log("遊戲已讀檔");
        }
    }
}