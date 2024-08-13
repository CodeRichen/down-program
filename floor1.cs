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

    // Update is called once per frame
    void Update()
    {   
        
        man2=FindObjectOfType<man1>();
        scoretime+=Time.deltaTime;  
        transform.Translate(0,man2.timee*Time.deltaTime,0);
        if(transform.position.y>5f){
        Destroy(gameObject);
    transform.parent.GetComponent<floormanager>().SpawnFloor();
 }
        
    }

}
