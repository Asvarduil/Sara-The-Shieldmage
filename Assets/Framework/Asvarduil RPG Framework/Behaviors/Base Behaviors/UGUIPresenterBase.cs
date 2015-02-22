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
    protected List<Text> _labels = new List<Text>();
    protected List<Image> _images = new List<Image>();
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

        DebugMessage("Presenter " + gameObject.name
                     + " has " + _buttons.Count + " buttons,"
                     + " and a total of " + _children.Count + " child objects.");
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

    protected IEnumerator FadeCanvasGroup(bool isActive)
    {
        CanvasGroup group = GetComponent<CanvasGroup>();

        float actualThreshold = isActive ? 1 - FadeThreshold : FadeThreshold;
        if (isActive)
        {
            ActivateControls(isActive);
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
            ActivateControls(isActive);
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

        Text childText = button.GetComponentInChildren<Text>();
        if (childText == null)
            return;

        childText.enabled = isActive;
    }

    public void ActivateSlider(Slider slider, bool isActive)
    {
        DebugMessage("Turning slider " + slider.gameObject.name + " " + (isActive ? "on" : "off"));

        slider.interactable = isActive;
        slider.enabled = isActive;

        // Hide child text, too!
        Text childText = slider.GetComponentInChildren<Text>();
        childText.enabled = isActive;
    }

    public void ActivateToggle(Toggle toggle, bool isActive)
    {
        DebugMessage("Turning toggle " + toggle.gameObject.name + " " + (isActive ? "on" : "off"));

        toggle.interactable = isActive;
        toggle.enabled = isActive;

        // Hide child text, too!
        Text childText = toggle.GetComponentInChildren<Text>();
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
