using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor1 : MonoBehaviour
{
    public man1 man2;
    private float scoretime;
    private float spawnInterval = 1f; // 呼叫 SpawnFloor 的間隔時間
    private float duration = 4f; // 持續時間
    private int can;
    void Start()
    {
        man2 = FindObjectOfType<man1>();
    }

    void Update()
    {   
        scoretime += Time.deltaTime;  
        transform.Translate(0, man2.timee * Time.deltaTime, 0);
        
        // if (transform.position.y > 5f && man2.score%5!=1)
        // {
        //     Destroy(gameObject);
        //     transform.parent.GetComponent<floormanager>().SpawnFloor();
        // }
        if (man2.score%5==1 && man2.score>can){
            can=man2.score;
            StartCoroutine(AutoSpawnFloors());
        }
       
    }
    IEnumerator AutoSpawnFloors()
    {
        Debug.Log(1);
        float elapsedTime = 0f;
         while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(spawnInterval); 
            
            if (transform.position.y > 5f){
                // DestroyHighestPlatform();
            transform.parent.GetComponent<floormanager>().SpawnFloor();
            }
            
            elapsedTime += Time.deltaTime;
            
            
        }
    }
    private void DestroyHighestPlatform()
    {
        // 找到 Y 軸位置最大的物件
        GameObject highestPlatform = null;
        float maxY = float.MinValue;

        foreach (Transform child in transform)
        {
            if (child.position.y > maxY)
            {
                maxY = child.position.y;
                highestPlatform = child.gameObject;
            }
        }

        // 如果找到物件，則刪除
        if (highestPlatform != null)
        {
            Destroy(highestPlatform);
        }
    }
}


