using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem; // Wsparcie dla Nowego Input Systemu

namespace ResonantMesh.DemoSoundManager
{
    /// <summary>
    /// Manages sound playback, dynamic AudioSource pooling, and global volume control.
    /// Provides public methods designed to be hooked directly into UI Buttons and Sliders.
    /// </summary>
    public class DemoSoundManager : MonoBehaviour
    {
        [System.Serializable]
        public class SoundEntry
        {
            [Tooltip("Unique Identifier for the sound category/effect. Used when calling PlaySound(id).")]
            public string id = "New Sound Group";

            [Tooltip("Array of AudioClips. A random clip from this list will be played when triggered.")]
            public AudioClip[] clips;

            [Tooltip("Optional hotkey trigger.")]
            public KeyCode hotkey = KeyCode.None;

            [Range(0f, 1f)]
            [Tooltip("Individual volume scaling for this sound group.")]
            public float volumeScale = 1.0f;

            [HideInInspector]
            public AudioSource source;
        }

        [Header("Sound Configuration")]
        [Tooltip("List of all audio groups and hotkeys in the soundboard.")]
        public List<SoundEntry> soundEntries = new List<SoundEntry>();

        [Header("Global Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float masterVolume = 1.0f;

        // Fast dictionary lookup by ID
        private Dictionary<string, SoundEntry> soundDictionary = new Dictionary<string, SoundEntry>(StringComparer.OrdinalIgnoreCase);

        public float MasterVolume => masterVolume;

        private void Awake()
        {
            InitializeAudioSources();
        }

        private void Update()
        {
            //HandleHotkeyInputs();
        }

        /// <summary>
        /// Instantiates individual AudioSource components dynamically at runtime for each entry.
        /// </summary>
        private void InitializeAudioSources()
        {
            soundDictionary.Clear();

            foreach (var entry in soundEntries)
            {
                if (string.IsNullOrWhiteSpace(entry.id))
                {
                    Debug.LogWarning("[DemoSoundManager] Sound entry with empty ID detected. Skipping.", this);
                    continue;
                }

                if (soundDictionary.ContainsKey(entry.id))
                {
                    Debug.LogWarning($"[DemoSoundManager] Duplicate Sound ID '{entry.id}' detected. Only the first entry will be used.", this);
                    continue;
                }

                GameObject child = new GameObject($"AudioSource_{entry.id}");
                child.transform.SetParent(transform);

                AudioSource source = child.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                source.spatialBlend = 0f; // 2D Sound

                entry.source = source;
                soundDictionary.Add(entry.id, entry);
            }
        }

        /// <summary>
        /// Checks for registered Hotkey triggers using Keyboard.current from the New Input System.
        /// </summary>
        private void HandleHotkeyInputs()
        {
           /* Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return;

            foreach (var entry in soundEntries)
            {
                if (entry.hotkey == KeyCode.None) continue;

                Key key = ConvertKeyCodeToKey(entry.hotkey);
                if (key != Key.None && keyboard[key].wasPressedThisFrame)
                {
                    PlaySound(entry.id);
                }
            }*/
        }

        /// <summary>
        /// Helper method mapping legacy KeyCode values to New Input System Key enums.
        /// </summary>
        /*private Key ConvertKeyCodeToKey(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Alpha0: return Key.Digit0;
                case KeyCode.Alpha1: return Key.Digit1;
                case KeyCode.Alpha2: return Key.Digit2;
                case KeyCode.Alpha3: return Key.Digit3;
                case KeyCode.Alpha4: return Key.Digit4;
                case KeyCode.Alpha5: return Key.Digit5;
                case KeyCode.Alpha6: return Key.Digit6;
                case KeyCode.Alpha7: return Key.Digit7;
                case KeyCode.Alpha8: return Key.Digit8;
                case KeyCode.Alpha9: return Key.Digit9;
                case KeyCode.Space: return Key.Space;
                case KeyCode.Return: return Key.Enter;
                case KeyCode.Escape: return Key.Escape;
                default:
                    if (Enum.TryParse(keyCode.ToString(), true, out Key parsedKey))
                    {
                        return parsedKey;
                    }
                    return Key.None;
            }
        }*/

        #region Public API Methods (For UI Binding)

        /// <summary>
        /// Plays a random AudioClip from the specified sound entry group.
        /// </summary>
        /// <param name="id">The unique identifier of the sound group matching SoundEntry.id.</param>
        public void PlaySound(string id)
        {
            if (soundDictionary.TryGetValue(id, out SoundEntry entry))
            {
                if (entry.clips == null || entry.clips.Length == 0)
                {
                    Debug.LogWarning($"[DemoSoundManager] Cannot play sound '{id}': No AudioClips assigned to this group.", this);
                    return;
                }

                List<AudioClip> validClips = new List<AudioClip>();
                foreach (var clip in entry.clips)
                {
                    if (clip != null) validClips.Add(clip);
                }

                if (validClips.Count == 0)
                {
                    Debug.LogWarning($"[DemoSoundManager] Cannot play sound '{id}': All assigned AudioClips are null.", this);
                    return;
                }

                AudioClip selectedClip = validClips[UnityEngine.Random.Range(0, validClips.Count)];

                if (entry.source != null)
                {
                    entry.source.Stop();
                    entry.source.clip = selectedClip;
                    entry.source.volume = entry.volumeScale * masterVolume;
                    entry.source.Play();
                }
            }
            else
            {
                Debug.LogWarning($"[DemoSoundManager] Sound ID '{id}' was not found in the sound list.", this);
            }
        }

        /// <summary>
        /// Triggers a random sound group from the entire entries list. Hook this to a "Play Random" UI Button.
        /// </summary>
        public void PlayRandomSound()
        {
            if (soundEntries.Count == 0)
            {
                Debug.LogWarning("[DemoSoundManager] Cannot play random sound: Sound list is empty.", this);
                return;
            }

            List<SoundEntry> validEntries = soundEntries.FindAll(e => e.clips != null && e.clips.Length > 0);
            if (validEntries.Count == 0)
            {
                Debug.LogWarning("[DemoSoundManager] Cannot play random sound: No assigned AudioClips found in any entry.", this);
                return;
            }

            int randomIndex = UnityEngine.Random.Range(0, validEntries.Count);
            PlaySound(validEntries[randomIndex].id);
        }

        /// <summary>
        /// Instantly stops playback on all active AudioSource instances. Hook this to a "Stop All" UI Button.
        /// </summary>
        public void StopAllSounds()
        {
            foreach (var entry in soundEntries)
            {
                if (entry.source != null && entry.source.isPlaying)
                {
                    entry.source.Stop();
                }
            }
        }

        /// <summary>
        /// Adjusts master volume for all sound entries and updates active playback levels. Hook this to UI Slider's OnValueChanged.
        /// </summary>
        /// <param name="volume">Float value between 0.0 and 1.0.</param>
        public void SetGlobalVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);

            foreach (var entry in soundEntries)
            {
                if (entry.source != null)
                {
                    entry.source.volume = entry.volumeScale * masterVolume;
                }
            }
        }

        #endregion
    }
    
}