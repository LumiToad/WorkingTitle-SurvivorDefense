using Sirenix.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class CSVLanguageFileParser
{
    //This CSV Parser uses Google Sheets structure of CSV
    //Columns are separated with a simple comma
    //Lines with '\n' | '\r'
    //An Escape character for commas is required

    public static Dictionary<string, string> GetLangDictionary(string path, LanguageType language)
    {
        string langKey = GetLanguageKey(language);

        Dictionary<string, string> retVal = new Dictionary<string, string>();
        string[] csv = ReadTextFromCSV(path);

        if (csv[0].IsNullOrWhitespace())
        {
            Debug.LogWarning("CSV broken or empty!");
            return null;
        }

        retVal.Add("lang", langKey);
        retVal.Add("Meta", GetMetaInfo(csv));

        int langIndex = GetLangIndex(langKey, csv);

        if (langIndex == -1)
        {
            Debug.LogWarning("Language could not be found in CSV! Did you set the Language Code correctly? (DE, EN,...)");
            return null;
        }

        for (int i = 1; i < csv.Length; i++)
        {
            string[] column = csv[i].Split(',');
            if (column.Length <= langIndex || retVal.ContainsKey(column[0])) continue;
            column[langIndex] = column[langIndex].Replace("//.", ",");
            retVal.Add(column[0], column[langIndex]);
        }

        return retVal;
    }

    private static string[] ReadTextFromCSV(string path)
    {
        return Resources.Load<TextAsset>(path).text.Split('\n');
    }

    private static int GetLangIndex(string langKey, string[] csv)
    {
        string[] langRow = csv[0].Split(',');

        for (int i = 1; i < langRow.Length; i++)
        {
            if (langRow[i].Trim('\n', '\r') == langKey)
            {
                return i;
            }
        }

        return -1;
    }

    private static string GetMetaInfo(string[] csv)
    {
        return csv[0].Split(',')[0];
    }

    private static string GetLanguageKey(LanguageType language)
    {
        string retVal = string.Empty;

        switch (language)
        {
            default:
                Debug.LogWarning("LANGUAGE NOT SPECIFIED (ADD TO GetLanguageKey first)!");
                break;
            case LanguageType.None:
                Debug.LogWarning("NO LANGUAGE SELECTED!");
                break;
            case LanguageType.DE:
                retVal = "DE";
                break;
            case LanguageType.EN:
                retVal = "EN";
                break;
            case LanguageType.FR:
                retVal = "FR";
                break;
        }

        return retVal;
    }
}
