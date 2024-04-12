using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
    public class AmbientZone2 : MonoBehaviour
    {
        [Tooltip("Cinemachine Path to follow")]
        public CinemachinePathBase m_Path;
        [Tooltip("Character to track")]
        public GameObject Player;
        public AudioSource ambientAudioSource; // Reference to the AudioSource component playing the ambient audio
        public float fadeOutDuration = 1.0f; // Duration of fade-out in seconds

        float m_Position;
        private CinemachinePathBase.PositionUnits m_PositionUnits = CinemachinePathBase.PositionUnits.PathUnits;
        private bool wasInsideZone = false; // Track the previous zone state

        void Start()
        {
            // Ensure that the ambient audio source is not playing when the game starts
            ambientAudioSource.Stop();
        }

        void Update()
        {
            // Find closest point to the player along the path
            SetCartPosition(m_Path.FindClosestPoint(Player.transform.position, 0, -1, 10));
            // Define vectors for the dot product
            Vector3 Sub = transform.position - Player.transform.position;
            Vector3 Spline = transform.right;
            // Check if the player is inside the zone
            bool insideZone = Vector3.Dot(Sub, Spline) > 0;

            // Check if the zone state has changed
            if (insideZone != wasInsideZone)
            {
                if (insideZone)
                {
                    // Player entered the zone
                    FadeOutAudio();
                }
                else
                {
                    // Player exited the zone
                    ambientAudioSource.Play();
                }
                wasInsideZone = insideZone;
            }
        }

        // Set cart's position to closest point
        void SetCartPosition(float distanceAlongPath)
        {
            m_Position = m_Path.StandardizeUnit(distanceAlongPath, m_PositionUnits);
            transform.position = m_Path.EvaluatePositionAtUnit(m_Position, m_PositionUnits);
            transform.rotation = m_Path.EvaluateOrientationAtUnit(m_Position, m_PositionUnits);
        }

        // Coroutine to fade out the audio
        void FadeOutAudio()
        {
            StartCoroutine(FadeOutCoroutine());
        }

        IEnumerator FadeOutCoroutine()
        {
            float startVolume = ambientAudioSource.volume;
            float startTime = Time.time;
            while (Time.time < startTime + fadeOutDuration)
            {
                ambientAudioSource.volume = Mathf.Lerp(startVolume, 0, (Time.time - startTime) / fadeOutDuration);
                yield return null;
            }
            ambientAudioSource.Stop();
            ambientAudioSource.volume = startVolume; // Reset volume to its original value
        }
    }
}
