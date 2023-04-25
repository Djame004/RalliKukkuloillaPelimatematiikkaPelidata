using UnityEngine;

public class RandomXPosition : MonoBehaviour
{
    public RectTransform rectTransform;
    public float randomX;
    public PlaneTrigger planeTrigger;
    public PlaneTrigger planeTrigger2;
    public PlaneTrigger planeTrigger3;
    private bool hasRandomPosition = false;

    void Start()
    {
    }

    void Update()
    {
        if (planeTrigger.isInside && !hasRandomPosition)
        {
            CalculateRandomXPosition();
            Debug.Log("Random X position: " + randomX);
            hasRandomPosition = true;
        }
        if (planeTrigger2.isInside && !hasRandomPosition)
        {
            CalculateRandomXPosition();
            Debug.Log("Random X position: " + randomX);
            hasRandomPosition = true;
        }
        if (planeTrigger3.isInside && !hasRandomPosition)
        {
            CalculateRandomXPosition();
            Debug.Log("Random X position: " + randomX);
            hasRandomPosition = true;
        }


        if (!planeTrigger.isInside && !planeTrigger2.isInside && !planeTrigger3.isInside)
        {
            hasRandomPosition = false;
        }
    }

    public void CalculateRandomXPosition()
    {
        randomX = Random.Range(rectTransform.rect.min.x, rectTransform.rect.max.x);
        transform.localPosition = new Vector3(randomX, transform.localPosition.y, transform.localPosition.z);
    }

    public float GetRandomX()
    {
        return randomX;
    }
}
