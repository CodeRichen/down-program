using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor1 : MonoBehaviour
{
    public man1 man2;
    public floormanager floormanager2;
    private float scoretime;
    public int can;
    
    void Start()
    {
        man2 = FindObjectOfType<man1>(); //自動匹配
        floormanager2 = FindObjectOfType<floormanager>();
        can=man2.score;
    }

    void Update()
    {   

        scoretime += Time.deltaTime;  
        transform.Translate(0, man2.timee * Time.deltaTime, 0);
        if (transform.position.y > 5f && floormanager2.can2==1)
        {

            transform.parent.GetComponent<floormanager>().SpawnFloor();
        }
        if( man2.score%5==1){
        if ( (man2.score!=can || man2.can==1) && transform.position.y<-4f){
            
            transform.parent.GetComponent<floormanager>().SpawnFloor2();
            can=man2.score;
            man2.can=0;
        }
        }

       if (transform.position.y > 5f )
        {
            Destroy(gameObject);
    }
}

}
