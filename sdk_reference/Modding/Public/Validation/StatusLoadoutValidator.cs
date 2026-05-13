using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class StatusLoadoutValidator
    {
        static readonly HashSet<string> StatusLoadoutRootFields = new(StringComparer.Ordinal)
        {
            "profileId",
            "allowedStatusEffects",
            "alwaysActiveStatusEffects",
        };

        readonly ValidationReport report;

        public StatusLoadoutValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: statusLoadouts entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateFile(path);
        }

        void ValidateFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateObject(relativePath, obj, "statusLoadout");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateObject(relativePath, entry, $"statusLoadout[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: statusLoadout[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: statusLoadout file should contain an object or an array of objects.");
        }

        void ValidateObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, StatusLoadoutRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "profileId", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "allowedStatusEffects", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "alwaysActiveStatusEffects", report, scope);
        }
    }
}
