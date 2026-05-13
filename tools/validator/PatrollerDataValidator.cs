using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class PatrollerDataValidator
    {
        static readonly HashSet<string> PatrollerDataRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "NameInAlphabet",
            "builtInPrefabAddress",
            "speechProfileId",
            "animationActorId",
            "animationSetId",
            "prefabSourcePatrollerId",
            "assetBundleFileName",
            "prefabAssetName",
            "assemblyFileName",
            "logicTypeName",
            "statusProfileId",
            "actionRuleProfileId",
            "dropTableProfileId",
            "shadowProfileId",
            "showUpStage",
            "uniqueDropTableIds",
            "patrolRegions",
            "positions",
            "flags",
            "stats",
            "inventories",
            "combat",
            "senses",
            "tasks",
            "extras",
        };

        static readonly HashSet<string> ProviderEntryFields = new(StringComparer.Ordinal)
        {
            "id",
            "enabled",
            "config",
        };

        static readonly HashSet<string> TaskOptionEntryFields = new(StringComparer.Ordinal)
        {
            "id",
            "enabled",
            "config",
        };

        readonly ValidationReport report;

        public PatrollerDataValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: patrollerData entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateFile(path);
        }

        void ValidateFile(string path)
        {
            JObject obj = JsonValidationUtil.TryReadObject(path, report);
            if (obj == null)
                return;

            string relativePath = report.ToRelativePath(path);
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, PatrollerDataRootFields, "patrollerData root", report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report);
            WarnString(relativePath, obj, "NameInAlphabet");
            WarnString(relativePath, obj, "builtInPrefabAddress");
            WarnString(relativePath, obj, "prefabSourcePatrollerId");
            WarnString(relativePath, obj, "assetBundleFileName");
            WarnString(relativePath, obj, "prefabAssetName");
            WarnString(relativePath, obj, "assemblyFileName");
            WarnString(relativePath, obj, "logicTypeName");
            WarnString(relativePath, obj, "statusProfileId");
            WarnString(relativePath, obj, "actionRuleProfileId");
            WarnString(relativePath, obj, "dropTableProfileId");
            WarnString(relativePath, obj, "shadowProfileId");
            WarnString(relativePath, obj, "speechProfileId");

            bool hasAnimationActorId = HasNonEmptyString(obj, "animationActorId");
            bool hasAnimationSetId = HasNonEmptyString(obj, "animationSetId");
            if (!hasAnimationActorId && !hasAnimationSetId)
                report.Warn($"{relativePath}: patrollerData should declare animationActorId or animationSetId.");
            WarnString(relativePath, obj, "animationActorId");
            WarnString(relativePath, obj, "animationSetId");

            WarnInteger(relativePath, obj, "showUpStage");
            WarnStringArray(relativePath, obj, "uniqueDropTableIds");
            WarnStringArray(relativePath, obj, "patrolRegions");
            WarnObject(relativePath, obj, "positions");
            WarnObject(relativePath, obj, "flags");
            WarnObject(relativePath, obj, "stats");
            WarnObject(relativePath, obj, "inventories");
            WarnObject(relativePath, obj, "combat");
            WarnObject(relativePath, obj, "extras");

            ValidateProviderList(relativePath, obj["senses"], "senses.providers", allowOptions: false);
            ValidateProviderList(relativePath, obj["tasks"], "tasks.providers", allowOptions: true);
        }

        void ValidateProviderList(string relativePath, JToken containerToken, string path, bool allowOptions)
        {
            if (containerToken == null)
                return;

            if (containerToken is not JObject container)
            {
                report.Warn($"{relativePath}: {path.Split('.')[0]} should be an object.");
                return;
            }

            JToken providersToken = container["providers"];
            if (providersToken == null)
                return;

            if (providersToken is not JArray providers)
            {
                report.Warn($"{relativePath}: {path} should be an array.");
                return;
            }

            for (int i = 0; i < providers.Count; i++)
            {
                string entryPath = $"{path}[{i}]";
                if (providers[i] is not JObject entry)
                {
                    report.Warn($"{relativePath}: {entryPath} should be an object.");
                    continue;
                }

                JsonValidationUtil.ValidateUnknownFields(relativePath, entry, ProviderEntryFields, entryPath, report);
                JsonValidationUtil.RequireString(relativePath, entry, "id", report, entryPath);
                JsonValidationUtil.WarnBoolean(relativePath, entry, "enabled", report, entryPath);

                JToken configToken = entry["config"];
                if (configToken != null && configToken is not JObject)
                    report.Warn($"{relativePath}: {entryPath}.config should be an object.");

                if (allowOptions && configToken is JObject config)
                    ValidateTaskOptions(relativePath, config["options"], $"{entryPath}.config.options");
            }
        }

        void ValidateTaskOptions(string relativePath, JToken optionsToken, string path)
        {
            if (optionsToken == null)
                return;

            if (optionsToken is not JArray options)
            {
                report.Warn($"{relativePath}: {path} should be an array.");
                return;
            }

            for (int i = 0; i < options.Count; i++)
            {
                string entryPath = $"{path}[{i}]";
                if (options[i] is not JObject entry)
                {
                    report.Warn($"{relativePath}: {entryPath} should be an object.");
                    continue;
                }

                JsonValidationUtil.ValidateUnknownFields(relativePath, entry, TaskOptionEntryFields, entryPath, report);
                JsonValidationUtil.RequireString(relativePath, entry, "id", report, entryPath);
                JsonValidationUtil.WarnBoolean(relativePath, entry, "enabled", report, entryPath);

                JToken configToken = entry["config"];
                if (configToken != null && configToken is not JObject)
                    report.Warn($"{relativePath}: {entryPath}.config should be an object.");
            }
        }

        static bool HasNonEmptyString(JObject obj, string field)
        {
            JToken token = obj?[field];
            return token != null && token.Type == JTokenType.String && !string.IsNullOrWhiteSpace(token.Value<string>());
        }

        void WarnString(string relativePath, JObject obj, string field)
        {
            JToken token = obj[field];
            if (token != null && token.Type != JTokenType.String)
                report.Warn($"{relativePath}: {field} should be a string.");
        }

        void WarnInteger(string relativePath, JObject obj, string field)
        {
            JToken token = obj[field];
            if (token != null && token.Type != JTokenType.Integer)
                report.Warn($"{relativePath}: {field} should be an integer.");
        }

        void WarnObject(string relativePath, JObject obj, string field)
        {
            JToken token = obj[field];
            if (token != null && token is not JObject)
                report.Warn($"{relativePath}: {field} should be an object.");
        }

        void WarnStringArray(string relativePath, JObject obj, string field)
        {
            JToken token = obj[field];
            if (token == null)
                return;

            if (token is not JArray arr)
            {
                report.Warn($"{relativePath}: {field} should be an array of strings.");
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].Type != JTokenType.String)
                    report.Warn($"{relativePath}: {field}[{i}] should be a string.");
            }
        }
    }
}
