using System;
using UnityEditor.PackageManager;
using System.Diagnostics.CodeAnalysis;
using Dialogue;
using UI;
using UnityEngine;
using Util;

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
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Tooltip("Reference to the in-game menu in this scene"), DisallowNull, NotNull]
        private InGameMenuHandler inGameMenuHandler = default!;
        [SerializeField, DisallowNull, NotNull]
        private PauseManager pauseManager = default!;
        [SerializeField, Tooltip("Movement speed of the player")]
        private float moveSpeed = 10;
        [SerializeField, Tooltip("Maximum velocity the player can have when moving"), Min(0f)]
        private float maxVelocity = 2;
        [DisallowNull, NotNull] private Rigidbody _rigidBody = default!;
        private Vector2 _moveDirection;
        
        [Header("Camera Settings")]
        public float cameraSensitivityX = 1;
        public float cameraSensitivityY = 1;
        public Transform playerCamera;
        [Range(30f, 90f)]
        public float cameraAngle = 60f;
        
        [Header("Interaction Settings")]
        public float interactionRange = 3;
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
            _moveDirection = moveDirection;
        }

        /// <summary>
        /// Stops movement, until <see cref="Move"/> is called again.
        /// </summary>
        /// <remarks>
        /// This method is required because of the way the input system is handled in this project.
        /// </remarks>
        public void StopMoving()
        {
            Move(Vector2.zero);
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
            lookDirectionDelta.x *= cameraSensitivityX;
            lookDirectionDelta.y *= cameraSensitivityY;
            
            var eulerAngles = transform.eulerAngles;
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
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObject))
                {
                    DialogueTrigger[] dialogueTriggers = hitInfo.collider.gameObject.GetComponents<DialogueTrigger>();
                    
                    if (dialogueTriggers.Length > 0)
                    {
                        foreach (var dialogueTrigger in dialogueTriggers)
                        {
                            if (!dialogueTrigger.hasDialogueTriggered)
                            {
                                dialogueTrigger.Interact();
                                break;
                            }
                        }
                    }
                    else
                    {
                        interactObject.Interact();
                    }
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
            if (pauseManager.IsPaused)
                inGameMenuHandler.Hide();
            else
                inGameMenuHandler.Show();
        }

        private void Awake()
        {
            _rigidBody = this.RequireComponent<Rigidbody>();
            if (inGameMenuHandler == null)
                throw new InvalidOperationException(
                    $"{nameof(inGameMenuHandler)} field in {nameof(PlayerController)} component on game object {gameObject.name} was not set!");
            if (pauseManager == null)
                throw new InvalidOperationException(
                    $"{nameof(pauseManager)} field in {nameof(PlayerController)} component on game object {gameObject.name} was not set!");
        }

        private void FixedUpdate()
        {
            if (pauseManager.IsPaused)
            {
                if (!_rigidBody.IsSleeping())
                    _rigidBody.Sleep();

                return;
            }

            if (_rigidBody.IsSleeping())
                _rigidBody.WakeUp();

            var moveVelocity = Time.fixedDeltaTime * moveSpeed * new Vector3(_moveDirection.x, 0f, _moveDirection.y);
            moveVelocity = Vector3.ClampMagnitude(moveVelocity, maxVelocity);
            _rigidBody.AddForce(moveVelocity, ForceMode.VelocityChange);
        }
    }
}