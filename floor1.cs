using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor1 : MonoBehaviour
{
    public man1 man2;
    float scoretime;
    private LineRenderer lineRenderer;
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
                    // 添加 LineRenderer 组件
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        
        // 设置材质和颜色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        
        // 设置宽度
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        
        // 设置位置
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(Random.Range(-4f, 4f), 0, 0));
            transform.parent.GetComponent<floormanager>().SpawnFloor2();
            man2.can=0;
        }
    
       if (transform.position.y > 5f )
        {
            Destroy(gameObject);
    }
        
    }

}
