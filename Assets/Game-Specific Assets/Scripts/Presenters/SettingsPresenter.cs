using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Toggle SoundEnabled;
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider EffectVolume;
    public Slider VisualQuality;
    public Slider Antialiasing;
    public Toggle Anisotropy;
    public Slider BattleRate;

    private AudioManager _audio;
    private BattleManager _battle;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        base.Start();

        _audio = AudioManager.Instance;
        _battle = BattleManager.Instance;
    }

    public void EnableSound()
    {
        _audio.SoundEnabled = SoundEnabled.isOn;
    }

    public void UpdateMasterVolume()
    {
        _audio.MasterVolume = MasterVolume.value;
    }

    public void UpdateMusicVolume()
    {
        _audio.MusicVolume = MusicVolume.value;
    }

    public void UpdateEffectVolume()
    {
        _audio.EffectVolume = EffectVolume.value;
    }

    public void UpdateVisualQuality()
    {
        QualitySettings.SetQualityLevel((int) VisualQuality.value, true);

        // Update the other controls with what's set up in the presets.
        Antialiasing.value = QualitySettings.antiAliasing / 2;
        Anisotropy.isOn = (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
                          || (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable);
    }

    public void UpdateAntialiasing()
    {
        QualitySettings.antiAliasing = (int) Antialiasing.value * 2;
    }

    public void UpdateAnisotropy()
    {
        QualitySettings.anisotropicFiltering = Anisotropy.isOn 
            ? AnisotropicFiltering.Enable 
            : AnisotropicFiltering.Disable;
    }

    public void UpdateBattleRate()
    {
        _battle.BattleRate = BattleRate.value;
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
