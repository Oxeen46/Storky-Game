using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Storky
{
    internal sealed class FlightBinding
    {
        // Giriş ataması ve görünen tuş adı
        readonly InputAction action;
        readonly int index;
        public readonly bool gamepad, stick;
        public readonly string label;
        string Key => "Storky.Binding." + action.name + "." + index;
        public string Path => action.bindings[index].effectivePath;
        public string Display => InputControlPath.ToHumanReadableString(Path, InputControlPath.HumanReadableStringOptions.OmitDevice);

        // Atamaları yükleme ve kaydetme
        FlightBinding(InputAction action, int index, string label)
        {
            this.action = action;
            this.index = index;
            this.label = label;
            string path = action.bindings[index].path;
            gamepad = path.StartsWith("<Gamepad>");
            stick = path.EndsWith("Stick", StringComparison.OrdinalIgnoreCase);
            string saved = PlayerPrefs.GetString(Key);
            if (!string.IsNullOrEmpty(saved) && !Reserved(saved)) action.ApplyBindingOverride(index, saved);
        }

        public void Assign(string path)
        {
            action.ApplyBindingOverride(index, path);
            PlayerPrefs.SetString(Key, path);
        }

        public static List<FlightBinding> Load(InputActionMap map)
        {
            var result = new List<FlightBinding>();
            foreach (var action in map.actions)
            {
                if (action.name == "Pause") continue;
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var b = action.bindings[i];
                    if (b.isComposite || b.path.StartsWith("<Mouse>")) continue;
                    string label = action.name switch
                    {
                        "Move" => b.name switch { "Up" => "İleri", "Down" => "Geri", "Left" => "Sol", "Right" => "Sağ", _ => "Hareket" },
                        "Look" => "Kamerayı çevir", "Flap" => "Kalkış / kanat vuruşu",
                        "Dive" => "Dalış", "AirRoll" => b.name.Equals("negative", StringComparison.OrdinalIgnoreCase) ? "Sola roll" : "Sağa roll",
                        _ => action.name
                    };
                    result.Add(new FlightBinding(action, i, label));
                }
            }
            return result;
        }

        // Sabit duraklatma tuşları ve yeni giriş yakalama
        static bool Reserved(string path) =>
            string.Equals(path, "<Keyboard>/escape", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(path, "<Gamepad>/start", StringComparison.OrdinalIgnoreCase);

        public string Capture()
        {
            if (!gamepad)
            {
                if (Keyboard.current != null)
                    foreach (var key in Keyboard.current.allKeys)
                        if (key.wasPressedThisFrame && !Reserved("<Keyboard>/" + key.name)) return "<Keyboard>/" + key.name;
                return null;
            }
            var pad = Gamepad.current;
            if (pad == null) return null;
            if (stick)
            {
                if (pad.leftStick.ReadValue().magnitude > .65f) return "<Gamepad>/leftStick";
                if (pad.rightStick.ReadValue().magnitude > .65f) return "<Gamepad>/rightStick";
                return null;
            }
            foreach (var control in pad.allControls)
                if (control is ButtonControl button && (control.parent == pad || control.parent == pad.dpad)
                    && control != pad.dpad && control != pad.startButton && button.wasPressedThisFrame)
                    return "<Gamepad>/" + (control.parent == pad.dpad ? "dpad/" : "") + control.name;
            return null;
        }
    }
}
