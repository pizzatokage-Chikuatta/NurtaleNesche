using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class ItemDefinitionValidator
    {
        static readonly HashSet<string> ItemDefinitionRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "NameInAlphabet",
            "showUpStage",
            "brokenChance",
            "itemSelectionRequestId",
            "builtInPrefabAddress",
            "prefabSourceItemId",
            "assetBundleFileName",
            "prefabAssetPathInsideBundle",
            // Legacy alias accepted for compatibility.
            "prefabAssetName",
            "assemblyFileName",
            "logicTypeName",
            "spriteIds",
            "extras",
        };

        static readonly HashSet<string> EquipmentDefinitionRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "NameInAlphabet",
            "showUpStage",
            "builtInPrefabAddress",
            "prefabSourceEquipmentId",
            "assetBundleFileName",
            "prefabAssetPathInsideBundle",
            // Legacy alias accepted for compatibility.
            "prefabAssetName",
            "assemblyFileName",
            "logicTypeName",
            "spriteIds",
            "extras",
        };

        static readonly HashSet<string> RuntimeSpriteSlotFields = new(StringComparer.Ordinal)
        {
            "key",
            "spriteId",
        };

        readonly ValidationReport report;

        public ItemDefinitionValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateItemDefinitionsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath, preferredFileName: "item.json");

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: itemDefinitions entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateItemDefinitionFile(path);
        }

        public void ValidateEquipmentDefinitionsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath, preferredFileName: "equipment.json");

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: equipmentDefinitions entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateEquipmentDefinitionFile(path);
        }

        void ValidateItemDefinitionFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateItemDefinitionObject(relativePath, obj, "itemDefinition");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateItemDefinitionObject(relativePath, entry, $"itemDefinition[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: itemDefinition[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: item definition file should contain an object or an array of objects.");
        }

        void ValidateItemDefinitionObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ItemDefinitionRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "NameInAlphabet", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "showUpStage", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "brokenChance", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "itemSelectionRequestId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "builtInPrefabAddress", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "prefabSourceItemId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "assetBundleFileName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "prefabAssetPathInsideBundle", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "prefabAssetName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "assemblyFileName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "logicTypeName", report, scope);
            JsonValidationUtil.WarnObject(relativePath, obj, "extras", report, scope);
            ValidateRuntimeSpriteSlots(relativePath, obj["spriteIds"], $"{scope}.spriteIds");
        }

        void ValidateEquipmentDefinitionFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateEquipmentDefinitionObject(relativePath, obj, "equipmentDefinition");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateEquipmentDefinitionObject(relativePath, entry, $"equipmentDefinition[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: equipmentDefinition[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: equipment definition file should contain an object or an array of objects.");
        }

        void ValidateEquipmentDefinitionObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, EquipmentDefinitionRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "NameInAlphabet", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "showUpStage", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "builtInPrefabAddress", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "prefabSourceEquipmentId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "assetBundleFileName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "prefabAssetPathInsideBundle", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "prefabAssetName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "assemblyFileName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "logicTypeName", report, scope);
            JsonValidationUtil.WarnObject(relativePath, obj, "extras", report, scope);
            ValidateRuntimeSpriteSlots(relativePath, obj["spriteIds"], $"{scope}.spriteIds");
        }

        void ValidateRuntimeSpriteSlots(string relativePath, JToken slotsToken, string scope)
        {
            if (slotsToken == null)
                return;

            if (slotsToken is not JArray slots)
            {
                report.Warn($"{relativePath}: {scope} should be an array.");
                return;
            }

            for (int i = 0; i < slots.Count; i++)
            {
                string entryScope = $"{scope}[{i}]";
                if (slots[i] is not JObject slot)
                {
                    report.Warn($"{relativePath}: {entryScope} should be an object.");
                    continue;
                }

                JsonValidationUtil.ValidateUnknownFields(relativePath, slot, RuntimeSpriteSlotFields, entryScope, report);
                JsonValidationUtil.RequireString(relativePath, slot, "key", report, entryScope);
                JsonValidationUtil.RequireString(relativePath, slot, "spriteId", report, entryScope);
            }
        }
    }
}
