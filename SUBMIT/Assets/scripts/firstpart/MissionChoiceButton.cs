using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MissionChoiceButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text optionName;
    [SerializeField] private TMP_Text optionDescription;
    [SerializeField] private Image background;

    [Header("Appearance")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField]
    private Color selectedColor =
        new Color(0.4f, 0.75f, 1f, 1f);

    public void Setup(
        string title,
        string description,
        UnityAction callback
    )
    {
        optionName.text = title;
        optionDescription.text = description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(callback);

        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (background != null)
        {
            background.color = selected
                ? selectedColor
                : normalColor;
        }
    }
}