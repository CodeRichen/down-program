using UnityEngine;
using System.Collections;

public class RandomLineGenerator : MonoBehaviour
{
    public float areaWidth = 8f;  // 区域宽度
    public float areaHeight = 10f; // 区域高度
    public float lineLength = 18f; // 线的长度
    public float lineDuration = 1f; // 线显示的持续时间
    public float increaseInterval = 4.5f; // 增加线条数量的时间间隔
    public float lineOffset = 0.1f;  // 旁边线条的偏移距离
    private int linesPerSecond = 1; // 每秒生成的初始线条数量
    private bool isGenerating = false; // 是否正在生成线条
    private Coroutine lineCoroutine; // 用于控制生成线条的协程
    private GameObject[] currentLines = new GameObject[1000]; 
    private int a;
    public man1 man2;

    void Start()
    {
        // 初始设置
    }
    
    public void StartGeneratingLines()
    {
        a = 0;
        linesPerSecond = 1; 
        isGenerating = true;
        lineCoroutine = StartCoroutine(GenerateRandomLines());
    }

    public void StopGeneratingLines()
    {
        if (isGenerating)
        {
            isGenerating = false;
            if (lineCoroutine != null)
            {
                StopCoroutine(lineCoroutine);
                lineCoroutine = null;
            }
        }
    }

    private IEnumerator GenerateRandomLines()
    {
        while (isGenerating)
        {
            linesPerSecond = 1 + man2.score;

            // 每秒生成多条线条
            for (int i = 0; i < linesPerSecond; i++)
            {
                GenerateLineSequence();
            }

            if (man2.score >= 5)
            {
                StopGeneratingLines(); 
            }

            // 每3秒生成一次
            yield return new WaitForSeconds(3f);
        }
    }

    private void GenerateLineSequence()
    {
        // 生成随机位置
        Vector3 randomPoint = new Vector3(
            Random.Range(-areaWidth / 2f, areaWidth / 2f),
            Random.Range(-areaHeight / 2f, areaHeight / 2f),
            0f
        );

        // 生成随机角度
        float randomAngle = Random.Range(0f, 360f);
        float halfLineLength = lineLength / 2f;

        // 计算线的方向和两条线的偏移量
        Vector3 direction = Quaternion.Euler(0, 0, randomAngle) * Vector3.right;
        Vector3 offset = Quaternion.Euler(0, 0, randomAngle) * Vector3.up * lineOffset;

        // 计算第一条线的起点和终点
        Vector3 startPoint1 = randomPoint - direction * halfLineLength;
        Vector3 endPoint1 = randomPoint + direction * halfLineLength;

        // 计算第二条线的起点和终点（偏移）
        Vector3 startPoint2 = startPoint1 + offset;
        Vector3 endPoint2 = endPoint1 + offset;

        // 创建并设置第一条线
        currentLines[a] = CreateLine(startPoint1, endPoint1);
        a++;
        // 创建并设置第二条线
        currentLines[a] = CreateLine(startPoint2, endPoint2);
        a++;
        
        // 0.3秒后在两条线的中间生成一条线并删除之前的两条线
        StartCoroutine(GenerateMiddleLine(startPoint1, endPoint1, startPoint2, endPoint2));
    }
 private IEnumerator GenerateMiddleLine(Vector3 startPoint1, Vector3 endPoint1, Vector3 startPoint2, Vector3 endPoint2)
    {
        yield return new WaitForSeconds(0.3f);

        // 删除两条原始线条
        foreach (GameObject line in currentLines)
        {
            Destroy(line);
        }

        // 计算中间线的起点和终点（两条线的中点）
        Vector3 middleStart = (startPoint1 + startPoint2) / 2f;
        Vector3 middleEnd = (endPoint1 + endPoint2) / 2f;

        // 创建并设置中间线
        GameObject middleLine = CreateLine(middleStart, middleEnd);

        // 0.3秒后在中间线的上方生成一条较粗的红色线，并删除中间线
        StartCoroutine(GenerateRedLine(middleLine));
    }

    private IEnumerator GenerateRedLine(GameObject middleLine)
{
    yield return new WaitForSeconds(0.3f);

    // 获取中间线的起点和终点
    Vector3 middleStart = middleLine.GetComponent<LineRenderer>().GetPosition(0);
    Vector3 middleEnd = middleLine.GetComponent<LineRenderer>().GetPosition(1);

    // 删除白色的中间线
    Destroy(middleLine);

    // 创建一个较粗的红色线条并添加动画效果
    GameObject redLineObject = new GameObject("RedLine");
    LineRenderer redLineRenderer = redLineObject.AddComponent<LineRenderer>();
    redLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    redLineRenderer.startColor = Color.red;
    redLineRenderer.endColor = Color.red;
    redLineRenderer.positionCount = 2;
    redLineRenderer.SetPosition(0, middleStart);
    redLineRenderer.SetPosition(1, middleEnd);
    redLineRenderer.startWidth = 0.05f; 
    redLineRenderer.endWidth = 0.05f; 

    // 动画效果：从中间开始慢慢扩展
    float animationDuration = 0.5f; // 动画持续时间
    float targetWidth = 0.3f; 
    float elapsedTime = 0f;

    while (elapsedTime < animationDuration)
    {
        elapsedTime += Time.deltaTime;
        float width = Mathf.Lerp(0.05f, targetWidth, elapsedTime / animationDuration);
        redLineRenderer.startWidth = width;
        redLineRenderer.endWidth = width;
        yield return null;
    }





    // 持续0.5秒后销毁红线
    yield return new WaitForSeconds(0.5f);
    Destroy(redLineObject);
}



    private GameObject CreateLine(Vector3 startPoint, Vector3 endPoint)
    {
        // 创建一个新的空GameObject用于LineRenderer
        GameObject lineObject = new GameObject("RandomLine");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = 0.05f; 
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        return lineObject;
    }
}




