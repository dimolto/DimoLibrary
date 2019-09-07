using UnityEngine;

namespace Dimo.Examples
{
    public class ScreenCaptureExample : MonoBehaviour
    {
        void Awake()
        {
            ScreenCaptureSingleton.Instance.OnComplete = (_, filePath) => { Debug.Log($"Created {filePath}"); };
        }

        public void PressCaptureScreen()
        {
            ScreenCaptureSingleton.Instance.Capture();
        }

        public void PressCaptureScreen500_500()
        {
            ScreenCaptureSingleton.Instance.Capture(500, 500);
        }
    }
}
