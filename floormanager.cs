using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floormanager : MonoBehaviour
{
    [SerializeField] GameObject[] floor; 
    public man1 man2;
    
    private List<GameObject> spawnedFloors = new List<GameObject>();
    public void SpawnFloor()
    {
        int r = Random.Range(0, floor.Length);
        GameObject floort = Instantiate(floor[r], transform);
        if(0<=man2.score && man2.score<=10)
        {
        floort.transform.position = new Vector3(Random.Range(-3f, 3f), -5f, 0);
        }
        if(5<man2.score && man2.score<=10)
        {
            int r2 = Random.Range(0, 3);
            if (r2 == 0) StartCoroutine(RotateFloor(floort));
            else if (r2 == 1) StartCoroutine(MoveFloor(floort));
            else if (r2 == 2) StartCoroutine(MoveFloor2(floort));
        }
        if(10<man2.score && man2.score<=15)
        {
            floort.transform.position = new Vector3(0, -5f, 0);


        }
        if(15<man2.score && man2.score<=20)
        {
            floort.transform.position = new Vector3(Random.Range(0, 6f), -5f, 0);
            floort.transform.Rotate(0, 0, 20, Space.Self);
        }
        if(20<man2.score && man2.score<=25)
        {
            int r2 = Random.Range(0, 2);
            if (r2 == 0) 
            {                
            floort.transform.position = new Vector3(Random.Range(0, 6f), -5f, 0);
            floort.transform.Rotate(0, 0, 25, Space.Self);
            }
            else if (r2 == 1) 
            {                
            floort.transform.position = new Vector3(Random.Range(-6f,0), -5f, 0);
            floort.transform.Rotate(0, 0, -25, Space.Self);
            }
        }
    }
        public void SpawnFloor2()
    {
        for(int leen=0; leen>-100;leen--)
        {
        int r = Random.Range(0, floor.Length);
        GameObject floort = Instantiate(floor[r], transform);
        floort.transform.position = new Vector3(Random.Range(-3f, 3f), -5.2f+leen*1.3f, 0);
        }
    }
     IEnumerator MoveFloor2(GameObject floort){
        float moveDuration = man2.score/(6f);
        for (float t = 0; t < moveDuration/2; t += Time.deltaTime)
            {
                if (floort == null) yield break; // 检查对象是否已被销毁
               floort.transform.Translate(0.01f,0,0);
                yield return null;
            }
         for (float q = 0; q<10;q++)
        {
            for (float t = 0; t < moveDuration; t += Time.deltaTime) 
            {
                if (floort == null) yield break; // 检查对象是否已被销毁
               floort.transform.Translate(-0.01f,0,0);
                yield return null;
            }

            // 向右移动
            for (float t = 0; t < moveDuration; t += Time.deltaTime)
            {
                if (floort == null) yield break; // 检查对象是否已被销毁
               floort.transform.Translate(+0.01f,0,0);
                yield return null;
            }
        }
    }
    IEnumerator MoveFloor(GameObject floort){
        float moveDuration = man2.score/(6f*5);
        for (float t = 0; t < moveDuration/2; t += Time.deltaTime)
            {
                if (floort == null) yield break; // 检查对象是否已被销毁
               floort.transform.Translate(-0.01f,0,0);
                yield return null;
            }
         for (float q = 0; q<10;q++)
        {
            for (float t = 0; t < moveDuration; t += Time.deltaTime) 
            {
                if (floort == null) yield break; // 检查对象是否已被销毁
               floort.transform.Translate(0.01f,0,0);
                yield return null;
            }

            // 向右移动
            for (float t = 0; t < moveDuration; t += Time.deltaTime)
            {
                if (floort == null) yield break; // 检查对象是否已被销毁
               floort.transform.Translate(-0.01f,0,0);
                yield return null;
            }
        }
    }
           IEnumerator RotateFloor(GameObject floort)
    {
        while (floort != null)
        {

            // 旋转30度
            int r3 = Random.Range(0, 2);
            if (r3 == 0)
            {if (floort == null) yield break;
                floort.transform.Rotate(0, 0, -20, Space.Self);
                yield return new WaitForSeconds(man2.timee/100);
                if (floort == null) yield break;
                floort.transform.Rotate(0, 0, 20, Space.Self);
            }
            else
            {if (floort == null) yield break;
                floort.transform.Rotate(0, 0, 20, Space.Self);
                yield return new WaitForSeconds(man2.timee/100);
                if (floort == null) yield break;
                floort.transform.Rotate(0, 0, -20, Space.Self);
            }
            yield return new WaitForSeconds(1 / man2.timee);
        }
    }
    

}
