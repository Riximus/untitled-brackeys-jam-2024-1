using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Input
{
    /// <summary>
    /// This component delegates the input to the appropriate methods on <see cref="PlayerInput"/>.
    /// </summary>
    /// <remarks>
    /// The component assumes that the <see cref="PlayerInput"/> and <see cref="PlayerController"/> components are
    /// present on the same <see cref="GameObject"/> during <see cref="Awake"/> and remain there during the object's
    /// lifetime. 
    /// </remarks>
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInputDelegate : MonoBehaviour
    {
        private const string MoveAction = nameof(Actions.PlayerActions.Move);
        private const string LookAction = nameof(Actions.PlayerActions.Look);
        private const string InteractAction = nameof(Actions.PlayerActions.Interact);
        private const string OpenMenuAction = nameof(Actions.PlayerActions.OpenMenu);
        [DisallowNull, NotNull] private PlayerInput _playerInput = default!;
        [DisallowNull, NotNull] private PlayerController _playerController = default!;

        private void DelegateInput(InputAction.CallbackContext callbackContext)
        {
            if (!callbackContext.performed)
                return;

            if (callbackContext.action == null)
            {
                Debug.LogWarning("Player input with no action!", this);
                return;
            }

            var actionName = callbackContext.action.name;
            
            switch (actionName)
            {
                case MoveAction:
                {
                    var moveDirection = callbackContext.ReadValue<Vector2>();
                    _playerController.Move(moveDirection);
                    break;
                }
                case LookAction:
                {
                    var lookDirectionDelta = callbackContext.ReadValue<Vector2>();
                    _playerController.Look(lookDirectionDelta);
                    break;
                }
                case InteractAction:
                {
                    _playerController.Interact();
                    break;
                }
                case OpenMenuAction:
                {
                    _playerController.OpenMenu();
                    break;
                }
                default:
                {
                    Debug.LogWarningFormat("Unrecognized action {0}!", actionName);
                    break;
                }
            }
        }

        private void Awake()
        {
            _playerInput = this.RequireComponent<PlayerInput>();
            _playerController = this.RequireComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _playerInput.onActionTriggered += DelegateInput;
        }

        private void OnDisable()
        {
            _playerInput.onActionTriggered -= DelegateInput;
        }
    }
}
