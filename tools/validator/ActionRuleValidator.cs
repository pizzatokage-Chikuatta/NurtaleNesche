using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class ActionRuleValidator
    {
        static readonly HashSet<string> ActionRuleSetFields = new(StringComparer.Ordinal)
        {
            "profileId",
            "rules",
        };

        static readonly HashSet<string> ActionRuleFields = new(StringComparer.Ordinal)
        {
            "passCheckId",
            "blockIfAny",
            "blockIfMissing",
            "blockIfNeither",
            "forcePassIfAny",
            "reason",
        };

        readonly ValidationReport report;

        public ActionRuleValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: actionRules entry folder has no .json files.");
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
            if (token is not JObject obj)
            {
                report.Error($"{relativePath}: actionRules file should contain a JSON object.");
                return;
            }

            ValidateObject(relativePath, obj, "actionRules");
        }

        void ValidateObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ActionRuleSetFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "profileId", report, scope);
            ValidateRules(relativePath, obj["rules"], $"{scope}.rules");
        }

        void ValidateRules(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JArray arr)
            {
                report.Warn($"{relativePath}: {scope} should be an array.");
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                string entryScope = $"{scope}[{i}]";
                if (arr[i] is not JObject entry)
                {
                    report.Warn($"{relativePath}: {entryScope} should be an object.");
                    continue;
                }

                ValidateRule(relativePath, entry, entryScope);
            }
        }

        void ValidateRule(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ActionRuleFields, scope, report);
            WarnPassCheckId(relativePath, obj, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "blockIfAny", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "blockIfMissing", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "blockIfNeither", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "forcePassIfAny", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "reason", report, scope);
        }

        void WarnPassCheckId(string relativePath, JObject obj, string scope)
        {
            JToken token = obj["passCheckId"];
            if (token == null)
            {
                report.Warn($"{relativePath}: {scope}.passCheckId should be a string.");
                return;
            }

            if (token.Type != JTokenType.String)
            {
                report.Warn($"{relativePath}: {scope}.passCheckId should be a string.");
                return;
            }

            if (string.IsNullOrWhiteSpace(token.Value<string>()))
                report.Warn($"{relativePath}: {scope}.passCheckId cannot be empty.");
        }
    }
}
