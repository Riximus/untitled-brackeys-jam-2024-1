using System.Collections;
using Input;
using UnityEngine;

namespace DefaultNamespace
{
    public class EndingTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform carpenter;
        [SerializeField] private Transform musician;
        [SerializeField] private Transform librarian;
        [SerializeField] private Transform player;
        [SerializeField] private float rotationSpeed = 2f; // Adjust rotation speed as needed

        public void Interact()
        {
            // Existing code for setting positions and rotations
            carpenter.position = new Vector3(-226.821f, 4.27f, 221.62f);
            carpenter.eulerAngles = new Vector3(0, -108.276f, 0);

            musician.position = new Vector3(-227.179f, 4.134f, 222.6f);
            musician.eulerAngles = new Vector3(0, -108.276f, 0);

            librarian.position = new Vector3(-227.2133f, 4.321f, 223.7123f);
            librarian.eulerAngles = new Vector3(0, -118.032f, 0);

            // Start rotating the player towards the musician
            StartCoroutine(RotateTowards(musician.position));
        }

        private IEnumerator RotateTowards(Vector3 targetPosition)
        {
            Vector3 directionToTarget = targetPosition - player.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            int counter = 0;
            
            while (Quaternion.Angle(player.rotation, targetRotation) > 1f && counter < 200)
            {
                player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                counter++;
                yield return null;
            }
        }
    }
}