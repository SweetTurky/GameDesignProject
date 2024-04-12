using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
    public class AmbientZone : MonoBehaviour
    {
        [Tooltip("Cinemachine Path to follow")]
        public CinemachinePathBase m_Path;
        [Tooltip("Character to track")]
        public GameObject Player;
        public AudioSource ambientAudioSource; // Reference to the AudioSource component playing the ambient audio

        float m_Position;
        private CinemachinePathBase.PositionUnits m_PositionUnits = CinemachinePathBase.PositionUnits.PathUnits;

        // Variable to track whether the player is inside the ambient zone
        bool playerInsideZone = false;

        void Start()
        {
            // Ensure that the ambient audio source is not playing when the game starts
            ambientAudioSource.Stop();
            Debug.Log("Audio source stopped on start.");
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
                    // Play the ambient audio
                    ambientAudioSource.Play();
                    Debug.Log("Audio source played on zone enter.");
                }
            }
            else
            {
                // Check if the player just exited the zone
                if (playerInsideZone)
                {
                    playerInsideZone = false;
                    // Stop the ambient audio
                    ambientAudioSource.Stop();
                    Debug.Log("Audio source stopped on zone exit.");
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
    }
}
