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
        man2 = FindObjectOfType<man1>();
        can=man2.score;
    }

    void Update()
    {   
        Debug.Log(can);
        scoretime += Time.deltaTime;  
        transform.Translate(0, man2.timee * Time.deltaTime, 0);
        
        if (transform.position.y > 5f && man2.score%5!=1)
        {
            Destroy(gameObject);
            transform.parent.GetComponent<floormanager>().SpawnFloor();
        }
        if( man2.score%5==1){
        if ( man2.score!=can && transform.position.y<-4f){
            
            transform.parent.GetComponent<floormanager>().SpawnFloor2();
            can=man2.score;
           
        }
       if (transform.position.y > 5f )
        {
            Destroy(gameObject);
    }
  
}
}
}

