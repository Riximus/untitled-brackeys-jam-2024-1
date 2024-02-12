using System;

namespace UI
{
    public interface ISubMenuHandler
    {
        event Action NavigateBackRequested;

        void Cancel();
    }
}