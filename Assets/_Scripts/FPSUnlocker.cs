using UnityEngine;

namespace _Scripts
{
    public class FPSUnlocker : MonoBehaviour
    {
        [SerializeField] private bool _lockFPS = false;

        private void Start()
        {
            switch (_lockFPS)
            {
                case true:
                    break;
                case false:
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = 60;
                    break;
            }
        }
    }
}