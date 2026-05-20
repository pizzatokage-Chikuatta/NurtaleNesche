using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class ChainpointValidator
    {
        static readonly HashSet<string> DefinitionFields = new(StringComparer.Ordinal)
        {
            "id",
            "Id",
            "interactionLoadout",
            "InteractionLoadout",
            "addressableKey",
            "AddressableKey",
            "assetBundleFileName",
            "AssetBundleFileName",
            "prefabAssetPathInsideBundle",
            "PrefabAssetPathInsideBundle",
            // Legacy aliases accepted for compatibility.
            "prefabAssetName",
            "PrefabAssetName",
            "statusId",
            "StatusId",
            "typeEnum",
            "TypeEnum",
            "accessScope",
            "AccessScope",
            "isNeedLockPickToEscape",
            "IsNeedLockPickToEscape",
            "runtimeSpawnable",
            "RuntimeSpawnable",
            "stateAnimatorBindings",
            "StateAnimatorBindings",
            "_comment",
            "_howToUse",
            "_example",
        };

        static readonly HashSet<string> StateAnimatorBindingFields = new(StringComparer.Ordinal)
        {
            "chainpointStateId",
            "ChainpointStateId",
            "mainAnimatorId",
            "MainAnimatorId",
            "layerIndex",
            "LayerIndex",
            "entryMode",
            "EntryMode",
            "entryStateName",
            "EntryStateName",
            "entryTriggerName",
            "EntryTriggerName",
            "playOnEnter",
            "PlayOnEnter",
            "monitorStateNames",
            "MonitorStateNames",
        };

        static readonly HashSet<string> SpawnFields = new(StringComparer.Ordinal)
        {
            "id",
            "instanceId",
            "checkpointUuid",
            "enabled",
            "regionId",
            "offsetX",
            "heightFromGround",
            "rotation",
            "rotationY",
            "chainPointId",
            "statusId",
            "typeEnum",
            "isNeedLockPickToEscape",
            "extras",
            "_comment",
            "_howToUse",
            "_example",
        };

        static readonly HashSet<string> RotationFields = new(StringComparer.Ordinal)
        {
            "x",
            "y",
            "z",
        };

        static readonly HashSet<string> SpawnLoadoutFields = new(StringComparer.Ordinal)
        {
            "chainPointSpawnLoadoutId",
            "chainPointSpawnIds",
            "_comment",
            "_howToUse",
            "_example",
        };

        readonly ValidationReport report;

        public ChainpointValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateDefinitionsFolder(string folderPath)
        {
            ValidateFolder(folderPath, "chainPointDefinitions", "chainpointDefinition", ValidateDefinitionObject);
        }

        public void ValidateSpawnsFolder(string folderPath)
        {
            ValidateFolder(folderPath, "chainPointSpawns", "chainpointSpawn", ValidateSpawnObject);
        }

        public void ValidateSpawnLoadoutsFolder(string folderPath)
        {
            ValidateFolder(folderPath, "chainPointSpawnLoadouts", "chainpointSpawnLoadout", ValidateSpawnLoadoutObject);
        }

        void ValidateFolder(string folderPath, string surfaceName, string scopeName, Action<string, JObject, string> validateObject)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: {surfaceName} entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateObjectOrArrayFile(path, surfaceName, scopeName, validateObject);
        }

        void ValidateObjectOrArrayFile(string path, string surfaceName, string scopeName, Action<string, JObject, string> validateObject)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                validateObject(relativePath, obj, scopeName);
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        validateObject(relativePath, entry, $"{scopeName}[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: {scopeName}[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: {surfaceName} file should contain a JSON object or array.");
        }

        void ValidateDefinitionObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, DefinitionFields, scope, report);
            RequireAnyString(relativePath, obj, scope, "id", "Id");
            WarnAnyString(relativePath, obj, scope, "interactionLoadout", "InteractionLoadout");
            WarnAnyString(relativePath, obj, scope, "addressableKey", "AddressableKey");
            WarnAnyString(relativePath, obj, scope, "assetBundleFileName", "AssetBundleFileName");
            WarnAnyString(relativePath, obj, scope, "prefabAssetPathInsideBundle", "PrefabAssetPathInsideBundle");
            WarnAnyString(relativePath, obj, scope, "prefabAssetName", "PrefabAssetName");
            WarnBundlePair(relativePath, obj, scope);
            WarnAnyString(relativePath, obj, scope, "statusId", "StatusId");
            WarnAnyString(relativePath, obj, scope, "typeEnum", "TypeEnum");
            WarnAnyString(relativePath, obj, scope, "accessScope", "AccessScope");
            WarnAnyBoolean(relativePath, obj, scope, "isNeedLockPickToEscape", "IsNeedLockPickToEscape");
            WarnAnyBoolean(relativePath, obj, scope, "runtimeSpawnable", "RuntimeSpawnable");
            ValidateStateAnimatorBindings(relativePath, GetFirst(obj, "stateAnimatorBindings", "StateAnimatorBindings"), $"{scope}.stateAnimatorBindings");
        }

        void ValidateStateAnimatorBindings(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JArray arr)
            {
                report.Warn($"{relativePath}: {scope} should be an array of objects.");
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] is not JObject binding)
                {
                    report.Warn($"{relativePath}: {scope}[{i}] should be an object.");
                    continue;
                }

                string bindingScope = $"{scope}[{i}]";
                JsonValidationUtil.ValidateUnknownFields(relativePath, binding, StateAnimatorBindingFields, bindingScope, report);
                WarnAnyString(relativePath, binding, bindingScope, "chainpointStateId", "ChainpointStateId");
                WarnAnyString(relativePath, binding, bindingScope, "mainAnimatorId", "MainAnimatorId");
                WarnAnyInteger(relativePath, binding, bindingScope, "layerIndex", "LayerIndex");
                WarnAnyString(relativePath, binding, bindingScope, "entryMode", "EntryMode");
                WarnAnyString(relativePath, binding, bindingScope, "entryStateName", "EntryStateName");
                WarnAnyString(relativePath, binding, bindingScope, "entryTriggerName", "EntryTriggerName");
                WarnAnyBoolean(relativePath, binding, bindingScope, "playOnEnter", "PlayOnEnter");
                WarnAnyStringArray(relativePath, binding, bindingScope, "monitorStateNames", "MonitorStateNames");
            }
        }

        void ValidateSpawnObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SpawnFields, scope, report);
            RequireAnyString(relativePath, obj, scope, "id", "instanceId");
            JsonValidationUtil.WarnString(relativePath, obj, "checkpointUuid", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "enabled", report, scope);
            JsonValidationUtil.RequireString(relativePath, obj, "regionId", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "offsetX", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "heightFromGround", report, scope);
            ValidateRotation(relativePath, obj["rotation"], $"{scope}.rotation");
            JsonValidationUtil.WarnNumber(relativePath, obj, "rotationY", report, scope);
            JsonValidationUtil.RequireString(relativePath, obj, "chainPointId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "statusId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "typeEnum", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "isNeedLockPickToEscape", report, scope);
            JsonValidationUtil.WarnObject(relativePath, obj, "extras", report, scope);
        }

        void ValidateRotation(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object with numeric x, y, and z fields.");
                return;
            }

            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, RotationFields, scope, report);
            JsonValidationUtil.WarnNumber(relativePath, obj, "x", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "y", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "z", report, scope);
        }

        void ValidateSpawnLoadoutObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SpawnLoadoutFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "chainPointSpawnLoadoutId", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "chainPointSpawnIds", report, scope);
        }

        void RequireAnyString(string relativePath, JObject obj, string scope, params string[] fields)
        {
            if (TryFindPresentField(obj, out JProperty property, fields))
            {
                if (property.Value.Type != JTokenType.String)
                {
                    report.Warn($"{relativePath}: {scope}.{property.Name} should be a string.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(property.Value.Value<string>()))
                    report.Error($"{relativePath}: {scope}.{property.Name} cannot be empty.");

                return;
            }

            report.Error($"{relativePath}: {scope}.{fields[0]} is required.");
        }

        void WarnAnyString(string relativePath, JObject obj, string scope, params string[] fields)
        {
            if (TryFindPresentField(obj, out JProperty property, fields) && property.Value.Type != JTokenType.String)
                report.Warn($"{relativePath}: {scope}.{property.Name} should be a string.");
        }

        void WarnAnyInteger(string relativePath, JObject obj, string scope, params string[] fields)
        {
            if (TryFindPresentField(obj, out JProperty property, fields) && property.Value.Type != JTokenType.Integer)
                report.Warn($"{relativePath}: {scope}.{property.Name} should be an integer.");
        }

        void WarnAnyBoolean(string relativePath, JObject obj, string scope, params string[] fields)
        {
            if (TryFindPresentField(obj, out JProperty property, fields) && property.Value.Type != JTokenType.Boolean)
                report.Warn($"{relativePath}: {scope}.{property.Name} should be a boolean.");
        }

        void WarnAnyStringArray(string relativePath, JObject obj, string scope, params string[] fields)
        {
            if (!TryFindPresentField(obj, out JProperty property, fields))
                return;

            string path = $"{scope}.{property.Name}";
            if (property.Value is not JArray arr)
            {
                report.Warn($"{relativePath}: {path} should be an array of strings.");
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].Type != JTokenType.String)
                    report.Warn($"{relativePath}: {path}[{i}] should be a string.");
            }
        }

        void WarnBundlePair(string relativePath, JObject obj, string scope)
        {
            bool hasBundleFile = HasAnyNonEmptyString(obj, "assetBundleFileName", "AssetBundleFileName");
            bool hasPrefabAsset = HasAnyNonEmptyString(obj, "prefabAssetPathInsideBundle", "PrefabAssetPathInsideBundle", "prefabAssetName", "PrefabAssetName");
            if (hasBundleFile != hasPrefabAsset)
                report.Warn($"{relativePath}: {scope} should define both assetBundleFileName and prefabAssetPathInsideBundle for AssetBundle chainpoint prefabs.");
        }

        static JToken GetFirst(JObject obj, params string[] fields)
        {
            return TryFindPresentField(obj, out JProperty property, fields) ? property.Value : null;
        }

        static bool HasAnyNonEmptyString(JObject obj, params string[] fields)
        {
            foreach (string field in fields)
            {
                JToken token = obj?[field];
                if (token != null &&
                    token.Type == JTokenType.String &&
                    !string.IsNullOrWhiteSpace(token.Value<string>()))
                {
                    return true;
                }
            }

            return false;
        }

        static bool TryFindPresentField(JObject obj, out JProperty property, params string[] fields)
        {
            foreach (string field in fields)
            {
                property = obj.Property(field);
                if (property != null)
                    return true;
            }

            property = null;
            return false;
        }
    }
}
