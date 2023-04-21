using UnityEngine;

public class MoveAlongXAxis : MonoBehaviour
{
    public RectTransform rectTransform;
    public float speed = 5f;
    private float maxX;
    private float minX;
    private int direction = 1;
    public float currentX;

    void Start()
    {
        maxX = rectTransform.rect.width / 2f - transform.localScale.x / 2f;
        minX = -maxX;
    }

    void Update()
    {
        float newX = transform.localPosition.x + speed * Time.deltaTime * direction;

        if (newX > maxX)
        {
            newX = maxX;
            direction = -1;
        }
        else if (newX < minX)
        {
            newX = minX;
            direction = 1;
        }

        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float currentX = transform.localPosition.x;
            Debug.Log("Current X position: " + currentX);
        }
    }

    public float GetCurrentXPosition()
    {
        return transform.localPosition.x;
    }

}
