using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSliderMainMenu;
    [SerializeField] private Slider sfxSliderMainMenu;

    private void Start()
    {
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        bgmSlider.value = bgm;
        sfxSlider.value = sfx;
        bgmSliderMainMenu.value = bgm;
        sfxSliderMainMenu.value = sfx;

        audioManager.SetBGMVolume(bgm);
        audioManager.SetSFXVolume(sfx);

        bgmSlider.onValueChanged.AddListener(val =>
        {
            audioManager.SetBGMVolume(val);
            PlayerPrefs.SetFloat("BGMVolume", val);
        });

        sfxSlider.onValueChanged.AddListener(val =>
        {
            audioManager.SetSFXVolume(val);
            PlayerPrefs.SetFloat("SFXVolume", val);
        });

        bgmSliderMainMenu.onValueChanged.AddListener(val =>
        {
            audioManager.SetBGMVolume(val);
            PlayerPrefs.SetFloat("BGMVolume", val);
        });

        sfxSliderMainMenu.onValueChanged.AddListener(val =>
        {
            audioManager.SetSFXVolume(val);
            PlayerPrefs.SetFloat("SFXVolume", val);
        });
    }
}
