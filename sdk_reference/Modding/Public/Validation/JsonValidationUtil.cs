using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NurtaleNesche.Modding.Validation
{
    internal static class JsonValidationUtil
    {
        public static JObject TryReadObject(string path, ValidationReport report)
        {
            try
            {
                string json = SanitizeJson(File.ReadAllText(path));
                return JObject.Parse(json);
            }
            catch (Exception e)
            {
                report.Error($"{report.ToRelativePath(path)}: JSON parse failed: {e.Message}");
                return null;
            }
        }

        public static JToken TryReadToken(string path, ValidationReport report)
        {
            try
            {
                string json = SanitizeJson(File.ReadAllText(path));
                return JToken.Parse(json);
            }
            catch (Exception e)
            {
                report.Error($"{report.ToRelativePath(path)}: JSON parse failed: {e.Message}");
                return null;
            }
        }

        public static string[] EnumerateJsonFiles(string folderPath, string preferredFileName = null)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                return Array.Empty<string>();

            if (!string.IsNullOrWhiteSpace(preferredFileName))
            {
                string[] preferred = Directory.GetFiles(folderPath, preferredFileName, SearchOption.AllDirectories)
                    .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                if (preferred.Length > 0)
                    return preferred;
            }

            return Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories)
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        static string SanitizeJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return json;

            return Regex.Replace(json, @",(\s*[}\]])", "$1");
        }

        public static void ValidateUnknownFields(
            string relativePath,
            JObject obj,
            HashSet<string> allowedFields,
            string scope,
            ValidationReport report)
        {
            foreach (JProperty property in obj.Properties())
            {
                if (allowedFields.Contains(property.Name))
                    continue;

                report.Warn($"{relativePath}: unknown {scope} field '{property.Name}'.");
            }
        }

        public static void RequireString(
            string relativePath,
            JObject obj,
            string field,
            ValidationReport report,
            string scope = null)
        {
            JToken token = obj[field];
            string path = string.IsNullOrWhiteSpace(scope) ? field : $"{scope}.{field}";

            if (token == null)
            {
                report.Error($"{relativePath}: {path} is required.");
                return;
            }

            if (token.Type != JTokenType.String)
            {
                report.Warn($"{relativePath}: {path} should be a string.");
                return;
            }

            if (string.IsNullOrWhiteSpace(token.Value<string>()))
                report.Error($"{relativePath}: {path} cannot be empty.");
        }

        public static void WarnString(
            string relativePath,
            JObject obj,
            string field,
            ValidationReport report,
            string scope)
        {
            JToken token = obj[field];
            if (token != null && token.Type != JTokenType.String)
                report.Warn($"{relativePath}: {scope}.{field} should be a string.");
        }

        public static void WarnNumber(
            string relativePath,
            JObject obj,
            string field,
            ValidationReport report,
            string scope)
        {
            JToken token = obj[field];
            if (token != null && token.Type != JTokenType.Integer && token.Type != JTokenType.Float)
                report.Warn($"{relativePath}: {scope}.{field} should be a number.");
        }

        public static void WarnInteger(
            string relativePath,
            JObject obj,
            string field,
            ValidationReport report,
            string scope)
        {
            JToken token = obj[field];
            if (token != null && token.Type != JTokenType.Integer)
                report.Warn($"{relativePath}: {scope}.{field} should be an integer.");
        }

        public static void WarnBoolean(
            string relativePath,
            JObject obj,
            string field,
            ValidationReport report,
            string scope)
        {
            JToken token = obj[field];
            if (token != null && token.Type != JTokenType.Boolean)
                report.Warn($"{relativePath}: {scope}.{field} should be a boolean.");
        }

        public static void WarnObject(
            string relativePath,
            JObject obj,
            string field,
            ValidationReport report,
            string scope)
        {
            JToken token = obj[field];
            if (token != null && token is not JObject)
                report.Warn($"{relativePath}: {scope}.{field} should be an object.");
        }

        public static void WarnStringArray(
            string relativePath,
            JObject obj,
            string field,
            ValidationReport report,
            string scope)
        {
            JToken token = obj[field];
            if (token == null)
                return;

            string path = $"{scope}.{field}";
            if (token is not JArray arr)
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

        public static void WarnIntegerArray(
            string relativePath,
            JObject obj,
            string field,
            ValidationReport report,
            string scope)
        {
            JToken token = obj[field];
            if (token == null)
                return;

            string path = $"{scope}.{field}";
            if (token is not JArray arr)
            {
                report.Warn($"{relativePath}: {path} should be an array of integers.");
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].Type != JTokenType.Integer)
                    report.Warn($"{relativePath}: {path}[{i}] should be an integer.");
            }
        }
    }
}
