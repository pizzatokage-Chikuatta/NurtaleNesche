#if UNITY_EDITOR
using NurtaleNesche.Modding.Validation;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Unity Editor adapter for the v1 experimental mod validator.
/// Keep this separate from developer-only export tools so it can be packaged with
/// the public validator core later.
/// </summary>
public static class ModdingV1ValidationReporter
{
    const string ReportAssetPath = "Assets/Scripts/MODDING_V1_VALIDATION_REPORT.md";

    [MenuItem("Tools/Mod Data/Validate V1 Experimental Mods")]
    public static void Validate()
    {
        string projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
        string modsRoot = string.IsNullOrWhiteSpace(projectRoot)
            ? null
            : Path.Combine(projectRoot, "Mods");

        var options = new ValidationOptions
        {
            ResolveCodeManifestTypes = true,
            LoadDllsFromModsRootForTypeResolution = true,
        };

        ValidationReport report = ModdingV1ValidationCore.Validate(modsRoot, projectRoot, options);
        WriteReport(report);
    }

    static void WriteReport(ValidationReport report)
    {
        string absoluteReportPath = Path.GetFullPath(ReportAssetPath);
        Directory.CreateDirectory(Path.GetDirectoryName(absoluteReportPath) ?? ".");
        File.WriteAllText(absoluteReportPath, report.ToMarkdown());
        AssetDatabase.Refresh();

        Debug.Log($"[ModdingV1ValidationReporter] Wrote report: {ReportAssetPath}. Errors={report.ErrorCount}, Warnings={report.WarningCount}");
        EditorUtility.DisplayDialog(
            "V1 Experimental Mod Validation",
            $"Report written to:\n{ReportAssetPath}\n\nErrors: {report.ErrorCount}\nWarnings: {report.WarningCount}",
            "OK");
    }
}
#endif
