using System;
using UnityEngine;

namespace Storky
{
    [Serializable]
    internal sealed class FlightSettings
    {
        // Kaydedilen tercihler ve varsayılan değerler
        const string Key = "Storky.Settings.v1";
        public float volume = 1, mouseSensitivity = 1, stickSensitivity = 1;
        public float ambientVolume = 1, musicVolume = 1;
        public bool invertY, vsync;
        public bool fullscreen, highQuality = true;
        public int resolutionWidth, resolutionHeight;
        public int fps = 180;
        public const int Unlimited = 361;
        public float distanceLevel = 5, heightLevel = 5, angle = 17, speedDistanceLevel = 5;

        // Kamera seviyelerinin metreye dönüşümü
        public float Distance => distanceLevel * (3f / 5);
        public float Height => heightLevel * .25f;
        public float SpeedDistance => speedDistanceLevel * .2f;
        Vector2Int desktopSize;

        // Ayarları yükleme ve uygulama
        public static FlightSettings Load()
        {
            var desktop = Screen.currentResolution;
            var settings = new FlightSettings
            {
                fullscreen = Screen.fullScreen,
                desktopSize = new Vector2Int(desktop.width, desktop.height)
            };
            if (PlayerPrefs.HasKey(Key))
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(Key), settings);
            else
                settings.vsync = QualitySettings.vSyncCount > 0;
            settings.Apply();
            settings.ApplyDisplay();
            return settings;
        }

        public void ApplyDisplay()
        {
            if (Application.isEditor) return;
            int width = resolutionWidth > 0 ? resolutionWidth : desktopSize.x;
            int height = resolutionHeight > 0 ? resolutionHeight : desktopSize.y;
            Screen.SetResolution(width, height, fullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed);
        }

        public void Apply()
        {
            AudioListener.volume = volume;
            QualitySettings.vSyncCount = vsync ? 1 : 0;
            Application.targetFrameRate = vsync || fps == Unlimited ? -1 : fps;
        }

        // Tercihleri kaydetme ve kamerayı sıfırlama
        public void Save()
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }

        public void ResetCamera()
        {
            var defaults = new FlightSettings();
            distanceLevel = defaults.distanceLevel;
            heightLevel = defaults.heightLevel;
            angle = defaults.angle;
            speedDistanceLevel = defaults.speedDistanceLevel;
        }
    }
}
