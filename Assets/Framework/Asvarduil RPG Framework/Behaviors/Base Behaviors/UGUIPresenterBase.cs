using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUIPresenterBase : DebuggableBehavior 
{
    #region Variables / Properties

    public float FadeRate = 0.5f;
    public float FadeThreshold = 0.05f;

    public AudioClip ButtonSound;

    protected bool _isFading = false;
    protected Maestro _maestro;

    protected CanvasGroup _group;
    protected List<GameObject> _children = new List<GameObject>();
    protected List<Text> _labels = new List<Text>();
    protected List<Image> _images = new List<Image>();
    protected List<Button> _buttons = new List<Button>();
    protected List<Toggle> _toggles = new List<Toggle>();
    protected List<Slider> _sliders = new List<Slider>();

    protected CanvasGroup Group
    {
        get 
        { 
            if(_group == null)
                _group = GetComponent<CanvasGroup>();

            // Second null check is intentional.
            if (_group == null)
                throw new Exception("Game Object " + gameObject.name + " requires a CanvasGroup to use a UGUI Presenter behavior!");

            return _group;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public virtual void Start()
    {
        _maestro = Maestro.Instance;

        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform current = transform.GetChild(i);
            Text text = current.GetComponent<Text>();
            Image image = current.GetComponent<Image>();
            Button button = current.GetComponent<Button>();
            Toggle toggle = current.GetComponent<Toggle>();
            Slider slider = current.GetComponent<Slider>();

            _children.Add(current.gameObject);

            if (text != null)
                _labels.Add(text);

            if (image != null)
                _images.Add(image);

            if (button != null)
                _buttons.Add(button);

            if (toggle != null)
                _toggles.Add(toggle);

            if (slider != null)
                _sliders.Add(slider);
        }

        DebugMessage("Presenter " + gameObject.name + " has a total of " + _children.Count + " child objects.");
    }

    public virtual void PresentGUI(bool isActive)
    {
        DebugMessage((isActive ? "Presenting " : "Hiding ") + gameObject.name);
        StartCoroutine(FadeCanvasGroup(isActive));
    }

    public void PlayButtonSound()
    {
        _maestro.PlayOneShot(ButtonSound);
    }

    #endregion Hooks

    #region Methods

    public void HideCanvasGroup()
    {
        Group.alpha = 0.0f;
        ActivateControls(false);
    }

    public void ShowCanvasGroup()
    {
        Group.alpha = 1.0f;
        ActivateControls(true);
    }

    protected IEnumerator FadeCanvasGroup(bool isActive)
    {
        // Allow another fade to finish, before initiating a new fade.
        while (_isFading)
            yield return 0;

        CanvasGroup group = Group;

        _isFading = true;
        float actualThreshold = isActive ? 1 - FadeThreshold : FadeThreshold;
        if (isActive)
        {
            ActivateControls(isActive);
            while (group.alpha < actualThreshold)
            {
                group.alpha = Mathf.Lerp(group.alpha, DetermineOpacity(isActive), FadeRate);
                yield return 0;
            }

            _isFading = false;
            group.alpha = 1.0f;
            DebugMessage("Fade-in complete.");
        }
        else
        {
            while (Group.alpha > actualThreshold)
            {
                group.alpha = Mathf.Lerp(group.alpha, DetermineOpacity(isActive), FadeRate);
                yield return 0;
            }

            _isFading = false;
            group.alpha = 0.0f;
            ActivateControls(isActive);
            DebugMessage("Fade-out complete.");
        }
    }

    public void ActivateText(Text label, bool isActive)
    {
        DebugMessage("Turning image " + label.gameObject.name + " " + (isActive ? "on" : "off"));

        label.enabled = isActive;
    }

    public void ActivateImage(Image image, bool isActive)
    {
        DebugMessage("Turning image " + image.gameObject.name + " " + (isActive ? "on" : "off"));

        image.enabled = isActive;
    }

    public void ActivateButton(Button button, bool isActive)
    {
        DebugMessage("Turning button " + button.gameObject.name + " " + (isActive ? "on" : "off"));

        button.interactable = isActive;
        button.enabled = isActive;

        Image image = button.GetComponent<Image>();
        image.enabled = isActive;

        HideChildText(button, isActive);
    }

    public void ActivateSlider(Slider slider, bool isActive)
    {
        DebugMessage("Turning slider " + slider.gameObject.name + " " + (isActive ? "on" : "off"));

        slider.interactable = isActive;
        slider.enabled = isActive;

        HideChildText(slider, isActive);
    }

    public void ActivateToggle(Toggle toggle, bool isActive)
    {
        DebugMessage("Turning toggle " + toggle.gameObject.name + " " + (isActive ? "on" : "off"));

        toggle.interactable = isActive;
        toggle.enabled = isActive;

        HideChildText(toggle, isActive);
    }

    protected void HideChildText(Selectable component, bool isActive)
    {
        Text childText = component.GetComponentInChildren<Text>();
        if (childText == null)
            return;

        childText.enabled = isActive;
    }

    protected void ActivateControls(bool isActive)
    {
        for (int i = 0; i < _labels.Count; i++)
        {
            Text current = _labels[i];
            ActivateText(current, isActive);
        }

        for (int i = 0; i < _images.Count; i++)
        {
            Image current = _images[i];
            ActivateImage(current, isActive);
        }

        for (int i = 0; i < _buttons.Count; i++)
        {
            Button current = _buttons[i];
            ActivateButton(current, isActive);
        }

        for (int i = 0; i < _toggles.Count; i++)
        {
            Toggle current = _toggles[i];
            ActivateToggle(current, isActive);
        }

        for (int i = 0; i < _sliders.Count; i++)
        {
            Slider current = _sliders[i];
            ActivateSlider(current, isActive);
        }
    }

    protected float DetermineOpacity(bool isActive)
    {
        return isActive ? 1.0f : 0.0f;
    }

    #endregion Methods
}
