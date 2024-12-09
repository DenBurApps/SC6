using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Settings : MonoBehaviour
{
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _privacyCanvas;
    [SerializeField] private GameObject _termsCanvas;
    [SerializeField] private GameObject _contactCanvas;
    [SerializeField] private GameObject _versionCanvas;
    [SerializeField] private TMP_Text _versionText;
    [SerializeField] private Button _soundButton;
    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private float _soundOnVolume = -20f;
    [SerializeField] private float _soundOffVolume = -80f;
    private string _version = "Application version:\n";

    private bool _soundIsOn = true;

    private void Awake()
    {
        _settingsCanvas.SetActive(false);
        _privacyCanvas.SetActive(false);
        _termsCanvas.SetActive(false);
        _contactCanvas.SetActive(false);
        _versionCanvas.SetActive(false);
        SetVersion();
    }

    private void OnEnable()
    {
        _soundButton.onClick.AddListener(ProcessSoundToggled);
    }

    private void OnDisable()
    {
        _soundButton.onClick.RemoveListener(ProcessSoundToggled);
    }


    private void ProcessSoundToggled()
    {
        _soundIsOn = !_soundIsOn;
        
        PlayerPrefs.SetInt("SoundOff", _soundIsOn ? 0 : 1);
        UpdateSoundState();
    }

    private void UpdateSoundState()
    {
        _soundButton.image.sprite = _soundIsOn ? _soundOn : _soundOff;
        _audioMixer.audioMixer.SetFloat("MyExposedParam", _soundIsOn ? _soundOnVolume : _soundOffVolume);
    }

    private void SetVersion()
    {
        _versionText.text = _version + Application.version;
    }

    public void ShowSettings()
    {
        _settingsCanvas.SetActive(true);
        
        if (PlayerPrefs.HasKey("SoundOff"))
        {
            _soundIsOn = PlayerPrefs.GetInt("SoundOff") == 0;
        }
        else
        {
            _soundIsOn = true;
            PlayerPrefs.SetInt("SoundOff", 0);
        }

        _soundButton.image.sprite = _soundIsOn ? _soundOn : _soundOff;
    }

    public void RateUs()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}