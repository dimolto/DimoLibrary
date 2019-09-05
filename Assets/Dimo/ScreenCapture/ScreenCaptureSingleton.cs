using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Dimo
{
    public class ScreenCaptureSingleton : MonoBehaviour
    {
        protected static ScreenCaptureSingleton instance = null;

        public static ScreenCaptureSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("ScreenCaptureSingleton").AddComponent<ScreenCaptureSingleton>();
                }

                return instance;
            }
        }

        public Action<string> OnComplete = null;

        public Func<string> CreateFilePath = null;

        protected bool capturing = false;

        protected virtual void Awake()
        {
            if (CreateFilePath == null)
            {
                // Default file path
                CreateFilePath = () =>
                {
                    return Application.dataPath + "/" + System.DateTime.Now.Second.ToString() + ".png";
                };
            }
        }

        /// <summary>
        /// Capture screen.
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        protected virtual IEnumerator CaptureCoroutine(int startX, int startY, int width, int height)
        {
            // Need wait for CaptureScreen.
            yield return new WaitForEndOfFrame();
            Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
            SaveTexture(tex, startX, startY, width, height);
            capturing = false;
        }

        /// <summary>
        /// Reserve capture screen.
        /// </summary>
        public virtual void Capture()
        {
            Capture(0, 0, Screen.width, Screen.height);
        }

        public virtual void Capture(int width, int height)
        {
            var x = (Screen.width - width) / 2;
            var y = (Screen.height - height) / 2;
            Capture(x, y, width, height);
        }

        /// <summary>
        /// Reserve capture screen.
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void Capture(int startX, int startY, int width, int height)
        {
            if (capturing)
            {
                return;
            }

            capturing = true;
            StartCoroutine(CaptureCoroutine(startX, startY, width, height));
        }

        /// <summary>
        /// Save png file from Texture2D.
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void SaveTexture(Texture2D tex, int startX, int startY, int width, int height)
        {
            Color[] colors = new Color[width * height];
            for (var i = 0; i < width && i < tex.width; i++)
            {
                for (var j = 0; j < height && j < tex.height; j++)
                {
                    var x = startX + i;
                    var y = startY + j;
                    var pixelColor = tex.GetPixel(x, y);
                    colors[x + y * width] = pixelColor;
                }
            }

            var fileName = CreateFilePath();
            Texture2D saveTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            saveTex.SetPixels(colors);
            File.WriteAllBytes(fileName, saveTex.EncodeToPNG());
            OnComplete?.Invoke(fileName);
        }
    }
}
