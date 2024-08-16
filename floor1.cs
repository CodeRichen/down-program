using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor1 : MonoBehaviour
{
    public man1 man2;
    private float scoretime;
    private float checkInterval = 0.5f; // 檢查的時間間隔
    private float timeSinceLastCheck = 0f;

    void Start()
    {
        man2 = FindObjectOfType<man1>();
    }

    void Update()
    {   
        scoretime += Time.deltaTime;  
        transform.Translate(0, man2.timee * Time.deltaTime, 0);
        
        // 如果得分小於 25，並且位置超過 5f 則刪除該物件
        // if (man2.score < 25 && transform.position.y > 5f)
        // {
        //     Destroy(gameObject);
        //     transform.parent.GetComponent<floormanager>().SpawnFloor();
        // }

        // 每 0.5 秒檢查一次
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= checkInterval)
        {
            CheckAndMaintainFloorLimit();
            timeSinceLastCheck = 0f;
        }
    }

    void CheckAndMaintainFloorLimit()
    {
        // 找到所有的 floor1 物件
        floor1[] allFloors = FindObjectsOfType<floor1>();
        
        // 如果總數超過8，刪除Y軸上位置最大的floor1物件
        if (allFloors.Length ==8)
        {
            Debug.Log(allFloors.Length);
            floor1 highestFloor = allFloors[0];
            foreach (floor1 floor in allFloors)
            {
                if (floor.transform.position.y > highestFloor.transform.position.y)
                {
                    highestFloor = floor;
                }
            }
            StartCoroutine(DestroyAfterDelay(highestFloor.gameObject, 0.5f));
        }
    }

    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
        transform.parent.GetComponent<floormanager>().SpawnFloor();
    }
}
