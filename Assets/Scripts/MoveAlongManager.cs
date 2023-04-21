using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongManager : MonoBehaviour
{
    private RandomXPosition RandomXPosition;
    private MoveAlongXAxis MoveAlongXAxis;
    public float moveSpeed;
    public float accelerationSpeed;
    

    // Start is called before the first frame update
    void Start()
    {
        RandomXPosition = FindObjectOfType<RandomXPosition>();
        MoveAlongXAxis = FindObjectOfType<MoveAlongXAxis>();
    }

    // Update is called once per frame
    void Update()
    {
        accelerationSpeed = Time.deltaTime * moveSpeed;

        float randomX = RandomXPosition.GetRandomX();
        float currentX = MoveAlongXAxis.GetCurrentXPosition();
        float absoluteX = Mathf.Abs(randomX - currentX);
        
        Debug.Log("Absolute X: " + absoluteX);

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if ((absoluteX) > 90)
            {
                Debug.Log("Slider: You missed the area");
                moveSpeed = 0f;

            }
            else
            {
                if (absoluteX < 90 && absoluteX >= 60) 
                {
                    moveSpeed = moveSpeed * 0.5f;
                    Debug.Log("Slider: You hit Orange!");
                }
                if (absoluteX < 60 && absoluteX >= 30) 
                {
                    
                    Debug.Log("Slider: You hit Yellow!");
                }
                if(absoluteX < 30)
                {
                    moveSpeed = moveSpeed * 2f;
                    Debug.Log("Slider: You hit Green!");
                }
            }
        }
        
    
        
    }

    /* Acceleration of car!!
     *  double initialSpeed = 0.0; // initial speed of the car
        double accelerationRate = 10.0; // acceleration rate of the car (m/s^2)
        double deaccelerationRate = -10.0; // deacceleration rate of the car (m/s^2)
        double timeDuration = 1.0; // time duration of each acceleration or deacceleration (s)

        double speedAfterFirstAcceleration = 2 * accelerationRate * timeDuration; // speed of the car after the first acceleration
        double speedAfterSecondAcceleration = 2 * speedAfterFirstAcceleration; // speed of the car after the second acceleration
        double speedAfterDeacceleration = speedAfterSecondAcceleration + deaccelerationRate * timeDuration; // speed of the car after the deacceleration to the normal speed

        double finalSpeed = initialSpeed + speedAfterDeacceleration; // final speed of the car

        Console.WriteLine("Final speed of the car is: " + finalSpeed + " m/s");
    }
     */
}
