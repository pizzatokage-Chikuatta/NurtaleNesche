using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class LootingValidator
    {
        static readonly HashSet<string> DropTableRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "guaranteedItem",
            "guaranteedEquipment",
            "groups",
        };

        static readonly HashSet<string> DropTableGroupFields = new(StringComparer.Ordinal)
        {
            "rolls",
            "entries",
        };

        static readonly HashSet<string> DropTableEntryFields = new(StringComparer.Ordinal)
        {
            "itemId",
            "weight",
            "hasBrokenChance",
        };

        static readonly HashSet<string> DropTableLoadoutRootFields = new(StringComparer.Ordinal)
        {
            "dropTableLoadoutId",
            "dropTableIds",
        };

        static readonly HashSet<string> ItemCombinationRootFields = new(StringComparer.Ordinal)
        {
            "combinationId",
            "inputs",
            "output",
        };

        static readonly HashSet<string> ItemCombinationLoadoutRootFields = new(StringComparer.Ordinal)
        {
            "combinationLoadoutId",
            "combinationIds",
        };

        readonly ValidationReport report;

        public LootingValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateDropTablesFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath, preferredFileName: "drop_table.json");

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: dropTables entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateDropTableFile(path);
        }

        public void ValidateDropTableLoadoutsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath, preferredFileName: "drop_table_loadout.json");

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: dropTableLoadouts entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateDropTableLoadoutFile(path);
        }

        public void ValidateItemCombinationsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath, preferredFileName: "item_combination.json");

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: itemCombinations entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateItemCombinationFile(path);
        }

        public void ValidateItemCombinationLoadoutsFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath, preferredFileName: "item_combination_loadout.json");

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: itemCombinationLoadouts entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateItemCombinationLoadoutFile(path);
        }

        void ValidateDropTableFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateDropTableObject(relativePath, obj, "dropTable");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateDropTableObject(relativePath, entry, $"dropTable[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: dropTable[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: drop table file should contain an object or an array of objects.");
        }

        void ValidateDropTableObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, DropTableRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "guaranteedItem", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "guaranteedEquipment", report, scope);
            ValidateDropTableGroups(relativePath, obj["groups"], $"{scope}.groups");
        }

        void ValidateDropTableGroups(string relativePath, JToken groupsToken, string scope)
        {
            if (groupsToken == null)
                return;

            if (groupsToken is not JArray groups)
            {
                report.Warn($"{relativePath}: {scope} should be an array.");
                return;
            }

            for (int i = 0; i < groups.Count; i++)
            {
                string entryScope = $"{scope}[{i}]";
                if (groups[i] is not JObject group)
                {
                    report.Warn($"{relativePath}: {entryScope} should be an object.");
                    continue;
                }

                JsonValidationUtil.ValidateUnknownFields(relativePath, group, DropTableGroupFields, entryScope, report);
                JsonValidationUtil.WarnInteger(relativePath, group, "rolls", report, entryScope);
                ValidateDropTableEntries(relativePath, group["entries"], $"{entryScope}.entries");
            }
        }

        void ValidateDropTableEntries(string relativePath, JToken entriesToken, string scope)
        {
            if (entriesToken == null)
                return;

            if (entriesToken is not JArray entries)
            {
                report.Warn($"{relativePath}: {scope} should be an array.");
                return;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                string entryScope = $"{scope}[{i}]";
                if (entries[i] is not JObject entry)
                {
                    report.Warn($"{relativePath}: {entryScope} should be an object.");
                    continue;
                }

                JsonValidationUtil.ValidateUnknownFields(relativePath, entry, DropTableEntryFields, entryScope, report);

                JToken itemIdToken = entry["itemId"];
                if (itemIdToken != null && itemIdToken.Type != JTokenType.String && itemIdToken.Type != JTokenType.Null)
                    report.Warn($"{relativePath}: {entryScope}.itemId should be a string or null.");

                JsonValidationUtil.WarnInteger(relativePath, entry, "weight", report, entryScope);
                JsonValidationUtil.WarnBoolean(relativePath, entry, "hasBrokenChance", report, entryScope);
            }
        }

        void ValidateDropTableLoadoutFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateDropTableLoadoutObject(relativePath, obj, "dropTableLoadout");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateDropTableLoadoutObject(relativePath, entry, $"dropTableLoadout[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: dropTableLoadout[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: drop table loadout file should contain an object or an array of objects.");
        }

        void ValidateDropTableLoadoutObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, DropTableLoadoutRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "dropTableLoadoutId", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "dropTableIds", report, scope);
        }

        void ValidateItemCombinationFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateItemCombinationObject(relativePath, obj, "itemCombination");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateItemCombinationObject(relativePath, entry, $"itemCombination[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: itemCombination[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: item combination file should contain an object or an array of objects.");
        }

        void ValidateItemCombinationObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ItemCombinationRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "combinationId", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "inputs", report, scope);
            JsonValidationUtil.RequireString(relativePath, obj, "output", report, scope);
        }

        void ValidateItemCombinationLoadoutFile(string path)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                ValidateItemCombinationLoadoutObject(relativePath, obj, "itemCombinationLoadout");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateItemCombinationLoadoutObject(relativePath, entry, $"itemCombinationLoadout[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: itemCombinationLoadout[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: item combination loadout file should contain an object or an array of objects.");
        }

        void ValidateItemCombinationLoadoutObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ItemCombinationLoadoutRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "combinationLoadoutId", report, scope);
            JsonValidationUtil.WarnStringArray(relativePath, obj, "combinationIds", report, scope);
        }
    }
}
