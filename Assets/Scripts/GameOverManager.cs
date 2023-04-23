using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private float previousSpeed = 0f;
    public bool isGameOver = false;

    [SerializeField] GameObject explosion;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject moveAlongManager;
    [SerializeField] GameObject logOut;
    [SerializeField] GameObject actionUI;

    void Update()
    {
        float currentSpeed = moveAlongManager.GetComponent<MoveAlongManager>().moveSpeed;

        if (previousSpeed > 0f && currentSpeed == 0f)
        {
            isGameOver = true;
            
            
        }
        
        if(isGameOver == true)
        {
            Debug.Log("Game Over!");
            explosion.SetActive(true);
            gameOverUI.SetActive(true);
            moveAlongManager.GetComponent<MoveAlongManager>().moveSpeed = 0f;
            moveAlongManager.SetActive(false);
            actionUI.SetActive(false);
            logOut.SetActive(true);
        }

        previousSpeed = currentSpeed;
    }
}
