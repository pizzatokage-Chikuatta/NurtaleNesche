using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class PotionEffectValidator
    {
        static readonly HashSet<string> PotionEffectRootFields = new(StringComparer.Ordinal)
        {
            "potionId",
            "onDrink",
            "onHit",
        };

        static readonly HashSet<string> PotionUseConfigFields = new(StringComparer.Ordinal)
        {
            "consumeItem",
            "stunTime",
            "sfxId",
            "vfxId",
            "actions",
        };

        static readonly HashSet<string> PotionActionFields = new(StringComparer.Ordinal)
        {
            "type",
            "effectId",
            "stringValue",
            "amount",
            "floatValue",
        };

        readonly ValidationReport report;

        public PotionEffectValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: potionEffects entry folder has no .json files.");
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
                ValidateObject(relativePath, obj, "potionEffect");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateObject(relativePath, entry, $"potionEffect[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: potionEffect[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: potionEffects file should contain an object or an array of objects.");
        }

        void ValidateObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, PotionEffectRootFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "potionId", report, scope);

            if (obj["onDrink"] == null && obj["onHit"] == null)
                report.Error($"{relativePath}: {scope} should define at least one of onDrink or onHit.");

            ValidateUseConfig(relativePath, obj["onDrink"], $"{scope}.onDrink");
            ValidateUseConfig(relativePath, obj["onHit"], $"{scope}.onHit");
        }

        void ValidateUseConfig(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object.");
                return;
            }

            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, PotionUseConfigFields, scope, report);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "consumeItem", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "stunTime", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "sfxId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "vfxId", report, scope);
            ValidateActions(relativePath, obj["actions"], $"{scope}.actions");
        }

        void ValidateActions(string relativePath, JToken token, string scope)
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

                JsonValidationUtil.ValidateUnknownFields(relativePath, entry, PotionActionFields, entryScope, report);
                WarnRequiredActionType(relativePath, entry, entryScope);
                JsonValidationUtil.WarnString(relativePath, entry, "effectId", report, entryScope);
                JsonValidationUtil.WarnString(relativePath, entry, "stringValue", report, entryScope);
                JsonValidationUtil.WarnInteger(relativePath, entry, "amount", report, entryScope);
                JsonValidationUtil.WarnNumber(relativePath, entry, "floatValue", report, entryScope);
            }
        }

        void WarnRequiredActionType(string relativePath, JObject obj, string scope)
        {
            JToken token = obj["type"];
            if (token == null)
            {
                report.Warn($"{relativePath}: {scope}.type should be a string.");
                return;
            }

            if (token.Type != JTokenType.String)
            {
                report.Warn($"{relativePath}: {scope}.type should be a string.");
                return;
            }

            if (string.IsNullOrWhiteSpace(token.Value<string>()))
                report.Warn($"{relativePath}: {scope}.type cannot be empty.");
        }
    }
}
