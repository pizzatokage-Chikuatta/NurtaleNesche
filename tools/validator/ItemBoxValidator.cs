using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class ItemBoxValidator
    {
        static readonly HashSet<string> PrefabRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "builtInPrefabAddress",
            "assetBundleFileName",
            "prefabAssetName",
        };

        static readonly HashSet<string> SpawnRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "enabled",
            "boxPrefabId",
            "spawnRegionId",
            "xOffsetFromRegionCenter",
            "useSpawnRegionCheckpointPosition",
            "yPosOffset",
            "dropYVelocity",
            "dontRespawn",
            "hasRandomSpawn",
            "dontCurseItem",
            "dontBrokeItem",
            "curseAll",
            "dontDropStageCommonDrops",
            "isLockedAtStart",
            "isNeedHammer",
            "openSFXId",
            "uniqueDropTableIds",
            "fixedRandomSpawnItemIds",
            "fixedRandomSpawnQuantityList",
            "spawnEquipmentIds",
        };

        static readonly HashSet<string> SpawnLoadoutRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "itemBoxSpawnIds",
        };

        readonly ValidationReport report;

        public ItemBoxValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidatePrefabsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: itemBoxPrefabs entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidatePrefabFile(path);
        }

        public void ValidateSpawnsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: itemBoxSpawns entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateSpawnFile(path);
        }

        public void ValidateSpawnLoadoutsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: itemBoxSpawnLoadouts entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateSpawnLoadoutFile(path);
        }

        void ValidatePrefabFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidatePrefabObject(relativePath, obj, "itemBoxPrefab");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidatePrefabObject(relativePath, entry, $"itemBoxPrefab[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: itemBoxPrefab[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: item box prefab file should contain an object or an array of objects.");
        }

        void ValidatePrefabObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, PrefabRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "builtInPrefabAddress", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "assetBundleFileName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "prefabAssetName", report, scope);
        }

        void ValidateSpawnFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateSpawnObject(relativePath, obj, "itemBoxSpawn");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateSpawnObject(relativePath, entry, $"itemBoxSpawn[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: itemBoxSpawn[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: item box spawn file should contain an object or an array of objects.");
        }

        void ValidateSpawnObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SpawnRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "enabled", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "boxPrefabId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "spawnRegionId", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "xOffsetFromRegionCenter", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "useSpawnRegionCheckpointPosition", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "yPosOffset", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "dropYVelocity", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "dontRespawn", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "hasRandomSpawn", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "dontCurseItem", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "dontBrokeItem", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "curseAll", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "dontDropStageCommonDrops", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "isLockedAtStart", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "isNeedHammer", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "openSFXId", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "uniqueDropTableIds", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "fixedRandomSpawnItemIds", report, scope);
            JsonValidationUtil.WarnIntegerArray(relativePath, obj, "fixedRandomSpawnQuantityList", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "spawnEquipmentIds", report, scope);
        }

        void ValidateSpawnLoadoutFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateSpawnLoadoutObject(relativePath, obj, "itemBoxSpawnLoadout");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateSpawnLoadoutObject(relativePath, entry, $"itemBoxSpawnLoadout[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: itemBoxSpawnLoadout[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: item box spawn loadout file should contain an object or an array of objects.");
        }

        void ValidateSpawnLoadoutObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SpawnLoadoutRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "itemBoxSpawnIds", report, scope);
        }
    }
}
