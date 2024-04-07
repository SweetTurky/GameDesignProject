using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Animator aiAnim;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, idleTime, sightDistance, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime;
    public bool walking, chasing;
    public Transform player;
    Transform currentDest;
    // Vector3 destLimit = new Vector3(-2, -2, 0);
    public Vector3 dest;
    int randNum;
    public Vector3 rayCastOffset;
    public string deathScene;
    public bool showSightDistanceGizmo = true; // Toggle visibility of sight distance gizmo
    public GameManager gameManager;
    public GameObject firstPersonController;

    void Start()
    {
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        // dest = dest - destLimit;
    }

    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                walking = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");
                chasing = true;
            }
        }
        if (chasing)
        {
             // Calculate the direction vector from NPC to player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            // Calculate the destination point slightly in front of the player
            Vector3 destinationPoint = player.position - directionToPlayer * catchDistance;
            //dest = player.position - destLimit;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            // Smoothly transition to chase animation
            aiAnim.SetFloat("locomotion", Mathf.Lerp(aiAnim.GetFloat("locomotion"), 1f, Time.deltaTime * 1f)); 
            float distance = Vector3.Distance(player.position, ai.transform.position);
            if (distance <= catchDistance)
            {
                ai.isStopped = true;
                walkSpeed = 0;
                chaseSpeed = 0;
                StartCoroutine("LoseGameAfterTimer");
                //player.gameObject.SetActive(false);
                // Set jumpscare animation
                aiAnim.Play("Cast03"); // Assuming "Death" animation is for jumpscare
                transform.LookAt(player.position);
                //StartCoroutine(deathRoutine());
                chasing = false;
            }
        }
        if (walking)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            // Smoothly transition to walk animation
            aiAnim.SetFloat("locomotion", Mathf.Lerp(aiAnim.GetFloat("locomotion"), 0.5f, Time.deltaTime * 1f)); 
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                //ai.speed = 0;
                aiAnim.Play("IdleBreak"); // Assuming "Idle" animation is for idle
                StopCoroutine("stayIdle");
                StartCoroutine("stayIdle");
                walking = false;
            }
        }
    }
    public IEnumerator LoseGameAfterTimer()
    {
        firstPersonController.GetComponent<FirstPersonController>().enabled = false;
        yield return new WaitForSeconds(7.5f);
        //walkSpeed = 2;
        //chaseSpeed = 4;
        gameManager.isGameLost = true;
        gameManager.LoseGame();
        yield break;
    }
    void OnDrawGizmosSelected()
    {
        if (showSightDistanceGizmo)
        {
            // Draw a wire sphere to represent sight distance
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sightDistance);
        }
    }

   IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }

    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }

    /*IEnumerator deathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }*/
}
