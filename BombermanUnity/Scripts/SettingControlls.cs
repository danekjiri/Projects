using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class SettingControls : MonoBehaviour
{
    //gameobjects references
    public TMPro.TMP_Dropdown Resolution;
    public TMPro.TMP_Dropdown BombCount;
    public TMPro.TMP_Dropdown PowerupProb;
    public TMPro.TMP_Dropdown StartSpeed;
    public TMPro.TMP_Dropdown ExplosionLenght;
    public AudioMixer MainMixer;
    public Slider AudioSlider;

    //constant variables to store settings data
    private const string storedResolution = "resolution";
    private const string storedBombCount = "bombCount";
    private const string storedPowerupProb = "powerupProb";
    private const string storedStartSpeed = "startSpeed";
    private const string storedExplosionLenght = "explosionLenght";
    private const string storedAudioLevel = "audioLevel";

    //check if anything happens to object
    private void Awake()
    {
        Resolution.onValueChanged.AddListener(OnResolutionChanged);
        BombCount.onValueChanged.AddListener(ChangeBombCount);
        PowerupProb.onValueChanged.AddListener(ChangePowerupProb);
        StartSpeed.onValueChanged.AddListener(ChangeStartSpeed);
        ExplosionLenght.onValueChanged.AddListener(ChangeExplosionLenght);
        AudioSlider.onValueChanged.AddListener(SetVolume);
    }

    //show object state with current value after leaving, indexing by array value 0_n-1
    private void Start()
    {
        Resolution.value = PlayerPrefs.GetInt(storedResolution, 0);
        AudioSlider.value = PlayerPrefs.GetFloat(storedAudioLevel, 0f);
        BombCount.value = PlayerPrefs.GetInt(storedBombCount, 0);
        PowerupProb.value = PlayerPrefs.GetInt(storedPowerupProb, 3);
        StartSpeed.value = PlayerPrefs.GetInt(storedStartSpeed, 1);
        ExplosionLenght.value = PlayerPrefs.GetInt(storedExplosionLenght, 0);
    }

    //set of functions given by its objects => store data if value changed
    public void ChangeExplosionLenght(int value)
    {
        PlayerPrefs.SetInt(storedExplosionLenght, ExplosionLenght.value);
        PlayerPrefs.Save();
    }

    public void ChangeStartSpeed(int value)
    {
        PlayerPrefs.SetInt(storedStartSpeed, StartSpeed.value);
        PlayerPrefs.Save();
    }

    public void ChangePowerupProb(int value)
    {
        PlayerPrefs.SetInt(storedPowerupProb, PowerupProb.value);
        PlayerPrefs.Save();
    }

    public void ChangeBombCount(int value)
    {
        PlayerPrefs.SetInt(storedBombCount, BombCount.value);
        PlayerPrefs.Save();
    }

    public void OnResolutionChanged(int value)
    {
        PlayerPrefs.SetInt(storedResolution, Resolution.value);
        PlayerPrefs.Save();
    }

    //save current slide value, modify master game player
    public void SetVolume(float volume)
    {
        MainMixer.SetFloat(storedAudioLevel, volume);
        PlayerPrefs.SetFloat(storedAudioLevel, volume);
        PlayerPrefs.Save();
    }

    //behaviour by menu selection - setting given screen resolution
    public void SetScreenResolution(int resolution)
    {
        switch(resolution)
        {
            case 0:
                Screen.SetResolution(1280, 720, false);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, false);
                break;
            case 2:
                Screen.SetResolution(2560, 1440, false);
                break;
            case 3:
                Screen.SetResolution(3840, 2160, false);
                break;
            default:
                break;
        }
    }
}
