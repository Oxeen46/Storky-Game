using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Storky
{
    internal sealed class FlightPauseMenu
    {
        // Menü öğeleri ve seçim durumu
        sealed class Item
        {
            public VisualElement view;
            public string help;
            public Action activate;
            public Action<int> adjust;
            public bool enabled = true;
        }

        readonly VisualElement overlay, card;
        readonly FlightSettings settings;
        readonly Action resume, preview;
        readonly List<FlightBinding> bindings;
        readonly List<Vector2Int> resolutions;
        readonly List<Item> tabs = new(), rows = new();
        readonly string[] sections = { "Ses", "Grafik", "Kontroller", "Kamera" };
        VisualElement content, modal;
        ScrollView scroll;
        Label explanation;
        bool inSettings, onTabs, padBindings, sticksReleased;
        int section, selected, captureFrame, modalChoice;
        float nextRepeat, deadline;
        Vector2Int lastDirection;
        FlightBinding pending, conflict;
        string candidate;

        // Panel kurulumu ve ortak UI parçaları
        public FlightPauseMenu(VisualElement root, FlightSettings settings, InputActionMap controls, Action resume, Action preview)
        {
            this.settings = settings;
            this.resume = resume;
            this.preview = preview;
            bindings = FlightBinding.Load(controls);
            resolutions = Screen.resolutions.Select(r => new Vector2Int(r.width, r.height))
                .Concat(new[] { new Vector2Int(1280, 720), new Vector2Int(1600, 900), new Vector2Int(1920, 1080) })
                .Distinct().OrderBy(r => r.x).ThenBy(r => r.y).ToList();
            resolutions.Insert(0, Vector2Int.zero);
            var savedResolution = new Vector2Int(settings.resolutionWidth, settings.resolutionHeight);
            if (!resolutions.Contains(savedResolution)) resolutions.Add(savedResolution);
            overlay = new VisualElement { name = "pause-overlay" };
            overlay.styleSheets.Add(Resources.Load<StyleSheet>("FlightPause"));
            root.Add(overlay);
            card = Element(overlay, "pause-card");
            overlay.RegisterCallback<GeometryChangedEvent>(e =>
            {
                float scale = Mathf.Clamp(Mathf.Min(e.newRect.width / 1440f, e.newRect.height / 810f), 1, 1.5f);
                card.style.scale = new Scale(new Vector3(scale, scale, 1));
                card.style.maxHeight = Length.Percent(86 / scale);
            });
            overlay.style.display = DisplayStyle.None;
        }

        static VisualElement Element(VisualElement parent, string style)
        {
            var element = new VisualElement();
            element.AddToClassList(style);
            parent.Add(element);
            return element;
        }

        static Label Text(VisualElement parent, string value, string style)
        {
            var label = new Label(value) { pickingMode = PickingMode.Ignore };
            label.AddToClassList(style);
            parent.Add(label);
            return label;
        }

        // Menü yaşam döngüsü ve sayfa düzeni
        public void Open()
        {
            overlay.style.display = DisplayStyle.Flex;
            Main();
        }

        public void Close()
        {
            EndDialog();
            overlay.style.display = DisplayStyle.None;
            settings.Save();
        }

        void Clear()
        {
            card.Clear();
            rows.Clear();
            tabs.Clear();
            selected = 0;
            explanation = null;
            scroll = null;
            lastDirection = Vector2Int.zero;
        }

        void Main()
        {
            Clear();
            inSettings = onTabs = false;
            card.EnableInClassList("wide", false);
            Text(card, "Oyun duraklatıldı", "heading");
            content = card;
            Row("Devam et", "", resume);
            Row("Ayarlar", "", () => Settings(0, true));
            Row("Ana menüye dön", "", null, false);
            Select(false, 0);
        }

        void Settings(int page, bool focusTabs)
        {
            Clear();
            inSettings = true;
            section = page;
            card.EnableInClassList("wide", true);
            Text(card, "Ayarlar", "heading");
            var columns = Element(card, "columns");
            var sidebar = Element(columns, "sidebar");
            for (int i = 0; i < sections.Length; i++)
            {
                int tab = i;
                var item = MakeItem(sidebar, sections[i], "", () => Settings(tab, false), true, true);
                item.view.EnableInClassList("current", i == page);
            }
            scroll = new ScrollView(ScrollViewMode.Vertical);
            scroll.AddToClassList("settings-scroll");
            columns.Add(scroll);
            content = scroll.contentContainer;
            var info = Element(columns, "info");
            Text(info, "Açıklama", "caption");
            explanation = Text(info, "", "description");
            switch (page)
            {
                case 0:
                    Slider("Ana ses", "Oyundaki tüm seslerin seviyesini ayarlar.", 0, 100, 1, () => settings.volume * 100,
                        v => { settings.volume = v / 100; AudioListener.volume = settings.volume; }, Percent);
                    Slider("Çevre sesleri", "Çevre sesleri için kaydedilen seviye.", 0, 100, 1,
                        () => settings.ambientVolume * 100, v => settings.ambientVolume = v / 100, Percent);
                    Slider("Müzik", "Oyun müziği için kaydedilen seviye.", 0, 100, 1,
                        () => settings.musicVolume * 100, v => settings.musicVolume = v / 100, Percent);
                    break;
                case 1: Graphics(); break;
                case 2: Controls(); break;
                case 3: Camera(); break;
            }
            var back = Element(card, "back");
            Text(back, "‹   Geri", "back-label");
            back.RegisterCallback<ClickEvent>(_ => Back());
            Select(focusTabs, focusTabs ? page : 0);
        }

        // Grafik ayarları
        void Graphics()
        {
            Row("V-Sync", "Kare sunumunu ekranın yenileme hızıyla eşler. Kapatınca seçtiğin FPS sınırı geri gelir.",
                () => { settings.vsync = !settings.vsync; settings.Apply(); Settings(1, false); }, value: settings.vsync ? "Açık" : "Kapalı");
            Slider("FPS sınırı", settings.vsync ? "V-Sync açık. Ekranın yenileme hızı kullanılıyor." :
                "Saniyedeki kare sayısını belirler. En sağ uç sınırsızdır. Sol çubuk ince, sağ çubuk hızlı ayarlar.",
                30, FlightSettings.Unlimited, 1, () => settings.fps,
                v => { settings.fps = Mathf.RoundToInt(v); settings.Apply(); },
                v => v == FlightSettings.Unlimited ? "Sınırsız" : v.ToString("0"), !settings.vsync);
            if (settings.vsync) Text(content, "Ekranın yenileme hızı kullanılıyor.", "description");
            Row("Ekran modu", "Windows oyun penceresini tam ekran veya pencereli kullanır.", () =>
            {
                settings.fullscreen = !settings.fullscreen;
                settings.ApplyDisplay();
                Settings(1, false);
                Select(false, 2);
            }, value: settings.fullscreen ? "Tam ekran" : "Pencereli");
            Row("Grafik kalitesi", "Dengeli veya Yüksek tercihini kaydeder.", () =>
            {
                settings.highQuality = !settings.highQuality;
                Settings(1, false);
                Select(false, 3);
            }, value: settings.highQuality ? "Yüksek" : "Dengeli");
            var current = new Vector2Int(settings.resolutionWidth, settings.resolutionHeight);
            int index = resolutions.IndexOf(current);
            void ChangeResolution(int direction)
            {
                var resolution = resolutions[(index + direction + resolutions.Count) % resolutions.Count];
                settings.resolutionWidth = resolution.x;
                settings.resolutionHeight = resolution.y;
                settings.ApplyDisplay();
                var directionHeld = lastDirection;
                Settings(1, false);
                lastDirection = directionHeld;
                Select(false, 4);
            }
            var row = Row("Çözünürlük", "Windows derlemesinde görüntü boyutunu ayarlar. Sol ve sağ ile seçenekler arasında geç.",
                () => ChangeResolution(1), value: current == Vector2Int.zero ? "Masaüstü" : $"{current.x} × {current.y}");
            row.adjust = direction => ChangeResolution(Math.Sign(direction));
        }

        // Kontrol ayarları
        void Controls()
        {
            Slider("Fare hassasiyeti", "Fare ile kamerayı çevirme hızını ayarlar.", .1f, 3, .05f,
                () => settings.mouseSensitivity, v => settings.mouseSensitivity = v, Factor);
            Slider("Kol hassasiyeti", "Kamera çubuğunun dönüş hızını ayarlar.", .1f, 3, .05f,
                () => settings.stickSensitivity, v => settings.stickSensitivity = v, Factor);
            Row("Dikey kamerayı ters çevir", "Yukarı ve aşağı bakış yönünü değiştirir. Leyleğin uçuş kumandası aynı kalır.",
                () => { settings.invertY = !settings.invertY; Settings(2, false); Select(false, 2); }, value: settings.invertY ? "Açık" : "Kapalı");
            Row("Tuş atamaları", "Düzenlemek istediğin giriş cihazını seç.", () =>
            {
                padBindings = !padBindings; Settings(2, false); Select(false, 3);
            }, value: padBindings ? "Kol" : "Klavye");
            foreach (var binding in bindings)
                if (binding.gamepad == padBindings)
                    Row(binding.label, "Yeni atama için seç. Kullanılan bir giriş seçersen iki atamayı takas edebilirsin.",
                        () => BeginCapture(binding), value: binding.Display);
        }

        // Kamera ayarları
        void Camera()
        {
            Slider("Takip mesafesi", "Kameranın leylekten temel uzaklığını ayarlar.", 0, 10, 1,
                () => settings.distanceLevel, v => settings.distanceLevel = v, Level);
            Slider("Yükseklik", "Kameranın baktığı noktayı leyleğe göre yükseltir.", 0, 10, 1,
                () => settings.heightLevel, v => settings.heightLevel = v, Level);
            Slider("Bakış açısı", "Kameranın aşağı bakış eğimini değiştirir.", -10, 45, 1,
                () => settings.angle, v => settings.angle = v, Degrees);
            Slider("Hızla uzaklaşma", "Hızlı uçuşta kameranın alacağı ek mesafe. Sıfırda uzaklaşma kapanır.", 0, 10, 1,
                () => settings.speedDistanceLevel, v => settings.speedDistanceLevel = v, Level);
            Row("Kamera ayarlarını sıfırla", "Bu sayfadaki ayarları başlangıç değerlerine döndürür.", () =>
            {
                settings.ResetCamera(); preview(); Settings(3, false); Select(false, 4);
            });
        }

        // Ayar satırları, değer biçimleri ve çubuklar
        static string Factor(float v) => v.ToString("0.00") + "×";
        static string Level(float v) => v.ToString("0");
        static string Percent(float v) => "%" + Level(v);
        static string Degrees(float v) => v.ToString("0") + "°";

        Item MakeItem(VisualElement parent, string label, string help, Action activate, bool enabled, bool tab)
        {
            var item = new Item { view = Element(parent, "item"), help = help, activate = activate, enabled = enabled };
            var list = tab ? tabs : rows;
            int index = list.Count;
            list.Add(item);
            Text(item.view, label, "item-label");
            item.view.EnableInClassList("inactive", !enabled);
            item.view.RegisterCallback<PointerEnterEvent>(evt =>
            {
                if (pending != null || evt.pointerType != UnityEngine.UIElements.PointerType.mouse || Mouse.current == null ||
                    Mouse.current.delta.ReadValue().sqrMagnitude == 0) return;
                if (enabled) Select(tab, index, false);
                else if (explanation != null) explanation.text = help;
            });
            item.view.RegisterCallback<ClickEvent>(_ =>
            {
                if (pending != null || !enabled) return;
                Select(tab, index, false);
                item.activate?.Invoke();
            });
            return item;
        }

        Item Row(string label, string help, Action activate, bool enabled = true, string value = null)
        {
            var item = MakeItem(content, label, help, activate, enabled, false);
            if (value != null) Text(item.view, value, "value");
            return item;
        }

        void Slider(string label, string help, float min, float max, float step, Func<float> get, Action<float> set,
            Func<float, string> format, bool enabled = true)
        {
            var item = Row(label, help, null, enabled);
            var value = Text(item.view, format(get()), "value");
            var track = Element(item.view, "track");
            var fill = Element(track, "fill");
            fill.pickingMode = PickingMode.Ignore;
            void Refresh(float v)
            {
                v = Mathf.Clamp(min + Mathf.Round((v - min) / step) * step, min, max);
                set(v);
                value.text = format(v);
                fill.style.width = Length.Percent(100 * Mathf.InverseLerp(min, max, v));
                if (section == 3) preview();
            }
            fill.style.width = Length.Percent(100 * Mathf.InverseLerp(min, max, get()));
            if (!enabled) return;
            item.adjust = n => Refresh(get() + n * step);
            void Drag(float x) => Refresh(Mathf.Lerp(min, max, (x - track.worldBound.xMin) / Mathf.Max(1, track.worldBound.width)));
            track.RegisterCallback<PointerDownEvent>(e =>
            {
                Select(false, rows.IndexOf(item), false);
                track.CapturePointer(e.pointerId);
                Drag(e.position.x);
            });
            track.RegisterCallback<PointerMoveEvent>(e => { if (track.HasPointerCapture(e.pointerId)) Drag(e.position.x); });
            track.RegisterCallback<PointerUpEvent>(e => track.ReleasePointer(e.pointerId));
        }

        // Klavye, fare ve kolla gezinme
        void Select(bool tab, int index, bool reveal = true)
        {
            onTabs = tab;
            selected = index;
            for (int i = 0; i < tabs.Count; i++) tabs[i].view.EnableInClassList("chosen", tab && i == index);
            for (int i = 0; i < rows.Count; i++) rows[i].view.EnableInClassList("chosen", !tab && i == index);
            if (explanation == null) return;
            explanation.text = tab ? sections[index] + " ayarlarını aç." : rows[index].help;
            if (!tab && reveal) scroll.ScrollTo(rows[index].view);
        }

        void Back()
        {
            if (!inSettings) resume();
            else if (!onTabs) Select(true, section);
            else Main();
        }

        public void Tick(bool pausePressed)
        {
            var k = Keyboard.current;
            var p = Gamepad.current;
            bool accept = k?.enterKey.wasPressedThisFrame == true || k?.spaceKey.wasPressedThisFrame == true || p?.buttonSouth.wasPressedThisFrame == true;
            bool cancel = k?.escapeKey.wasPressedThisFrame == true || p?.buttonEast.wasPressedThisFrame == true;
            if (pending != null)
            {
                if (pausePressed) EndDialog();
                else DialogInput(accept, cancel);
                return;
            }
            if (cancel || pausePressed || p?.startButton.wasPressedThisFrame == true) { Back(); return; }
            var list = onTabs ? tabs : rows;
            if (accept) { list[selected].activate?.Invoke(); return; }
            if (inSettings && k?.tabKey.wasPressedThisFrame == true) { Select(!onTabs, onTabs ? 0 : section); return; }

            Vector2 axis = p?.leftStick.ReadValue() ?? Vector2.zero;
            axis += p?.dpad.ReadValue() ?? Vector2.zero;
            if (k?.upArrowKey.isPressed == true) axis.y++;
            if (k?.downArrowKey.isPressed == true) axis.y--;
            if (k?.leftArrowKey.isPressed == true) axis.x--;
            if (k?.rightArrowKey.isPressed == true) axis.x++;
            float fast = p?.rightStick.x.ReadValue() ?? 0;
            bool coarse = !onTabs && list[selected].adjust != null && Mathf.Abs(fast) > .45f;
            if (coarse) axis = new Vector2(fast, 0);
            var dir = Vector2Int.zero;
            if (Mathf.Abs(axis.y) > .45f && Mathf.Abs(axis.y) >= Mathf.Abs(axis.x)) dir.y = axis.y > 0 ? -1 : 1;
            else if (Mathf.Abs(axis.x) > .45f) dir.x = axis.x > 0 ? 1 : -1;
            if (dir == Vector2Int.zero) { lastDirection = dir; return; }
            bool first = dir != lastDirection;
            if (!first && Time.unscaledTime < nextRepeat) return;
            nextRepeat = Time.unscaledTime + (first ? .30f : .075f);
            lastDirection = dir;
            if (dir.y != 0)
            {
                int index = selected + dir.y;
                while (index >= 0 && index < list.Count && !list[index].enabled) index += dir.y;
                if (index >= 0 && index < list.Count) Select(onTabs, index);
            }
            else if (onTabs && dir.x > 0) Settings(selected, false);
            else if (!onTabs && list[selected].adjust != null) list[selected].adjust(dir.x * (coarse ? 10 : 1));
            else if (inSettings && !onTabs && dir.x < 0) Select(true, section);
        }

        // Tuş atama ve çakışma iletişim kutusu
        void BeginCapture(FlightBinding binding)
        {
            pending = binding;
            captureFrame = Time.frameCount;
            deadline = Time.unscaledTime + 12;
            sticksReleased = false;
            modal = Element(overlay, "dialog-shade");
            var box = Element(modal, "dialog-box");
            Text(box, binding.label, "heading");
            string instruction = binding.stick ? "Çubukları bırak, sonra kullanmak istediğin çubuğu hareket ettir." :
                "Atamak istediğin " + (binding.gamepad ? "kol düğmesine" : "klavye tuşuna") + " bas.";
            Text(box, instruction + " 12 saniye içinde giriş olmazsa önceki atama korunur.", "description");
            DialogButton(box, "İptal", EndDialog);
        }

        VisualElement DialogButton(VisualElement box, string label, Action action)
        {
            var button = Element(box, "item");
            Text(button, label, "item-label");
            button.RegisterCallback<ClickEvent>(_ => action());
            return button;
        }

        void DialogInput(bool accept, bool cancel)
        {
            if (captureFrame == Time.frameCount) return;
            if (conflict != null)
            {
                var k = Keyboard.current;
                var p = Gamepad.current;
                if (cancel) { EndDialog(); return; }
                if (accept) { if (modalChoice == 0) FinishBinding(); else EndDialog(); return; }
                if (k?.upArrowKey.wasPressedThisFrame == true || k?.downArrowKey.wasPressedThisFrame == true ||
                    p?.dpad.up.wasPressedThisFrame == true || p?.dpad.down.wasPressedThisFrame == true ||
                    p?.leftStick.up.wasPressedThisFrame == true || p?.leftStick.down.wasPressedThisFrame == true)
                {
                    modalChoice = 1 - modalChoice;
                    HighlightDialog();
                }
                return;
            }
            if (Time.unscaledTime > deadline) { EndDialog(); return; }
            if (pending.stick && !sticksReleased)
            {
                var pad = Gamepad.current;
                sticksReleased = pad != null && pad.leftStick.ReadValue().magnitude < .3f && pad.rightStick.ReadValue().magnitude < .3f;
                return;
            }
            candidate = pending.Capture();
            if (candidate == null) return;
            conflict = bindings.Find(b => b != pending && string.Equals(b.Path, candidate, StringComparison.OrdinalIgnoreCase));
            if (conflict == null) { FinishBinding(); return; }
            modal.Clear();
            var box = Element(modal, "dialog-box");
            Text(box, "Atamalar takas edilsin mi?", "heading");
            Text(box, conflict.Display + " şu anda “" + conflict.label + "” için kullanılıyor. Bu eyleme “" + pending.Display + "” atanacak.", "description");
            DialogButton(box, "Takas et", FinishBinding);
            DialogButton(box, "İptal", EndDialog);
            modalChoice = 1;
            HighlightDialog();
        }

        void HighlightDialog()
        {
            var buttons = modal.Query<VisualElement>(className: "item").ToList();
            for (int i = 0; i < buttons.Count; i++) buttons[i].EnableInClassList("chosen", i == modalChoice);
        }

        void FinishBinding()
        {
            int index = selected;
            conflict?.Assign(pending.Path);
            pending.Assign(candidate);
            PlayerPrefs.Save();
            EndDialog();
            Settings(2, false);
            Select(false, index);
            var row = rows[index].view;
            row.schedule.Execute(() => scroll?.ScrollTo(row));
        }

        void EndDialog()
        {
            modal?.RemoveFromHierarchy();
            modal = null;
            pending = conflict = null;
            candidate = null;
        }
    }
}
