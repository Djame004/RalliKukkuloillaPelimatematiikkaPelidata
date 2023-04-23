using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongManager : MonoBehaviour
{
    private RandomXPosition RandomXPosition;
    private MoveAlongXAxis MoveAlongXAxis;
    public float moveSpeed = 0f;
    private float targetSpeed = 0f;
    private float accelerationSpeed = 0f;
    private bool isAccelerating = false;
    [SerializeField] GameObject ActionUI;

    // Start is called before the first frame update
    void Start()
    {
        RandomXPosition = FindObjectOfType<RandomXPosition>();
        MoveAlongXAxis = FindObjectOfType<MoveAlongXAxis>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAccelerating)
        {
            moveSpeed += accelerationSpeed;
            if (moveSpeed >= targetSpeed)
            {
                moveSpeed = targetSpeed;
                isAccelerating = false;
            }
        }

        float randomX = RandomXPosition.GetRandomX();
        float currentX = MoveAlongXAxis.GetCurrentXPosition();
        float absoluteX = Mathf.Abs(randomX - currentX);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((absoluteX) > 90)
            {
                Debug.Log("Slider: You missed the area");
                targetSpeed = 0f;
                accelerationSpeed = Mathf.Abs(moveSpeed) / 1f;
                isAccelerating = true;
                GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
                
                    gameOverManager.isGameOver = true;
                
            }
            else
            {
                if (absoluteX < 90 && absoluteX >= 60)
                {
                    targetSpeed = 25f;
                    accelerationSpeed = Mathf.Abs(moveSpeed - targetSpeed) / 1f;
                    isAccelerating = true;
                    Debug.Log("Slider: You hit Orange!");
                }
                if (absoluteX < 60 && absoluteX >= 30)
                {
                    targetSpeed = 50f;
                    accelerationSpeed = Mathf.Abs(moveSpeed - targetSpeed) / 1f;
                    isAccelerating = true;
                    Debug.Log("Slider: You hit Yellow!");
                }
                if (absoluteX < 30)
                {
                    targetSpeed = 100f;
                    accelerationSpeed = Mathf.Abs(moveSpeed - targetSpeed) / 1f;
                    isAccelerating = true;
                    Debug.Log("Slider: You hit Green!");
                }

                
            }
            ActionUI.SetActive(false);
        }
    }
}
