using UnityEngine.UIElements;
using Util;

namespace UI
{
    public class CreditsSubMenu : VisualElement
    {
        public CreditsSubMenu()
        {
            UxmlUtil.LoadUxml(this);
        }
        
        /// <summary>
        /// Required class to allow <see cref="CreditsSubMenu"/> to be used as a visual element in UXML files.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<CreditsSubMenu>
        {
        }
    }
}