using UnityEngine.UIElements;

namespace UI
{
    public class CreditsSubMenu : VisualElement
    {
        public CreditsSubMenu()
        {
            Add(new ScrollView { name = "content" });
            var mainButtonRow = new VisualElement { name = "main-button-row" };
            mainButtonRow.Add(new Button
            {
                text = "Back",
                parseEscapeSequences = true,
                displayTooltipWhenElided = true,
                name = "back-button"
            });
            Add(mainButtonRow);
        }

        /// <summary>
        /// Required class to allow <see cref="CreditsSubMenu"/> to be used as a visual element in UXML files.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<CreditsSubMenu>
        {
        }
    }
}