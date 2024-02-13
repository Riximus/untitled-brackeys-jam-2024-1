﻿using UnityEditor.PackageManager;
using UnityEngine;

namespace Input
{
    interface IInteractable
    {
        void Interact();
    }
    
    /// <summary>
    /// Controls the player character.
    /// </summary>
    /// <remarks>
    /// The action methods <see cref="Move"/>, <see cref="Look"/>, <see cref="Interact"/>, and <see cref="OpenMenu"/>
    /// will be called from <see cref="PlayerInputDelegate"/>.
    /// </remarks>
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera Settings")]
        public float sensX = 400.0f;
        public float sensY = 400.0f;
        public Transform orientation;
        public Transform playerCamera;
        [Range(30f, 90f)]
        public float cameraAngle = 60f;
        
        [Header("Movement Settings")]
        public float moveSpeed;
        
        [Header("Interaction Settings")]
        public float interactionRange;
        
        private Rigidbody _rigidBody;
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _rigidBody = GetComponent<Rigidbody>();
            _rigidBody.freezeRotation = true;
        }
        
        /// <summary>
        /// Moves the player character on the x and z axis.
        /// </summary>
        /// <remarks>
        /// Unlike the other input event methods, this method is called once per frame when a move input is active,
        /// instead of when the input is performed.
        /// </remarks>
        /// <param name="moveDirection">vector of x and z coordinates with a clamped magnitude (0..1)</param>
        public void Move(Vector2 moveDirection)
        {
            Vector3 move = new Vector3(moveDirection.x, 0, moveDirection.y);
            move = orientation.forward * move.z + orientation.right * move.x;
            float forceMagnitude = moveSpeed * 10f * Time.deltaTime; // Calculate the scalar value first
            _rigidBody.AddForce(move.normalized * forceMagnitude, ForceMode.Force);
        }

        /// <summary>
        /// Moves the camera around the player character.
        /// </summary>
        /// <remarks>
        /// The x axis of <paramref name="lookDirectionDelta"/> lets the camera rotate around the y axis of the player
        /// character.
        /// </remarks>
        /// <param name="lookDirectionDelta">contains the camera rotation around the y axis and height</param>
        public void Look(Vector2 lookDirectionDelta)
        {
            // TODO: The rotation using the y axis is quite hard: the camera moves up and down, but is limited.
            //       That way the camera can never be upside down.
            //       Additionally, the camera should zoom into and out of the player character.
            //       Good tests are: can I look at something in the sky? can I look at something on the table?

            var eulerAngles = transform.eulerAngles;
            // Apply the rotation to the camera
            float desiredRotationX = playerCamera.localEulerAngles.x - lookDirectionDelta.y;
            
            if(desiredRotationX > 180f) desiredRotationX -= 360f;
            
            float playerEulerX = Mathf.Clamp(desiredRotationX, -cameraAngle, cameraAngle);
            playerCamera.localRotation = Quaternion.Euler(playerEulerX, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, eulerAngles.y + lookDirectionDelta.x, 0f);
        }

        /// <summary>
        /// Interacts with the nearest object or character to the player character in the direction they're looking.
        /// </summary>
        public void Interact()
        {
            // TODO: I think Vector3.Cross() can help identify which object is closest to "forward".
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObject))
                {
                    interactObject.Interact();
                }
            }
        }

        /// <summary>
        /// Opens the in-game menu.
        /// </summary>
        /// <remarks>
        /// For convenience, the player character can open the in-game menu, because it works well with the
        /// <see cref="PlayerInputDelegate"/>.
        /// </remarks>
        public void OpenMenu()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}