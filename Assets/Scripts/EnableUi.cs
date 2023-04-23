using UnityEngine;

public class EnableUi : MonoBehaviour
{
    public GameObject leaderBoard;
    private bool isLeaderboardActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isLeaderboardActive = !isLeaderboardActive;
            leaderBoard.SetActive(isLeaderboardActive);

        }
    }
}
