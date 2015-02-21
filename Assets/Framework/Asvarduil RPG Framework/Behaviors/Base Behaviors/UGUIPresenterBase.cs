using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UGUIPresenterBase : DebuggableBehavior 
{
    #region Variables / Properties

    public float FadeRate = 0.5f;
    public float FadeThreshold = 0.05f;

    public AudioClip ButtonSound;

    protected Maestro _maestro;

    protected List<GameObject> _children = new List<GameObject>();
    protected List<Button> _buttons = new List<Button>();
    protected List<Toggle> _toggles = new List<Toggle>();
    protected List<Slider> _sliders = new List<Slider>();

    #endregion Variables / Properties

    #region Hooks

    public virtual void Start()
    {
        _maestro = Maestro.Instance;

        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform current = transform.GetChild(i);
            Button button = current.GetComponent<Button>();
            Toggle toggle = current.GetComponent<Toggle>();
            Slider slider = current.GetComponent<Slider>();

            _children.Add(current.gameObject);

            if (button != null)
                _buttons.Add(button);

            if (toggle != null)
                _toggles.Add(toggle);

            if (slider != null)
                _sliders.Add(slider);
        }

        DebugMessage("Presenter " + gameObject.name
                     + " has " + _buttons.Count + " buttons,"
                     + " and a total of " + _children.Count + " child objects.");
    }

    public virtual void PresentGUI(bool isActive)
    {
        DebugMessage((isActive ? "Presenting " : "Hiding ") + gameObject.name);

        ActivateControls(isActive);
        StartCoroutine(FadeCanvasGroup(isActive));
    }

    public void PlayButtonSound()
    {
        _maestro.PlayOneShot(ButtonSound);
    }

    #endregion Hooks

    #region Methods

    protected IEnumerator FadeCanvasGroup(bool isActive)
    {
        CanvasGroup group = GetComponent<CanvasGroup>();

        float actualThreshold = isActive ? 1 - FadeThreshold : FadeThreshold;
        if (isActive)
        {
            while (group.alpha < actualThreshold)
            {
                group.alpha = Mathf.Lerp(group.alpha, DetermineOpacity(isActive), FadeRate);
                yield return 0;
            }
            group.alpha = 1.0f;
        }
        else
        {
            while (group.alpha > actualThreshold)
            {
                group.alpha = Mathf.Lerp(group.alpha, DetermineOpacity(isActive), FadeRate);
                yield return 0;
            }
            group.alpha = 0.0f;
        }
    }

    public void ActivateButton(Button button, bool isActive)
    {
        DebugMessage("Turning button " + button.gameObject.name + " " + (isActive ? "on" : "off"));

        button.interactable = isActive;
        button.enabled = isActive;

        Text childText = button.GetComponentInChildren<Text>();
        if (childText == null)
            return;

        childText.CrossFadeAlpha(DetermineOpacity(isActive), FadeRate, false);
    }

    protected void ActivateControls(bool isActive)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            Button current = _buttons[i];
            ActivateButton(current, isActive);
        }

        for (int i = 0; i < _toggles.Count; i++)
        {
            Toggle current = _toggles[i];
            DebugMessage("Turning toggle " + current.gameObject.name + " " + (isActive ? "on" : "off"));

            current.interactable = isActive;
            current.enabled = isActive;

            // Hide child text, too!
            Text childText = current.GetComponentInChildren<Text>();
            childText.CrossFadeAlpha(DetermineOpacity(isActive), FadeRate, false);
        }

        for (int i = 0; i < _sliders.Count; i++)
        {
            Slider current = _sliders[i];
            DebugMessage("Turning slider " + current.gameObject.name + " " + (isActive ? "on" : "off"));

            current.interactable = isActive;
            current.enabled = isActive;

            // Hide child text, too!
            Text childText = current.GetComponentInChildren<Text>();
            childText.CrossFadeAlpha(DetermineOpacity(isActive), FadeRate, false);
        }
    }

    protected float DetermineOpacity(bool isActive)
    {
        return isActive ? 1.0f : 0.0f;
    }

    #endregion Methods
}
