using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    [Header ("Panel Menus")]
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject VictoryPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private Animator fadeAnimator;

    [Header ("HUD Settings")]
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Slider sliderBar;
    [SerializeField] private HealthSystem healthSystemPlayer;

    [Header ("Audio Menu Settings")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;
    [SerializeField] private AudioMixer mainMixer;

    [Header("Audio Menu Settings")]
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private Toggle vsyncToggle;

    
    private string KEY_MASTER = "vol_master";
    private string KEY_MUSIC = "vol_music";
    private string KEY_SFX = "vol_sfx";
    private string KEY_UI = "vol_ui";
    private string KEY_VSYNC = "vsync";
    private string KEY_RES = "resolution";

    private Resolution[] resolutions;

    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.pitch = 1.0f;
    }
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnChangeState += StateChage;
            GameManager.Instance.OnEnemyChange += UpdateEnemiesHUD;
        }

        if (healthSystemPlayer != null)
        {
            healthSystemPlayer.OnChangeHealthPlayer += UpdateBar;
        }

        SetupResolutionDropdown();
        LoadSettings();
        
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnChangeState -= StateChage;
            GameManager.Instance.OnEnemyChange -= UpdateEnemiesHUD;
        }

        if (healthSystemPlayer != null)
        {
            healthSystemPlayer.OnChangeHealthPlayer -= UpdateBar;
        }
    }


    private void StateChage(GameState newState)
    {
        if(PausePanel!=null) PausePanel.SetActive(false);
        VictoryPanel.SetActive(false);
        GameOverPanel.SetActive(false);


        switch (newState)
        {
            case GameState.Paused:
                PausePanel.SetActive(true);
                UnlockCursor();
                break;
            case GameState.Victory:
                VictoryPanel.SetActive(true);
                UnlockCursor();
                break;
            case GameState.GameOver:
                GameOverPanel.SetActive(true);
                UnlockCursor();
                break;
            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameState.ChangeScene:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                break;
        }
    }

   
    private void UpdateEnemiesHUD(int currentEnemies)
    {
        if (textMesh != null)
        {
            //textMesh.text = currentEnemies.ToString();
            textMesh.text = $"{currentEnemies}";
        }
    }

    private void UpdateBar(float health)
    {
        if (sliderBar != null)
        {
            sliderBar.value = health;
        }
    }





    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlaySoundUI(int index)
    {
        audioSource.PlayOneShot(audioClips[index]);
        audioSource.Play();
        audioSource.Stop();
        //audioSource.

    }
    public void ChangeScene(string scene)
    {
        StartCoroutine(TransitionFade(1.5f,"FadeOut",scene));
        

    }

    public void TogglePanel(GameObject panel)
    {
        if (panel != null)
        {
            bool toggle = panel.activeInHierarchy ? false : true;
            panel.SetActive(toggle);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator TransitionFade(float time, string parameter,string scene)
    {
        fadeAnimator.SetTrigger(parameter);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scene);
    }

    /*public void setVolume(float volume,string channel,string keyPrefs)
    {
        mainMixer.SetFloat(channel, Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f);
        PlayerPrefs.SetFloat(keyPrefs, volume);
    }*/
    
    public void SetMasterVolume(float v)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);

        PlayerPrefs.SetFloat(KEY_MASTER, v);
    }

    public void SetMusicVolume(float v)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);
        PlayerPrefs.SetFloat(KEY_MUSIC, v);
    }

    public void SetSfxVolume(float v)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);
        PlayerPrefs.SetFloat(KEY_SFX, v);
    }

    public void SetUiVolume(float v)
    {
        mainMixer.SetFloat("UIVolume", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);
        PlayerPrefs.SetFloat(KEY_UI, v);
    }


    public void SetVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        PlayerPrefs.SetInt(KEY_VSYNC, isEnabled ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions == null || resolutions.Length == 0) return;
        Resolution r = resolutions[resolutionIndex];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt(KEY_RES, resolutionIndex);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void SetupResolutionDropdown()
    {
        if (resolution == null) return;

        resolutions = Screen.resolutions;
        resolution.ClearOptions();

        List<string> options = new List<string>();
        int currentIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }
        resolution.AddOptions(options);
        resolution.value = currentIndex;
        resolution.RefreshShownValue();
        resolution.onValueChanged.AddListener(SetResolution);
    }


    private void LoadSettings()
    {
        float master = PlayerPrefs.GetFloat(KEY_MASTER, 1f);
        float music = PlayerPrefs.GetFloat(KEY_MUSIC, 1f);
        float sfx = PlayerPrefs.GetFloat(KEY_SFX, 1f);
        float ui = PlayerPrefs.GetFloat(KEY_UI, 1f);


        if (masterSlider != null) masterSlider.SetValueWithoutNotify(master);
        if (musicSlider != null) musicSlider.SetValueWithoutNotify(music);
        if (sfxSlider != null) sfxSlider.SetValueWithoutNotify(sfx);
        if (uiSlider != null) uiSlider.SetValueWithoutNotify(ui);

  
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(master, 0.0001f)) * 20f);
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(music, 0.0001f)) * 20f);
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(sfx, 0.0001f)) * 20f);
        mainMixer.SetFloat("UIVolume", Mathf.Log10(Mathf.Max(ui, 0.0001f)) * 20f);

        bool vsync = PlayerPrefs.GetInt(KEY_VSYNC, 1) == 1;
        if (vsyncToggle != null) vsyncToggle.SetIsOnWithoutNotify(vsync);
        QualitySettings.vSyncCount = vsync ? 1 : 0;

        int resIndex = PlayerPrefs.GetInt(KEY_RES, -1);
        if (resIndex >= 0 && resolutions != null && resIndex < resolutions.Length)
        {
            Resolution r = resolutions[resIndex];
            Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);
            if (resolution != null)
            {
                resolution.SetValueWithoutNotify(resIndex);
                resolution.RefreshShownValue();
            }
        }
    }



  

}
