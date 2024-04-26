using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_NPCFollowScary : MonoBehaviour
{
    public Transform playerTransform;
    public float rotationDuration = 1.0f; // Time taken for the NPC to fully turn

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Follow the player
        agent.destination = playerTransform.position;

        // Smoothly rotate towards the player
        StartCoroutine(LookAtSmoothly(playerTransform.position));
    }

    IEnumerator LookAtSmoothly(Vector3 targetPosition)
    {
        Quaternion currentRot = transform.rotation;
        Quaternion newRot = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

        float elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(currentRot, newRot, elapsedTime / rotationDuration);
            yield return null;
        }
    }
}