using UnityEngine;

public class RandomXPosition : MonoBehaviour
{
    public RectTransform rectTransform;
    public float randomX;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            randomX = Random.Range(rectTransform.rect.min.x, rectTransform.rect.max.x);
            transform.localPosition = new Vector3(randomX, transform.localPosition.y, transform.localPosition.z);
            Debug.Log("Random X position: " + randomX);
        }
    }

    public float GetRandomX()
    {
        return randomX;
    }
}

