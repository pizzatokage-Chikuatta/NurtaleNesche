using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NurtaleNesche.Modding.Validation
{
    /// <summary>
    /// UnityEditor-free core for validating v1 experimental mod folders.
    /// Editor windows, menu items, and future standalone tools should call this class
    /// instead of duplicating validation rules.
    /// </summary>
    public static class ModdingV1ValidationCore
    {
        public const string SupportedExperimentalApiVersion = "v1";

        public static ValidationReport Validate(string modsRoot, string projectRoot = null)
        {
            return Validate(modsRoot, projectRoot, null);
        }

        public static ValidationReport Validate(string modsRoot, string projectRoot, ValidationOptions options)
        {
            var report = new ValidationReport(modsRoot, projectRoot);
            options ??= new ValidationOptions();

            if (string.IsNullOrWhiteSpace(modsRoot) || !Directory.Exists(modsRoot))
            {
                report.Warn("Mods folder does not exist. Nothing to validate.");
                return report;
            }

            new ModManifestValidator(report).Validate(modsRoot);
            new CodeManifestValidator(report, options, modsRoot).Validate(modsRoot);
            return report;
        }
    }

    public sealed class ValidationOptions
    {
        public bool ResolveCodeManifestTypes { get; set; }
        public bool LoadDllsFromModsRootForTypeResolution { get; set; }
        public IReadOnlyList<string> AdditionalAssemblyPaths { get; set; } = Array.Empty<string>();
    }

    public sealed class ValidationReport
    {
        readonly List<string> infos = new();
        readonly List<string> warnings = new();
        readonly List<string> errors = new();
        readonly string modsRoot;
        readonly string projectRoot;

        public ValidationReport(string modsRoot, string projectRoot)
        {
            this.modsRoot = modsRoot;
            this.projectRoot = projectRoot;
        }

        public int WarningCount => warnings.Count;
        public int ErrorCount => errors.Count;
        public IReadOnlyList<string> Infos => infos;
        public IReadOnlyList<string> Warnings => warnings;
        public IReadOnlyList<string> Errors => errors;

        public void Info(string message) => infos.Add(message);
        public void Warn(string message) => warnings.Add(message);
        public void Error(string message) => errors.Add(message);

        public string ToRelativePath(string path)
        {
            if (string.IsNullOrWhiteSpace(projectRoot))
                return path;

            string fullPath = Path.GetFullPath(path);
            string relative = Path.GetRelativePath(projectRoot, fullPath);
            return relative.Replace('\\', '/');
        }

        public string ToMarkdown()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Modding V1 Experimental Validation Report");
            sb.AppendLine();
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Mods root: `{modsRoot}`");
            sb.AppendLine($"Supported experimentalApiVersion: `{ModdingV1ValidationCore.SupportedExperimentalApiVersion}`");
            sb.AppendLine();
            sb.AppendLine($"Errors: {ErrorCount}");
            sb.AppendLine($"Warnings: {WarningCount}");
            sb.AppendLine();
            AppendSection(sb, "Errors", errors);
            AppendSection(sb, "Warnings", warnings);
            AppendSection(sb, "Info", infos);
            return sb.ToString();
        }

        static void AppendSection(StringBuilder sb, string title, IReadOnlyList<string> items)
        {
            sb.AppendLine($"## {title}");
            sb.AppendLine();
            if (items.Count == 0)
            {
                sb.AppendLine("- None.");
                sb.AppendLine();
                return;
            }

            foreach (string item in items)
                sb.AppendLine($"- {item}");

            sb.AppendLine();
        }
    }
}
