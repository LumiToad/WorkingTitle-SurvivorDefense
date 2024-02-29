using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    private List<LevelSection> sections = new();

    private LevelSection currentSection;

    [SerializeField]
    private LevelSection startInSection;

    private void Awake()
    {
        int startSectionIndex = 0;
        foreach (var section in GetComponentsInChildren<LevelSection>())
        {
            sections.Add(section);    
        }
        
        if (startInSection != null)
        {
            startSectionIndex = sections.IndexOf(startInSection);
            sections.RemoveRange(0, startSectionIndex);
        }

        foreach (var section in sections)
        {
            section.Completed += OnSectionCompleted;
        }

        currentSection = sections.First();
    }

    private void Start()
    {
        foreach(var section in sections)
        {
            section.SetUp();
        }

        currentSection.StartSection(sections.IndexOf(currentSection));

        for(int i = 0;i < sections.Count; i++)
        {
            sections[i].LevelSectionUpdate(i);
        }
    }

    private void OnSectionCompleted()
    {
        var index = sections.IndexOf(currentSection);
        index++;

        if (index >= sections.Count)
        {
            GameUI.ShowVictoryScreen();
            Time.timeScale = 0;
            return;
        }

        currentSection = sections[index];
        currentSection.StartSection(index);

        for (int i = index; i < sections.Count; i++)
        {
            sections[i].LevelSectionUpdate(i - index);
        }
    }
}
