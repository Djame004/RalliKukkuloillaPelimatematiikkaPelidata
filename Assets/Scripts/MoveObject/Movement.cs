using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{

    public Camera camera;
    private RaycastHit hit;
    private string groundTag = "Ground";
    private float speed;
    public float normalSpeed;
    public float boostedSpeed;
    public float maxSpeed;
    public float boostCooldown;

    private NavMeshAgent agent;
    
    void Start()
    {
        normalSpeed = speed;
        
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            speed = boostedSpeed;
            

            if(Physics.Raycast(ray, out hit, Mathf.Infinity)) 
            {
                if(hit.collider.CompareTag(groundTag))
                {
                    agent.SetDestination(hit.point);
                    
                }
            }
        }
    }
    IEnumerator SpeedDuration()
    {
        yield return new WaitForSeconds(boostCooldown);
        speed = normalSpeed;
    }
}
