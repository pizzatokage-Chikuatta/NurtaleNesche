using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class PlayerStatValidator
    {
        static readonly HashSet<string> PlayerStatRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "displayName",
            "actionRuleProfileId",
            "shadowProfileId",
            "vitals",
            "movement",
            "interaction",
            "inventory",
            "sexualHeat",
            "extras",
        };

        static readonly HashSet<string> VitalFields = new(StringComparer.Ordinal)
        {
            "maxHealth",
            "healthCap",
            "maxStamina",
            "staminaCap",
            "staminaRestoreAmount",
        };

        static readonly HashSet<string> MovementFields = new(StringComparer.Ordinal)
        {
            "walkSpeed",
            "sprintMultiplier",
        };

        static readonly HashSet<string> InteractionFields = new(StringComparer.Ordinal)
        {
            "interactIntervalSeconds",
        };

        static readonly HashSet<string> InventoryFields = new(StringComparer.Ordinal)
        {
            "maxItemSpeciesHoldable",
            "combinationLoadoutId",
        };

        static readonly HashSet<string> SexualHeatFields = new(StringComparer.Ordinal)
        {
            "decreasePerSecond",
        };

        readonly ValidationReport report;

        public PlayerStatValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: playerStats entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
            {
                if (string.Equals(Path.GetFileName(path), "mod.json", StringComparison.OrdinalIgnoreCase))
                    continue;

                ValidateFile(path);
            }
        }

        void ValidateFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateObject(relativePath, obj, "playerStats");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateObject(relativePath, entry, $"playerStats[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: playerStats[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: playerStats file should contain an object or an array of objects.");
        }

        void ValidateObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, PlayerStatRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "displayName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "actionRuleProfileId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "shadowProfileId", report, scope);
            JsonValidationUtil.WarnObject(relativePath, obj, "extras", report, scope);
            ValidateVitals(relativePath, obj["vitals"], $"{scope}.vitals");
            ValidateMovement(relativePath, obj["movement"], $"{scope}.movement");
            ValidateInteraction(relativePath, obj["interaction"], $"{scope}.interaction");
            ValidateInventory(relativePath, obj["inventory"], $"{scope}.inventory");
            ValidateSexualHeat(relativePath, obj["sexualHeat"], $"{scope}.sexualHeat");
        }

        void ValidateVitals(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object.");
                return;
            }

            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, VitalFields, scope, report);
            JsonValidationUtil.WarnInteger(relativePath, obj, "maxHealth", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "healthCap", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "maxStamina", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "staminaCap", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "staminaRestoreAmount", report, scope);
        }

        void ValidateMovement(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object.");
                return;
            }

            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, MovementFields, scope, report);
            JsonValidationUtil.WarnNumber(relativePath, obj, "walkSpeed", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "sprintMultiplier", report, scope);
        }

        void ValidateInteraction(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object.");
                return;
            }

            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, InteractionFields, scope, report);
            JsonValidationUtil.WarnNumber(relativePath, obj, "interactIntervalSeconds", report, scope);
        }

        void ValidateInventory(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object.");
                return;
            }

            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, InventoryFields, scope, report);
            JsonValidationUtil.WarnInteger(relativePath, obj, "maxItemSpeciesHoldable", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "combinationLoadoutId", report, scope);
        }

        void ValidateSexualHeat(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object.");
                return;
            }

            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SexualHeatFields, scope, report);
            JsonValidationUtil.WarnNumber(relativePath, obj, "decreasePerSecond", report, scope);
        }
    }
}
