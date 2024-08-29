using UnityEngine;
using System.Collections;

public class RandomLineGenerator : MonoBehaviour
{
    public float areaWidth = 8f;
    public float areaHeight = 10f;
    public float lineLength = 18f;
    public float lineDuration = 1f;
    public float increaseInterval = 5f;
    public float lineOffset = 0.1f;
    private float nextTimeToIncrease = 0f;
    private int linesPerSecond = 1;
    private bool isGenerating = false;
    private Coroutine lineCoroutine;
    private int a = 0;
    private GameObject[] currentLines = new GameObject[2];

    void Start()
    {
        linesPerSecond = 1;
    }

    public void StartGeneratingLines()
    {
        if (!isGenerating)
        {
            isGenerating = true;
            lineCoroutine = StartCoroutine(GenerateRandomLines());
        }
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
        float startTime = Time.time;

        while (isGenerating)
        {
            for (int i = 0; i < linesPerSecond; i++)
            {
                if (currentLines[0] == null && currentLines[1] == null) // 确保没有未被删除的线条
                {
                    GenerateLineSequence();
                }
            }

            if (Time.time >= nextTimeToIncrease)
            {
                linesPerSecond++;
                nextTimeToIncrease = Time.time + increaseInterval;
            }

            if (Time.time - startTime >= 25f)
            {
                StopGeneratingLines();
            }

            yield return new WaitForSeconds(3f);
        }
    }

    private void GenerateLineSequence()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-areaWidth / 2f, areaWidth / 2f),
            Random.Range(-areaHeight / 2f, areaHeight / 2f),
            0f
        );

        float randomAngle = Random.Range(0f, 360f);
        float halfLineLength = lineLength / 2f;

        Vector3 direction = Quaternion.Euler(0, 0, randomAngle) * Vector3.right;
        Vector3 offset = Quaternion.Euler(0, 0, randomAngle) * Vector3.up * lineOffset;

        Vector3 startPoint1 = randomPoint - direction * halfLineLength;
        Vector3 endPoint1 = randomPoint + direction * halfLineLength;
        Vector3 startPoint2 = startPoint1 + offset;
        Vector3 endPoint2 = endPoint1 + offset;

        currentLines[0] = CreateLine(startPoint1, endPoint1);
        currentLines[1] = CreateLine(startPoint2, endPoint2);

        StartCoroutine(GenerateMiddleLine(startPoint1, endPoint1, startPoint2, endPoint2));
    }

    private IEnumerator GenerateMiddleLine(Vector3 startPoint1, Vector3 endPoint1, Vector3 startPoint2, Vector3 endPoint2)
    {
        yield return new WaitForSeconds(0.3f);

        foreach (GameObject line in currentLines)
        {
            Destroy(line);
        }

        currentLines[0] = null;
        currentLines[1] = null;

        Vector3 middleStart = (startPoint1 + startPoint2) / 2f;
        Vector3 middleEnd = (endPoint1 + endPoint2) / 2f;

        GameObject middleLine = CreateLine(middleStart, middleEnd);
        StartCoroutine(GenerateRedLine(middleLine));
    }

    private IEnumerator GenerateRedLine(GameObject middleLine)
    {
        yield return new WaitForSeconds(0.3f);

        Vector3 middleStart = middleLine.GetComponent<LineRenderer>().GetPosition(0);
        Vector3 middleEnd = middleLine.GetComponent<LineRenderer>().GetPosition(1);

        Destroy(middleLine);

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

        float animationDuration = 0.5f;
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

        BoxCollider2D collider = redLineObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        redLineObject.AddComponent<LineCollisionHandler>();

        yield return new WaitForSeconds(0.5f);
        Destroy(redLineObject);
    }

    private GameObject CreateLine(Vector3 startPoint, Vector3 endPoint)
    {
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

// LineCollisionHandler class remains unchanged.


// 创建一个新脚本LineCollisionHandler来检测碰撞
public class LineCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            man1 man = other.GetComponent<man1>();
            if (man != null)
            {
                man.Modifyhp(-1); 
                
            }
        }
    }
}

