using SurvivorDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAchivement : AbstractAchivement
{
    public TutorialAchivement(List<SaveableAchievement> achivements) : base("Objective_Tutorial",achivements)
    {
        base.maxProgress = 1;
    }

    public new void Progress(int progress) => base.Progress(progress);

    public override string GetName() => CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_Tutorial_Name"];

    public override string GetDescription() => CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_Tutorial_Description"];

    protected override bool CanProgress(int value, object[] args)
    {
        foreach(var thing in args)
        {
            if(thing is TutorialArena)
            {
                return true;
            }
        }

        return false;
    }
}