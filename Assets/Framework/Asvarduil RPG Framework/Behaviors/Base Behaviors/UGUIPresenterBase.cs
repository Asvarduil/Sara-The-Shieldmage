using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UGUIPresenterBase : DebuggableBehavior 
{
    #region Variables / Properties

    public float FadeRate = 0.5f;

    protected Maestro _maestro;

    protected List<GameObject> _children = new List<GameObject>();
    protected List<Button> _buttons = new List<Button>();
    protected List<Image> _images = new List<Image>();
    protected List<Text> _labels = new List<Text>();

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
            Image image = current.GetComponent<Image>();

            _children.Add(current.gameObject);

            if (button != null)
                _buttons.Add(button);

            if (image != null)
                _images.Add(image);
        }

        DebugMessage("Presenter " + gameObject.name
                     + " has " + _buttons.Count + " buttons, " + _images.Count + " images,"
                     + " and a total of " + _children.Count + " child objects.");
    }

    public virtual void PresentGUI(bool isActive)
    {
        DebugMessage((isActive ? "Presenting " : "Hiding ") + gameObject.name);

        ActivateButtons(isActive);
        ActivateImages(isActive);
    }

    #endregion Hooks

    #region Methods

    protected void ActivateImages(bool isActive)
    {
        for (int i = 0; i < _images.Count; i++)
        {
            Image current = _images[i];
            DebugMessage("Fading " + current.gameObject.name + " " + (isActive ? "in" : "out"));

            current.CrossFadeAlpha(DetermineOpacity(isActive), FadeRate, false);
        }
    }

    protected void ActivateButtons(bool isActive)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            Button current = _buttons[i];
            DebugMessage("Turning button " + current.gameObject.name + " " + (isActive ? "on" : "off"));
            
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
