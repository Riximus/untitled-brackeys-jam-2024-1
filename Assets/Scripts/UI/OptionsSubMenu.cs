using UnityEngine.UIElements;

namespace UI
{
    public class OptionsSubMenu : VisualElement
    {
        public OptionsSubMenu()
        {
            var content = new ScrollView { name = "content", style = { flexGrow = 1 } };
            Add(content);
            var container = new VisualElement { style = { flexGrow = 1 } };
            content.Add(container);
            var masterVolumeSlider = new Slider
                { label = "Master Volume", highValue = 1, name = "master-volume-slider" };
            container.Add(masterVolumeSlider);
            var mainButtonRow = new VisualElement
            {
                name = "main-button-row",
                style =
                {
                    flexGrow = 0, justifyContent = new StyleEnum<Justify>(Justify.Center),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row)
                }
            };
            Add(mainButtonRow);
            mainButtonRow.Add(new Button
            {
                text = "Save Changes", parseEscapeSequences = true, displayTooltipWhenElided = true,
                name = "save-changes-button",
                style = { minWidth = 200 }
            });
            mainButtonRow.Add(new Button
            {
                text = "Discard Changes", parseEscapeSequences = true, displayTooltipWhenElided = true,
                name = "discard-changes-button",
                style = { minWidth = 200 }
            });
            mainButtonRow.Add(new Button
            {
                text = "Back", parseEscapeSequences = true, displayTooltipWhenElided = true, name = "back-button",
                style = { minWidth = 200 }
            });
        }

        /// <summary>
        /// Required nested class to make this visual element usable in UXML files.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<OptionsSubMenu>
        {
        }
    }
}