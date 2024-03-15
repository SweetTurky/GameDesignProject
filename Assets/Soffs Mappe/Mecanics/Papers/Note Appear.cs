using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour
{
    [SerializeField]
    private Image _noteImage;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
       {
            _noteImage.enabled = true;
       } 
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _noteImage.enabled = false;
        }
    }
}
