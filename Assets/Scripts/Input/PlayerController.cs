using System;
using UI;
using UnityEngine;

namespace Input
{
    /// <summary>
    /// Controls the player character.
    /// </summary>
    /// <remarks>
    /// The action methods <see cref="Move"/>, <see cref="Look"/>, <see cref="Interact"/>, and <see cref="OpenMenu"/>
    /// will be called from <see cref="PlayerInputDelegate"/>.
    /// </remarks>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Tooltip("Reference to the in-game menu in this scene")]
        private InGameMenuHandler _inGameMenuHandler;
        
        /// <summary>
        /// Moves the player character on the x and z axis.
        /// </summary>
        /// <param name="moveDirection">vector of x and z coordinates with a clamped magnitude (0..1)</param>
        public void Move(Vector2 moveDirection)
        {
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
            if (_inGameMenuHandler == null)
                throw new InvalidOperationException(
                    $"{nameof(_inGameMenuHandler)} field in {nameof(InGameMenuHandler)} component on game object {gameObject.name} was not set!");
            
            _inGameMenuHandler.Show();
        }
    }
}