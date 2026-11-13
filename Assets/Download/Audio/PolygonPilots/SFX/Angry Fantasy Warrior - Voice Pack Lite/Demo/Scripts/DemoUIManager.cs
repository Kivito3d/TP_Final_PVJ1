using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ResonantMesh.DemoSoundManager
{
    /// <summary>
    /// Programmatically creates a full Canvas overlay UI at runtime based on DemoSoundManager configuration.
    /// </summary>
    public class DemoUIManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private DemoSoundManager soundManager;

        [Header("UI Styling Defaults")]
        private static readonly Color BgColor = new Color(0.1f, 0.12f, 0.15f, 0.92f);
        private static readonly Color HeaderBgColor = new Color(0.15f, 0.18f, 0.22f, 1.0f);
        private static readonly Color NormalBtnColor = new Color(0.22f, 0.26f, 0.32f, 1.0f);
        private static readonly Color GreenBtnColor = new Color(0.18f, 0.55f, 0.34f, 1.0f);
        private static readonly Color RedBtnColor = new Color(0.72f, 0.22f, 0.22f, 1.0f);
        private static readonly Color TextColor = new Color(0.92f, 0.94f, 0.96f, 1.0f);

        private Font defaultFont;

        private void Start()
        {
            if (soundManager == null)
            {
                soundManager = FindObjectOfType<DemoSoundManager>();
                if (soundManager == null)
                {
                    Debug.LogError("[DemoUIManager] DemoSoundManager reference missing in scene!", this);
                    return;
                }
            }


            EnsureEventSystem();
            BuildUI();
        }

        private void EnsureEventSystem()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystemGO = new GameObject("EventSystem");
                eventSystemGO.AddComponent<EventSystem>();
                eventSystemGO.AddComponent<StandaloneInputModule>();
            }
        }

        private void BuildUI()
        {
            GameObject canvasGO = new GameObject("SoundboardCanvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGO.AddComponent<GraphicRaycaster>();

            GameObject mainPanel = CreateUIGameObject("MainPanel", canvasGO.transform);
            RectTransform panelRT = mainPanel.AddComponent<RectTransform>();
            SetFullAnchorStretch(panelRT);
            Image bgImage = mainPanel.AddComponent<Image>();
            bgImage.color = BgColor;

            GameObject container = CreateUIGameObject("ContainerLayout", mainPanel.transform);
            RectTransform containerRT = container.AddComponent<RectTransform>();
            SetFullAnchorStretch(containerRT);
            containerRT.offsetMin = new Vector2(30, 30);
            containerRT.offsetMax = new Vector2(-30, -30);

            VerticalLayoutGroup vLayout = container.AddComponent<VerticalLayoutGroup>();
            vLayout.spacing = 15;
            vLayout.childControlHeight = true;
            vLayout.childControlWidth = true;
            vLayout.childForceExpandHeight = false;

            CreateHeader(container.transform);
            CreateSoundGrid(container.transform);
            CreateFooterControls(container.transform);
        }

        private void CreateHeader(Transform parent)
        {
            GameObject headerGO = CreateUIGameObject("Header", parent);
            Image headerBg = headerGO.AddComponent<Image>();
            headerBg.color = HeaderBgColor;

            LayoutElement le = headerGO.AddComponent<LayoutElement>();
            le.preferredHeight = 60;
            le.flexibleWidth = 1;

            Text titleText = CreateTextObject("TitleText", headerGO.transform, "SOUNDBOARD DEMO SCENE", 24, FontStyle.Bold, TextAnchor.MiddleCenter);
            RectTransform textRT = titleText.GetComponent<RectTransform>();
            SetFullAnchorStretch(textRT);
        }

        private void CreateSoundGrid(Transform parent)
        {
            GameObject gridGO = CreateUIGameObject("SoundGrid", parent);

            LayoutElement le = gridGO.AddComponent<LayoutElement>();
            le.flexibleHeight = 1;
            le.flexibleWidth = 1;

            GridLayoutGroup grid = gridGO.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(180, 70);
            grid.spacing = new Vector2(12, 12);
            grid.constraint = GridLayoutGroup.Constraint.Flexible;
            grid.childAlignment = TextAnchor.UpperLeft;

            foreach (var entry in soundManager.soundEntries)
            {
                string keyLabel = entry.hotkey != KeyCode.None ? $"[{FormatKeyCode(entry.hotkey)}]" : "[-]";
                string buttonText = $"{keyLabel}\n{entry.id}";

                GameObject btnGO = CreateButton(gridGO.transform, buttonText, NormalBtnColor, () =>
                {
                    soundManager.PlaySound(entry.id);
                });
            }
        }

        private void CreateFooterControls(Transform parent)
        {
            GameObject footerGO = CreateUIGameObject("FooterControls", parent);

            LayoutElement footerLE = footerGO.AddComponent<LayoutElement>();
            footerLE.preferredHeight = 80;
            footerLE.flexibleWidth = 1;

            HorizontalLayoutGroup hLayout = footerGO.AddComponent<HorizontalLayoutGroup>();
            hLayout.spacing = 20;
            hLayout.childControlWidth = true;
            hLayout.childControlHeight = true;
            hLayout.childForceExpandWidth = true;

            CreateButton(footerGO.transform, "PLAY RANDOM SOUND", GreenBtnColor, () =>
            {
                soundManager.PlayRandomSound();
            });

            CreateButton(footerGO.transform, "STOP ALL", RedBtnColor, () =>
            {
                soundManager.StopAllSounds();
            });

            GameObject volumeGroup = CreateUIGameObject("VolumeGroup", footerGO.transform);
            VerticalLayoutGroup volLayout = volumeGroup.AddComponent<VerticalLayoutGroup>();
            volLayout.childControlWidth = true;
            volLayout.childControlHeight = true;
            volLayout.spacing = 5;

            Text volLabel = CreateTextObject("VolumeLabel", volumeGroup.transform, "MASTER VOLUME", 14, FontStyle.Bold, TextAnchor.MiddleCenter);
            LayoutElement labelLE = volLabel.gameObject.AddComponent<LayoutElement>();
            labelLE.preferredHeight = 20;

            GameObject sliderGO = CreateUIGameObject("VolumeSlider", volumeGroup.transform);
            Slider slider = sliderGO.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = soundManager.MasterVolume;

            GameObject sliderBg = CreateUIGameObject("Background", sliderGO.transform);
            Image bgImg = sliderBg.AddComponent<Image>();
            bgImg.color = new Color(0.15f, 0.15f, 0.15f, 1f);
            RectTransform bgRT = sliderBg.GetComponent<RectTransform>();
            SetFullAnchorStretch(bgRT);

            GameObject fillArea = CreateUIGameObject("Fill Area", sliderGO.transform);
            RectTransform fillAreaRT = fillArea.GetComponent<RectTransform>();
            SetFullAnchorStretch(fillAreaRT);
            fillAreaRT.offsetMin = new Vector2(5, 0);
            fillAreaRT.offsetMax = new Vector2(-5, 0);

            GameObject fill = CreateUIGameObject("Fill", fillArea.transform);
            Image fillImg = fill.AddComponent<Image>();
            fillImg.color = GreenBtnColor;
            RectTransform fillRT = fill.GetComponent<RectTransform>();
            SetFullAnchorStretch(fillRT);

            slider.targetGraphic = fillImg;
            slider.fillRect = fillRT;

            slider.onValueChanged.AddListener((val) =>
            {
                soundManager.SetGlobalVolume(val);
            });
        }

        #region Helpers
        private GameObject CreateUIGameObject(string name, Transform parent)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go;
        }

        private Text CreateTextObject(string name, Transform parent, string textContent, int fontSize, FontStyle fontStyle, TextAnchor alignment)
        {
            GameObject go = CreateUIGameObject(name, parent);
            Text text = go.AddComponent<Text>();
            text.text = textContent;
            text.font = defaultFont;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.alignment = alignment;
            text.color = TextColor;
            return text;
        }

        private GameObject CreateButton(Transform parent, string labelText, Color buttonColor, UnityEngine.Events.UnityAction onClick)
        {
            GameObject btnGO = CreateUIGameObject("Btn_" + labelText.Replace("\n", "_"), parent);

            Image img = btnGO.AddComponent<Image>();
            img.color = buttonColor;

            Button btn = btnGO.AddComponent<Button>();
            ColorBlock colors = btn.colors;
            colors.normalColor = buttonColor;
            colors.highlightedColor = buttonColor * 1.25f;
            colors.pressedColor = buttonColor * 0.75f;
            btn.colors = colors;
            btn.onClick.AddListener(onClick);

            Text text = CreateTextObject("Label", btnGO.transform, labelText, 14, FontStyle.Bold, TextAnchor.MiddleCenter);
            RectTransform textRT = text.GetComponent<RectTransform>();
            SetFullAnchorStretch(textRT);

            return btnGO;
        }

        private void SetFullAnchorStretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private string FormatKeyCode(KeyCode code)
        {
            if (code >= KeyCode.Alpha0 && code <= KeyCode.Alpha9)
                return code.ToString().Replace("Alpha", "");
            return code.ToString();
        }
        #endregion
    }
}