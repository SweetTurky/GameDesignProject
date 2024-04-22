using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    public EnemyAI enemyAI;
    public AudioSource audioSourceChasing;

    // Update is called once per frame
    void Update()
    {
        if (enemyAI.chasing == true)
        {
            
        }
    }
}
