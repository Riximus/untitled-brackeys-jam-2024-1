using System;

namespace UI
{
    public class CreditsSubMenuHandler : ISubMenuHandler
    {
        public event Action NavigateBackRequested;

        public void Cancel()
        {
        }
    }
}