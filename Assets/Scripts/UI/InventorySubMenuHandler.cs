using System;
using UnityEngine;

namespace UI
{
    public class InventorySubMenuHandler : MonoBehaviour, ISubMenuHandler
    {
        public event Action NavigateBackRequested;

        public void Cancel()
        {
        }
    }
}