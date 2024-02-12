using System;
using UnityEngine;

namespace UI
{
    public class InventorySubMenuHandler : MonoBehaviour, ISubMenuHandler
    {
        /// <inheritdoc />
        public event Action NavigateBackRequested;

        /// <inheritdoc />
        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}