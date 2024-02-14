using UnityEngine.UIElements;
using Util;

namespace UI
{
    public class OptionsSubMenu : VisualElement
    {
        public OptionsSubMenu()
        {
            UxmlUtil.LoadUxml(this);
        }
        
        /// <summary>
        /// Required nested class to make this visual element usable in UXML files.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<OptionsSubMenu>
        {
        }
    }
}