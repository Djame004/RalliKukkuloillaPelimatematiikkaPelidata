using UnityEngine;

public class PlaneTrigger : MonoBehaviour
{
    public GameObject targetObject;
    public float triggerDistance = 5f;
    public GameObject actionUI;
    public bool isInside = false;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);

        if (distance < triggerDistance && !isInside)
        {
            // Target object is inside trigger area
            isInside = true;
            Debug.Log(targetObject.name + " entered trigger area.");

            RandomXPosition randomXPosition = targetObject.GetComponent<RandomXPosition>();
            if (randomXPosition != null)
            {
                randomXPosition.CalculateRandomXPosition();
            }

            // Activate the ActionUI game object
            if (actionUI != null)
            {
                if (actionUI.activeSelf)
                {
                    GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
                   
                        gameOverManager.isGameOver = true;
                      
                }
                else
                {
                    actionUI.SetActive(true);
                }
            }
        }
        else if (distance >= triggerDistance && isInside)
        {
            // Target object is outside trigger area
            isInside = false;
            Debug.Log(targetObject.name + " exited trigger area.");
        }
    }
}
