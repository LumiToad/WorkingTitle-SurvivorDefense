using SurvivorDTO;
using System.Collections.Generic;
using UnityEngine;

public class KillAchivement : AbstractAchivement
{
    private string glueTargetName;
    private AchivementDifficulty difficulty;

    public KillAchivement(List<SaveableAchievement> allAchivementsDatas, string glueTargetName, AchivementDifficulty difficulty, int killAmount) : base($"Objective_Kill_{glueTargetName}_{killAmount}_{difficulty}", allAchivementsDatas)
    {
        this.glueTargetName = glueTargetName;
        this.difficulty = difficulty;

        base.maxProgress = killAmount;
    }

    public bool TryProgress(AbstractEnemy target, int progress)
    {
        if(target.glueName == glueTargetName)
        {
            base.Progress(progress);
            return true;
        }

        return false;
    }

    public override string GetName()
    {
        var name = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_Kill_Name"];
        name = name.Replace("<m1>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Enemies", SelectedLanguage.value)[glueTargetName]);
        name = name.Replace("<m2>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)[difficulty.ToString()]);

        return name;
    }

    public override string GetDescription()
    {
        var description = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Objective_Kill_Description"];
        description = description.Replace("<m1>", base.maxProgress.ToString());
        description = description.Replace("<m2>", CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Enemies", SelectedLanguage.value)[glueTargetName]);
        description = description.Replace("<m3>", (maxProgress - progress).ToString());

        return description;
    }

    protected override bool CanProgress(int value, object[] args)
    {
        foreach (var thing in args)
        {
            if (thing is AbstractEnemy && ((AbstractEnemy)thing).glueName == this.glueTargetName)
            {
                return true;
            }
        }

        return false;
    }
}
