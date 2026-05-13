using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class StatusMetadataValidator
    {
        static readonly HashSet<string> StatusMetadataRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "uiIconSpriteId",
            "type",
            "isCureable",
            "isIncludeInCursePool",
            "stageAvailableFrom",
            "lockLevel",
            "extras",
            "efficacy",
            "blockedFrom",
            "forceDeactivates",
            "escapeTools",
            "animationOwned",
            "bodyProfileId",
            "bodyProfileHardOverride",
            "visibleTracks",
            "stackTrackRules",
            "overrides",
        };

        static readonly HashSet<string> StatusMetadataOverrideFields = new(StringComparer.Ordinal)
        {
            "profileId",
            "uiIconSpriteId",
            "type",
            "isCureable",
            "isIncludeInCursePool",
            "stageAvailableFrom",
            "lockLevel",
            "extras",
            "efficacy",
            "blockedFrom",
            "forceDeactivates",
            "escapeTools",
            "animationOwned",
            "bodyProfileId",
            "bodyProfileHardOverride",
            "visibleTracks",
            "stackTrackRules",
        };

        static readonly HashSet<string> StatusMetadataStackTrackRuleFields = new(StringComparer.Ordinal)
        {
            "minStack",
            "trackId",
        };

        readonly ValidationReport report;

        public StatusMetadataValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: statusMetadata entry folder has no .json files.");
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
                ValidateObject(relativePath, obj, "statusMetadata");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateObject(relativePath, entry, $"statusMetadata[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: statusMetadata[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: statusMetadata file should contain an object or an array of objects.");
        }

        void ValidateObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, StatusMetadataRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "uiIconSpriteId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "type", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "isCureable", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "isIncludeInCursePool", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "stageAvailableFrom", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "lockLevel", report, scope);
            JsonValidationUtil.WarnObject(relativePath, obj, "extras", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "efficacy", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "blockedFrom", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "forceDeactivates", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "escapeTools", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "animationOwned", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "bodyProfileId", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "bodyProfileHardOverride", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "visibleTracks", report, scope);
            ValidateStackTrackRules(relativePath, obj["stackTrackRules"], $"{scope}.stackTrackRules");
            ValidateOverrides(relativePath, obj["overrides"], $"{scope}.overrides");
        }

        void ValidateOverrides(string relativePath, JToken overridesToken, string scope)
        {
            if (overridesToken == null)
                return;

            if (overridesToken is not JArray overrides)
            {
                report.Warn($"{relativePath}: {scope} should be an array.");
                return;
            }

            for (int i = 0; i < overrides.Count; i++)
            {
                string entryScope = $"{scope}[{i}]";
                if (overrides[i] is not JObject entry)
                {
                    report.Warn($"{relativePath}: {entryScope} should be an object.");
                    continue;
                }

                JsonValidationUtil.ValidateUnknownFields(relativePath, entry, StatusMetadataOverrideFields, entryScope, report);
                JsonValidationUtil.RequireString(relativePath, entry, "profileId", report, entryScope);
                JsonValidationUtil.WarnString(relativePath, entry, "uiIconSpriteId", report, entryScope);
                JsonValidationUtil.WarnString(relativePath, entry, "type", report, entryScope);
                JsonValidationUtil.WarnBoolean(relativePath, entry, "isCureable", report, entryScope);
                JsonValidationUtil.WarnBoolean(relativePath, entry, "isIncludeInCursePool", report, entryScope);
                JsonValidationUtil.WarnInteger(relativePath, entry, "stageAvailableFrom", report, entryScope);
                JsonValidationUtil.WarnInteger(relativePath, entry, "lockLevel", report, entryScope);
                JsonValidationUtil.WarnObject(relativePath, entry, "extras", report, entryScope);
                JsonValidationUtil.WarnStringArray(relativePath, entry, "efficacy", report, entryScope);
                JsonValidationUtil.WarnStringArray(relativePath, entry, "blockedFrom", report, entryScope);
                JsonValidationUtil.WarnStringArray(relativePath, entry, "forceDeactivates", report, entryScope);
                JsonValidationUtil.WarnStringArray(relativePath, entry, "escapeTools", report, entryScope);
                JsonValidationUtil.WarnBoolean(relativePath, entry, "animationOwned", report, entryScope);
                JsonValidationUtil.WarnString(relativePath, entry, "bodyProfileId", report, entryScope);
                JsonValidationUtil.WarnBoolean(relativePath, entry, "bodyProfileHardOverride", report, entryScope);
                JsonValidationUtil.WarnStringArray(relativePath, entry, "visibleTracks", report, entryScope);
                ValidateStackTrackRules(relativePath, entry["stackTrackRules"], $"{entryScope}.stackTrackRules");
            }
        }

        void ValidateStackTrackRules(string relativePath, JToken rulesToken, string scope)
        {
            if (rulesToken == null)
                return;

            if (rulesToken is not JArray rules)
            {
                report.Warn($"{relativePath}: {scope} should be an array.");
                return;
            }

            for (int i = 0; i < rules.Count; i++)
            {
                string entryScope = $"{scope}[{i}]";
                if (rules[i] is not JObject rule)
                {
                    report.Warn($"{relativePath}: {entryScope} should be an object.");
                    continue;
                }

                JsonValidationUtil.ValidateUnknownFields(relativePath, rule, StatusMetadataStackTrackRuleFields, entryScope, report);
                JsonValidationUtil.WarnInteger(relativePath, rule, "minStack", report, entryScope);
                JsonValidationUtil.RequireString(relativePath, rule, "trackId", report, entryScope);
            }
        }
    }
}
