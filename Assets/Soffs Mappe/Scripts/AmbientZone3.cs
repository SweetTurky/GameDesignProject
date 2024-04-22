using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
    public class AmbientZone3 : MonoBehaviour
    {
        [Tooltip("Cinemachine Path to follow")]
        public CinemachinePathBase m_Path;
        [Tooltip("Character to track")]
        public GameObject Player;
        public AudioSource ambientAudioSource; // Reference to the AudioSource component playing the ambient audio
        public float fadeInDuration = 1.0f; // Fade in duration in seconds
        public float fadeOutDuration = 1.0f; // Fade out duration in seconds

        float m_Position;
        private CinemachinePathBase.PositionUnits m_PositionUnits = CinemachinePathBase.PositionUnits.PathUnits;
        private bool playerInsideZone = false; // Track whether the player is inside the ambient zone

        void Start()
        {
            // Ensure that the ambient audio source is playing on awake
            ambientAudioSource.Play();
            ambientAudioSource.volume = 0f; // Start with zero volume
        }

        void Update()
        {
            // Find closest point to the player along the path
            SetCartPosition(m_Path.FindClosestPoint(Player.transform.position, 0, -1, 10));
            // Define vectors for the dot product
            Vector3 Sub = transform.position - Player.transform.position;
            Vector3 Spline = transform.right;
            // Attach object to player on enter
            if (Vector3.Dot(Sub, Spline) > 0)
            {
                transform.position = Player.transform.position;
                transform.rotation = Player.transform.rotation;

                // Check if the player just entered the zone
                if (!playerInsideZone)
                {
                    playerInsideZone = true;
                    StartCoroutine(FadeAudio(ambientAudioSource, 1f, fadeInDuration)); // Fade in the audio
                }
            }
            else
            {
                // Check if the player just exited the zone
                if (playerInsideZone)
                {
                    playerInsideZone = false;
                    StartCoroutine(FadeAudio(ambientAudioSource, 0f, fadeOutDuration)); // Fade out the audio
                }
            }
        }

        // Set cart's position to closest point
        void SetCartPosition(float distanceAlongPath)
        {
            m_Position = m_Path.StandardizeUnit(distanceAlongPath, m_PositionUnits);
            transform.position = m_Path.EvaluatePositionAtUnit(m_Position, m_PositionUnits);
            transform.rotation = m_Path.EvaluateOrientationAtUnit(m_Position, m_PositionUnits);
        }

        // Coroutine to fade the audio
        IEnumerator FadeAudio(AudioSource audioSource, float targetVolume, float duration)
        {
            float startVolume = audioSource.volume;
            float startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime) / duration);
                yield return null;
            }
            // Ensure the volume reaches the target value exactly
            audioSource.volume = targetVolume;
        }
    }
}
