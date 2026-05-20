using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class ItemNameValidator
    {
        static readonly HashSet<string> ItemNameRootFields = new(StringComparer.Ordinal)
        {
            "itemId",
            "unknownNames",
            "knownNames",
        };

        readonly ValidationReport report;

        public ItemNameValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: itemNames entry folder has no .json files.");
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
                ValidateObject(relativePath, obj, "itemName");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        ValidateObject(relativePath, entry, $"itemName[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: itemName[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: itemNames file should contain an object or an array of objects.");
        }

        void ValidateObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ItemNameRootFields, scope, report);

            JsonValidationUtil.RequireString(relativePath, obj, "itemId", report, scope);
            ValidateNameMap(relativePath, obj["unknownNames"], $"{scope}.unknownNames");
            ValidateNameMap(relativePath, obj["knownNames"], $"{scope}.knownNames");
        }

        void ValidateNameMap(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object mapping language codes to names.");
                return;
            }

            foreach (JProperty property in obj.Properties())
            {
                if (property.Value.Type != JTokenType.String)
                    report.Warn($"{relativePath}: {scope}.{property.Name} should be a string.");
            }
        }
    }
}
