using System;
using UnityEngine;

namespace Util
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance { get; private set; }
        public bool IsPaused { get; private set; }
        
        public event Action Paused;
        public event Action Resumed;

        public void Pause()
        {
            if (IsPaused)
                return;

            IsPaused = true;
            Paused?.Invoke();
        }

        public void Resume()
        {
            if (!IsPaused)
                return;

            IsPaused = false;
            Resumed?.Invoke();
        }

        private void Awake()
        {
            if (Instance != null)
                Debug.LogWarningFormat(this, "Duplicate {0} components in current scene!", nameof(PauseManager));
            
            Instance = this;
        }
    }
}