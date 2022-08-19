using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPlayer : MonoBehaviour
{
    [SerializeField] private FlexibleColorPicker fcp;

    [SerializeField] private List<Image> playerPart;
    [SerializeField] private List<Player.PlayerColorableParts> playerPartName;
    // 0 = eye L
    // 1 = eye R
    // 2 = head
    // 3 = body

    public List<Image> PlayerParts { get => playerPart; }
    public List<Player.PlayerColorableParts> PlayerPartName { get => playerPartName; }

    private int currentIndex;

    private void Start()
    {
        Dropdown dropdown = this.transform.GetComponent<Dropdown>();

        playerPart ??= new List<Image>();

        dropdown.options.Clear();

        for (int i = 0; i < playerPart.Count; i++)
        {
            AddDropDownOption(dropdown, playerPart[i].gameObject.name);
        }

        string[] loadedColors = SaveLoadManager.Instance.LoadColors();

        if (loadedColors != null)
        {
            Color c;
            for (int i = 0; i < loadedColors.Length; i++)
            {
                if (loadedColors[i] != null)
                {
                    ColorUtility.TryParseHtmlString(loadedColors[i], out c);
                    playerPart[i].color = c;
                }
            }
        }

        OnDropDownValueChange(dropdown);

        dropdown.onValueChanged.AddListener(delegate { OnDropDownValueChange(dropdown); });
    }

    private void AddDropDownOption(Dropdown dropdown, string _text)
    {
        dropdown.options.Add(new Dropdown.OptionData() { text = _text });
    }

    private void OnDropDownValueChange(Dropdown dropdown)
    {
        currentIndex = dropdown.value;

        if (currentIndex >= 0 && playerPart.Count >= currentIndex)
            fcp.color = playerPart[currentIndex].color;
    }

    private void Update()
    {
        if (currentIndex >= 0 && playerPart.Count >= currentIndex)
            playerPart[currentIndex].color = fcp.color;

    }

    public void ResetColors()
    {
        foreach (Image part in playerPart)
            part.color = Color.white;
    }

    public void CallSave()
    {
        // ptdr ça pue la merde
        SaveLoadManager.Instance.SaveColors(playerPart[0].color, playerPart[1].color, playerPart[2].color, playerPart[3].color);
    }
}
