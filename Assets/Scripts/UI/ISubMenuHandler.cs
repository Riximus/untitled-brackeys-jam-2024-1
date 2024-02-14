using System;

namespace UI
{
    /// <summary>
    /// A component should implement this interface to indicate that it is intended to be used as a sub menu.
    /// </summary>
    public interface ISubMenuHandler
    {
        /// <summary>
        /// This event is fired when the user indicates that they want to navigate to the containing menu.
        /// </summary>
        event Action NavigateBackRequested;

        /// <summary>
        /// This method should be implemented to discard any unsaved changes, as the containing menu intends to take
        /// control.
        /// </summary>
        void Cancel();
    }
}