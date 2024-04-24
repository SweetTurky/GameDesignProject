using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public AudioClip[] jumpscareClips;
    public AudioClip[] playerSpottedClips;
    public Animator aiAnim;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, idleTime, sightDistance, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime;
    public bool walking, chasing;
    // Define a flag to track whether the jumpscare sound has been played
    private bool jumpscareSoundPlayed = false;
    public bool playerSpotted = false;
    public Transform player;
    public Transform currentDest;
    public Vector3 teleportPoint1;
    public Vector3 teleportPoint2;
    //Vector3 destLimit = new Vector3(-1, -1, 0);
    public Vector3 dest;
    int randNum;
    public Vector3 rayCastOffset;
    public string deathScene;
    public bool showSightDistanceGizmo = true; // Toggle visibility of sight distance gizmo
    public GameManager gameManager;
    public AudioSource audioSourceJumpscare;
    public AudioSource audioSourceSpotted;
    public float minIntervalBetweenClips;
    public float maxIntervalBetweenClips;


    void Start()
    {
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        //dest = dest - destLimit;
    }

    public void TeleportToPoint1()
    {
        ai.Warp(teleportPoint1);
    }
    public void TeleportToPoint2()
    {
        ai.Warp(teleportPoint2);
    }

    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        int lanternLayerMask = ~LayerMask.GetMask("Lantern");

        // Calculate the dot product between the direction vector and the NPC's forward vector
        float dotProduct = Vector3.Dot(direction, transform.forward);

        // Check if the dot product is positive (indicating that the player is in front of the NPC)
        if (dotProduct > 0f)
        {
            if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance, lanternLayerMask))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    walking = false;
                    StopCoroutine("stayIdle");
                    StopCoroutine("chaseRoutine");
                    StartCoroutine("chaseRoutine");
                    chasing = true;

                    // Play a random spotted clip when NPC spots the player
                    /*if (playerSpottedClips.Length > 0)
                    {
                        int randomClipIndex = Random.Range(0, playerSpottedClips.Length);
                        AudioClip clip = playerSpottedClips[randomClipIndex];
                        AudioSource.PlayClipAtPoint(clip, transform.position, 1f);
                    }*/


                }
            }
        }

        if (chasing == true)
        {
            if (!jumpscareSoundPlayed && jumpscareClips.Length > 0)
            {
                StartCoroutine(PlayRandomSpottedClips());
                int randomIndex = Random.Range(0, jumpscareClips.Length);
                audioSourceJumpscare.clip = jumpscareClips[randomIndex];
                audioSourceJumpscare.Play();
                jumpscareSoundPlayed = true;
            }

            // Calculate the direction vector from NPC to player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            // Calculate the destination point slightly in front of the player
            Vector3 destinationPoint = player.position - directionToPlayer * catchDistance;
            dest = player.position/* - destLimit*/;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            // Smoothly transition to chase animation
            aiAnim.SetFloat("locomotion", Mathf.Lerp(aiAnim.GetFloat("locomotion"), 1f, Time.deltaTime * 1f));
            float distance = Vector3.Distance(player.position, ai.transform.position);
            if (distance <= catchDistance)
            {
                transform.LookAt(player.position);
                ai.destination = transform.position;
                //ai.isStopped = true;
                //walkSpeed = 0;
                //chaseSpeed = 0;
                //player.gameObject.SetActive(false);
                // Set jumpscare animation
                aiAnim.Play("Cast03"); // Assuming "Death" animation is for jumpscare
                //StartCoroutine(deathRoutine());
                chasing = false;
                StartCoroutine("LoseGameAfterTimer");
            }
        }

        if (walking == true)
        {
            jumpscareSoundPlayed = false;
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            // Smoothly transition to walk animation
            aiAnim.SetFloat("locomotion", Mathf.Lerp(aiAnim.GetFloat("locomotion"), 1f, Time.deltaTime * 1f));
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                //ai.speed = 0;
                StopCoroutine("stayIdle");
                StartCoroutine("stayIdle");
                aiAnim.Play("IdleBreak"); // Assuming "Idle" animation is for idle
                walking = false;
            }
        }
    }

    public IEnumerator LoseGameAfterTimer()
    {
        player.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(4f);
        //walkSpeed = 2;
        //chaseSpeed = 4;
        gameManager.isGameLost = true;
        gameManager.LoseGame();
        yield break;
    }

    IEnumerator PlayRandomSpottedClips()
    {
        // Pick a random clip from playerSpottedClips
        int randomClipIndex = Random.Range(0, playerSpottedClips.Length);
        AudioClip clip = playerSpottedClips[randomClipIndex];

        // Play the clip as one-shot at NPC's current position
        audioSourceSpotted.PlayOneShot(clip, 1f);
        playerSpotted = true;

        // Wait for a random interval before playing the next clip
        float interval = Random.Range(minIntervalBetweenClips, maxIntervalBetweenClips);
        yield return new WaitForSeconds(interval);
        playerSpotted = false;
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

        // Keep track of the previously chosen destination
        int previousDestIndex = randNum;

        // Find a new destination different from the previous one
        do
        {
            randNum = Random.Range(0, destinations.Count);
        } while (randNum == previousDestIndex);

        // Set the new destination
        currentDest = destinations[randNum];

        // Set walking to true after choosing the destination
        walking = true;
    }

    /*IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }*/

    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);



        /*// Play a random sound clip
        if (playerSpottedClips.Length > 0)
        {
            int randomClipIndex = Random.Range(0, playerSpottedClips.Length);
            AudioClip clip = playerSpottedClips[randomClipIndex];
            audioSourceSpotted.PlayOneShot(clip, 1f);
        }

        // Wait between 2-4 seconds
        float waitTime = Random.Range(2f, 4f);
        yield return new WaitForSeconds(waitTime);*/


        chasing = false;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        walking = true;
    }


    /*IEnumerator deathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }*/
}
