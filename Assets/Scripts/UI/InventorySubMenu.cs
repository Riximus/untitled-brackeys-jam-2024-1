using UnityEngine.UIElements;
using Util;

namespace UI
{
    /// <summary>
    /// Displays a list of all obtained facts.
    /// </summary>
    public class InventorySubMenu : VisualElement
    {
        public InventorySubMenu()
        {
            UxmlUtil.LoadUxml(this);
        }

        /// <summary>
        /// Required class to allow <see cref="InventorySubMenu"/> to be used as a visual element in UXML files.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<InventorySubMenu>
        {
        }
    }
}
