using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class PatrollerSpawnValidator
    {
        static readonly HashSet<string> SpawnRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "patrollerId",
            "enabled",
            "spawnRegionId",
            "patrolRegionIds",
            "xOffsetFromRegionCenter",
            "yOffsetFromGround",
            "useSpawnRegionCheckpointPosition",
        };

        static readonly HashSet<string> SpawnLoadoutRootFields = new(StringComparer.Ordinal)
        {
            "patrollerSpawnLoadoutId",
            "patrollerSpawnIds",
            "commonPatrolRegionIds",
            "commonPatrolRegionExpansions",
        };

        static readonly HashSet<string> CommonPatrolRegionExpansionFields = new(StringComparer.Ordinal)
        {
            "requiredUnlockedPassageIds",
            "patrolRegionIds",
        };

        readonly ValidationReport report;

        public PatrollerSpawnValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateSpawnsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: patrollerSpawns entry folder has no .json files.");
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
                report.Warn($"{report.ToRelativePath(folderPath)}: patrollerSpawnLoadouts entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateSpawnLoadoutFile(path);
        }

        void ValidateSpawnFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateSpawnObject(relativePath, obj, "patrollerSpawn");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateSpawnObject(relativePath, entry, $"patrollerSpawn[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: patrollerSpawn[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: patroller spawn file should contain an object or an array of objects.");
        }

        void ValidateSpawnObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SpawnRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.RequireString(relativePath, obj, "patrollerId", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "enabled", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "spawnRegionId", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "patrolRegionIds", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "xOffsetFromRegionCenter", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "yOffsetFromGround", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "useSpawnRegionCheckpointPosition", report, scope);
        }

        void ValidateSpawnLoadoutFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateSpawnLoadoutObject(relativePath, obj, "patrollerSpawnLoadout");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateSpawnLoadoutObject(relativePath, entry, $"patrollerSpawnLoadout[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: patrollerSpawnLoadout[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: patroller spawn loadout file should contain an object or an array of objects.");
        }

        void ValidateSpawnLoadoutObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SpawnLoadoutRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "patrollerSpawnLoadoutId", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "patrollerSpawnIds", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "commonPatrolRegionIds", report, scope);
            ValidateCommonPatrolRegionExpansions(relativePath, obj, scope);
        }

        void ValidateCommonPatrolRegionExpansions(string relativePath, JObject obj, string scope)
        {
            JToken token = obj["commonPatrolRegionExpansions"];
            if (token == null)
                return;

            string path = $"{scope}.commonPatrolRegionExpansions";
            if (token is not JArray arr)
            {
                report.Warn($"{relativePath}: {path} should be an array of objects.");
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] is not JObject entry)
                {
                    report.Warn($"{relativePath}: {path}[{i}] should be an object.");
                    continue;
                }

                string entryScope = $"{path}[{i}]";
                JsonValidationUtil.ValidateUnknownFields(relativePath, entry, CommonPatrolRegionExpansionFields, entryScope, report);
                JsonValidationUtil.WarnStringArray(relativePath, entry, "requiredUnlockedPassageIds", report, entryScope);
                JsonValidationUtil.WarnStringArray(relativePath, entry, "patrolRegionIds", report, entryScope);
            }
        }
    }
}
