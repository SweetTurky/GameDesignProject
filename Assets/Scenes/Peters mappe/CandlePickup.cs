using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlePickup : MonoBehaviour
{
    public ObjectGrabbable objectGrabbable;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(objectGrabbable != null)
            {
                objectGrabbable.IncreaseLightTimer();
            }    
            
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0.01f, Time.time * 100f, 0);
    }
}
