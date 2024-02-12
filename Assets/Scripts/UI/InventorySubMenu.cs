using UnityEngine.UIElements;
using Util;

namespace UI
{
    public class InventorySubMenu : VisualElement
    {
        public InventorySubMenu()
        {
            UxmlUtil.LoadUxml(this);
        }

        public new class UxmlFactory : UxmlFactory<InventorySubMenu>
        {
        }
    }
}
