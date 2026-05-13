using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class ShadowProfileValidator
    {
        static readonly HashSet<string> ShadowProfileRootFields = new(StringComparer.Ordinal)
        {
            "id",
            "default",
            "states",
        };

        static readonly HashSet<string> ShadowPropertyFields = new(StringComparer.Ordinal)
        {
            "scale",
            "offset",
            "alpha",
            "smooth",
            "smoothDampTime",
        };

        static readonly HashSet<string> ShadowStateFields = new(StringComparer.Ordinal)
        {
            "scale",
            "offset",
            "alpha",
            "smooth",
            "smoothDampTime",
            "isBlendTree",
            "fallback",
            "clips",
        };

        readonly ValidationReport report;

        public ShadowProfileValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: shadowProfiles entry folder has no .json files.");
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
            if (token is not JObject obj)
            {
                report.Error($"{relativePath}: shadowProfiles file should contain a JSON object.");
                return;
            }

            ValidateObject(relativePath, obj, "shadowProfile");
        }

        void ValidateObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ShadowProfileRootFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            ValidateProperty(relativePath, obj["default"], $"{scope}.default");
            ValidateStates(relativePath, obj["states"], $"{scope}.states");
        }

        void ValidateStates(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject states)
            {
                report.Warn($"{relativePath}: {scope} should be an object mapping state names to shadow properties.");
                return;
            }

            foreach (JProperty stateProperty in states.Properties())
            {
                string stateScope = $"{scope}.{stateProperty.Name}";
                if (stateProperty.Value is not JObject stateObj)
                {
                    report.Warn($"{relativePath}: {stateScope} should be an object.");
                    continue;
                }

                ValidateState(relativePath, stateObj, stateScope);
            }
        }

        void ValidateState(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ShadowStateFields, scope, report);
            ValidatePropertyFields(relativePath, obj, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "isBlendTree", report, scope);
            ValidateProperty(relativePath, obj["fallback"], $"{scope}.fallback");
            ValidateClips(relativePath, obj["clips"], $"{scope}.clips");
        }

        void ValidateClips(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject clips)
            {
                report.Warn($"{relativePath}: {scope} should be an object mapping clip names to shadow properties.");
                return;
            }

            foreach (JProperty clipProperty in clips.Properties())
                ValidateProperty(relativePath, clipProperty.Value, $"{scope}.{clipProperty.Name}");
        }

        void ValidateProperty(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object.");
                return;
            }

            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, ShadowPropertyFields, scope, report);
            ValidatePropertyFields(relativePath, obj, scope);
        }

        void ValidatePropertyFields(string relativePath, JObject obj, string scope)
        {
            ValidateVec2(relativePath, obj["scale"], $"{scope}.scale");
            ValidateVec2(relativePath, obj["offset"], $"{scope}.offset");
            JsonValidationUtil.WarnNumber(relativePath, obj, "alpha", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "smooth", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "smoothDampTime", report, scope);
        }

        void ValidateVec2(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is JArray arr)
            {
                if (arr.Count < 2)
                {
                    report.Warn($"{relativePath}: {scope} should have at least two numeric entries.");
                    return;
                }

                if (!IsNumber(arr[0]) || !IsNumber(arr[1]))
                    report.Warn($"{relativePath}: {scope} should contain numeric x/y values.");

                return;
            }

            if (token is JObject obj)
            {
                JToken x = obj["x"];
                JToken y = obj["y"];
                if (!IsNumber(x) || !IsNumber(y))
                    report.Warn($"{relativePath}: {scope} should define numeric x and y fields.");

                return;
            }

            report.Warn($"{relativePath}: {scope} should be an array [x, y] or object {{ x, y }}.");
        }

        static bool IsNumber(JToken token)
        {
            return token != null && (token.Type == JTokenType.Integer || token.Type == JTokenType.Float);
        }
    }
}
