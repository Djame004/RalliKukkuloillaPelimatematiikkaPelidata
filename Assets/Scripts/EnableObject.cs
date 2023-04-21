using UnityEngine;

public class EnableObject: MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        
    }

    void Update()
    {
        // Check if the L key is pressed
        if (Input.GetKeyDown(KeyCode.L))
        {
            obj.SetActive(true);

        }
    }
}
