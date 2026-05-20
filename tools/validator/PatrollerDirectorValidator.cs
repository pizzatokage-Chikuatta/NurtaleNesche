using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class PatrollerDirectorValidator
    {
        static readonly HashSet<string> DirectorRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "patrolRegionIds",
            "startActive",
            "autoActivation",
            "autoActivationDelay",
            "maxAliveFromThisSpawner",
            "maxAliveOnFloor",
            "fastReduceCoolEnemyCount",
            "spawnCooldown",
            "spawnJitterX",
            "allowDuplicatePatrollersInSameWave",
            "spawnCountChoices",
            "patrollers",
        };

        static readonly HashSet<string> WeightedPatrollerFields = new(StringComparer.Ordinal)
        {
            "patrollerId",
            "weight",
        };

        static readonly HashSet<string> StageLoadoutRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "assignments",
        };

        static readonly HashSet<string> AssignmentFields = new(StringComparer.Ordinal)
        {
            "spawnPointId",
            "directorId",
        };

        readonly ValidationReport report;

        public PatrollerDirectorValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateDirectorsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: patrollerDirectors entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateDirectorFile(path);
        }

        public void ValidateStageLoadoutsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: patrollerDirectorStageLoadouts entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateStageLoadoutFile(path);
        }

        void ValidateDirectorFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateDirectorObject(relativePath, obj, "patrollerDirector");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateDirectorObject(relativePath, entry, $"patrollerDirector[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: patrollerDirector[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: patroller director file should contain an object or an array of objects.");
        }

        void ValidateDirectorObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, DirectorRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "patrolRegionIds", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "startActive", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "autoActivation", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "autoActivationDelay", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "maxAliveFromThisSpawner", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "maxAliveOnFloor", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "fastReduceCoolEnemyCount", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "spawnCooldown", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "spawnJitterX", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "allowDuplicatePatrollersInSameWave", report, scope);
            JsonValidationUtil.WarnIntegerArray(relativePath, obj, "spawnCountChoices", report, scope);
            ValidateWeightedPatrollers(relativePath, obj, scope);
        }

        void ValidateWeightedPatrollers(string relativePath, JObject obj, string scope)
        {
            JToken token = obj["patrollers"];
            if (token == null)
                return;

            string path = $"{scope}.patrollers";
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
                JsonValidationUtil.ValidateUnknownFields(relativePath, entry, WeightedPatrollerFields, entryScope, report);
                JsonValidationUtil.RequireString(relativePath, entry, "patrollerId", report, entryScope);
                JsonValidationUtil.WarnInteger(relativePath, entry, "weight", report, entryScope);
            }
        }

        void ValidateStageLoadoutFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateStageLoadoutObject(relativePath, obj, "patrollerDirectorStageLoadout");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateStageLoadoutObject(relativePath, entry, $"patrollerDirectorStageLoadout[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: patrollerDirectorStageLoadout[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: patroller director stage loadout file should contain an object or an array of objects.");
        }

        void ValidateStageLoadoutObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, StageLoadoutRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            WarnStageSuffix(relativePath, obj, scope);
            ValidateAssignments(relativePath, obj, scope);
        }

        void WarnStageSuffix(string relativePath, JObject obj, string scope)
        {
            JToken token = obj["id"];
            if (token?.Type != JTokenType.String)
                return;

            string id = token.Value<string>()?.Trim();
            int dotIndex = id?.LastIndexOf('.') ?? -1;
            string suffix = dotIndex >= 0 && dotIndex < id.Length - 1
                ? id[(dotIndex + 1)..]
                : null;

            if (string.IsNullOrWhiteSpace(suffix) || !suffix.StartsWith("stage_", StringComparison.Ordinal))
                report.Warn($"{relativePath}: {scope}.id should end with a stage id suffix like 'stage_03'.");
        }

        void ValidateAssignments(string relativePath, JObject obj, string scope)
        {
            JToken token = obj["assignments"];
            if (token == null)
                return;

            string path = $"{scope}.assignments";
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
                JsonValidationUtil.ValidateUnknownFields(relativePath, entry, AssignmentFields, entryScope, report);
                JsonValidationUtil.RequireString(relativePath, entry, "spawnPointId", report, entryScope);
                JsonValidationUtil.RequireString(relativePath, entry, "directorId", report, entryScope);
            }
        }
    }
}
