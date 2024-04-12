using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEvent : MonoBehaviour
{
    public UnityEvent onEnterFromOneSide;
    public UnityEvent onEnterFromOppositeSide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x > transform.position.x)
            {
                // Player entered from one side
                onEnterFromOneSide.Invoke();
            }
            else
            {
                // Player entered from opposite side
                onEnterFromOppositeSide.Invoke();
            }
        }
    }
}

