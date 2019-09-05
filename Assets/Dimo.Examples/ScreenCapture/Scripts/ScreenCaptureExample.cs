using UnityEngine;

namespace Dimo.Examples
{
    public class ScreenCaptureExample : MonoBehaviour
    {
        void Awake()
        {
            ScreenCaptureSingleton.Instance.OnComplete = (fileName) => { Debug.Log($"Create {fileName}"); };
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
