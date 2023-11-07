using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserPathing : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Camera userCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            agent.SetDestination(new Vector3(-22,this.transform.position.y, 33));
        }
    }
}
