using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor1 : MonoBehaviour
{
    public man1 man2;
    float scoretime;
    // Start is called before the first frame update
    void Start()
    {
     
       
    }

    void Update()
    {   
        
        man2=FindObjectOfType<man1>();
            scoretime += Time.deltaTime;  
        transform.Translate(0, man2.timee * Time.deltaTime, 0);
        // if (transform.position.y > 5f && man2.score<=25)
        // {

        //     transform.parent.GetComponent<floormanager>().SpawnFloor();
        // }
        // if (  man2.score==26 && man2.can==1 && transform.position.y<-4f)
        if (  man2.score==1 && man2.can==1 && transform.position.y<-4f)
        {
            transform.parent.GetComponent<floormanager>().SpawnFloor2();
            man2.can=0;
 
        }

    if (transform.position.y > 5f )
        {
            Destroy(gameObject);
    }
        
    }

}
