using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System;
using System.Collections;
using System.Resources;
using Resources;
using System.Diagnostics;
using System.Data;
using PowerPOS;

/// <summary>
/// Class to manage language on the website
/// </summary>
public sealed class LanguageManager
{
    /// <summary>
    /// Default CultureInfo
    /// </summary>
    public static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

    /// <summary>
    /// Available CultureInfo that according resources can be found
    /// </summary>
    public static readonly CultureInfo[] AvailableCultures;

    static LanguageManager()
    {
        //
        // Available Cultures
        //
        List<string> availableResources = new List<string>();
        string resourcespath = Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "App_GlobalResources");
        DirectoryInfo dirInfo = new DirectoryInfo(resourcespath);
        foreach (FileInfo fi in dirInfo.GetFiles("*.*.resx", SearchOption.AllDirectories))
        {
            //Take the cultureName from resx filename, will be smt like en-US
            string cultureName = Path.GetFileNameWithoutExtension(fi.Name); //get rid of .resx
            if (cultureName.LastIndexOf(".") == cultureName.Length - 1)
                continue; //doesnt accept format FileName..resx
            cultureName = cultureName.Substring(cultureName.LastIndexOf(".") + 1);
            availableResources.Add(cultureName);
        }

        List<CultureInfo> result = new List<CultureInfo>();
        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            //If language file can be found
            if (availableResources.Contains(culture.ToString()))
            {
                result.Add(culture);
            }
        }

        AvailableCultures = result.ToArray();

        //
        // Current Culture
        //
        CurrentCulture = DefaultCulture;
        // If default culture is not available, take another available one to use
        if (!result.Contains(DefaultCulture) && result.Count>0)
        {
            CurrentCulture = result[0];
        }
    }

    /// <summary>
    /// Current selected culture
    /// </summary>
    public static CultureInfo CurrentCulture
    {
        get { return Thread.CurrentThread.CurrentCulture; }
        set
        {
            //NOTE:
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-A"); //correct
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr"); //correct
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-A"); //correct as we have given locale 
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr"); //wrong, will not work
            Thread.CurrentThread.CurrentUICulture = value;
            Thread.CurrentThread.CurrentCulture = value;
        }
    }

    public static string GetTranslation(string txt)
    {
        string result = txt;
        try
        {
            bool isFound = false;
            ResourceSet resourceSet = dictionary.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry item in resourceSet)
            {
                if (txt.ToLower().Trim().Equals((item.Key + "").ToLower().Trim()))
                {
                    result = item.Value + "";
                    isFound = true;
                    break;
                }
            }
            if (!isFound)
                Debug.WriteLine(txt);
        }
        catch (Exception ex)
        {
 
        }

        return result;
    }

    public static DataTable GetTranslation(DataTable data, params int[] index)
    {
        DataTable result = data.Copy();

        try
        {
            for (int i = 0; i < index.Length; i++)
            {
                result.Columns[index[i]].ColumnName = GetTranslation(result.Columns[index[i]].ColumnName);
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return result;
    }
}