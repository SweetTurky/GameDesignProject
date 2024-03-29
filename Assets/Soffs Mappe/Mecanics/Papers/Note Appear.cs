using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour
{
    [SerializeField]
    private Image _noteImage;
    private bool _isVisible = false;

    void Start()
    {
        // Ensure the document is initially hidden
        _noteImage.enabled = false;
    }

    void OnMouseDown()
    {
        // Toggle the visibility of the document
        _isVisible = !_isVisible;
        _noteImage.enabled = _isVisible;
    }
}

