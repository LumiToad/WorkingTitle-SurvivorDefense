using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private XPBar xpBar;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI playerLevel;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI kills;
    [SerializeField, FoldoutGroup("References")]
    private DashUI dashUI;

    [SerializeField]
    [FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;

    private void Awake()
    {
        xpBar = GetComponentInChildren<XPBar>();
    }

    public void SetXPBar(float current, float max) => xpBar.UpdateValue(current, max); 
    public void SetLevel(int value)
    {
        var text = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)["GlueLevel"];
        text = text.Replace("<m1>", value.ToString());
        playerLevel.text = text;
    }

    public void SetKils(int value) => kills.text = $"kills: {value.ToString()}";

    public void SetDashCD(float dashCooldown) => dashUI.DashCooldown(dashCooldown);
}
