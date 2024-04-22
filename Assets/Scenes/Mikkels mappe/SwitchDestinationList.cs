using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDestinationList : MonoBehaviour
{
   public List<Transform> firstSetDestinations;
    public List<Transform> secondSetDestinations;

    // Reference to the EnemyAI script to communicate destination changes
    public EnemyAI enemyAI;

    // Method to switch to the first set of destinations
    public void SwitchToFirstSet()
    {
        enemyAI.destinations = firstSetDestinations;
        enemyAI.currentDest = firstSetDestinations[0]; // Set initial destination to the first in the list
        Debug.Log("1st destinations set");
    }

    // Method to switch to the second set of destinations
    public void SwitchToSecondSet()
    {
        enemyAI.destinations = secondSetDestinations;
        enemyAI.currentDest = secondSetDestinations[0]; // Set initial destination to the first in the list
        Debug.Log("2nd destinations set");
    }
}
