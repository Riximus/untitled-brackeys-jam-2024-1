using System;
using System.Diagnostics.CodeAnalysis;
using UI;
using UnityEngine;
using Util;

namespace Input
{
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
        [SerializeField, Tooltip("Reference to the in-game menu in this scene")]
        private InGameMenuHandler inGameMenuHandler;
        [SerializeField, DisallowNull, MaybeNull]
        private PauseManager pauseManager;
        [SerializeField, Tooltip("Movement speed of the player")]
        private float moveSpeed;
        [SerializeField, Tooltip("Maximum velocity the player can have when moving"), Min(0f)]
        private float maxVelocity;
        [DisallowNull, NotNull] private Rigidbody _rigidbody = default!;
        private Vector2 _moveDirection;
        
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
            // TODO: The rotation using the y axis is quite hard: the camera moves up and down, but is limited.
            //       That way the camera can never be upside down.
            //       Additionally, the camera should zoom into and out of the player character.
            //       Good tests are: can I look at something in the sky? can I look at something on the table?
        }

        /// <summary>
        /// Interacts with the nearest object or character to the player character in the direction they're looking.
        /// </summary>
        public void Interact()
        {
            // TODO: I think Vector3.Cross() can help identify which object is closest to "forward".
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
            if (inGameMenuHandler == null)
                throw new InvalidOperationException(
                    $"{nameof(inGameMenuHandler)} field in {nameof(PlayerController)} component on game object {gameObject.name} was not set!");
            if (pauseManager == null)
                throw new InvalidOperationException(
                    $"{nameof(pauseManager)} field in {nameof(PlayerController)} component on game object {gameObject.name} was not set!");

            if (pauseManager.IsPaused)
                inGameMenuHandler.Hide();
            else
                inGameMenuHandler.Show();
        }

        private void Awake()
        {
            _rigidbody = this.RequireComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var moveVelocity = Time.fixedDeltaTime * moveSpeed * new Vector3(_moveDirection.x, 0f, _moveDirection.y);
            moveVelocity = Vector3.ClampMagnitude(moveVelocity, maxVelocity);
            _rigidbody.AddForce(moveVelocity, ForceMode.VelocityChange);
        }
    }
}