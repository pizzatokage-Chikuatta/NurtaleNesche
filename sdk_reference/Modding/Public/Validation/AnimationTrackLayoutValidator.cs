using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class AnimationTrackLayoutValidator
    {
        static readonly HashSet<string> LayoutRootFields = new(StringComparer.Ordinal)
        {
            "animationSetId",
            "actorId",
            "tracks",
        };

        static readonly HashSet<string> TrackFields = new(StringComparer.Ordinal)
        {
            "trackId",
            "localPosition",
            "localEulerAngles",
            "localScale",
            "sortingLayerName",
            "sortingOrder",
            "flipX",
            "flipY",
            "drawMode",
            "size",
            "maskInteraction",
            "spriteSortPoint",
        };

        readonly ValidationReport report;

        public AnimationTrackLayoutValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateFolder(string folderPath)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: animationTrackLayouts entry folder has no .json files.");
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
                ValidateObject(relativePath, obj, "animationTrackLayout");
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject element)
                    {
                        ValidateObject(relativePath, element, $"animationTrackLayout[{i}]");
                        continue;
                    }

                    report.Error($"{relativePath}: animationTrackLayout[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: animationTrackLayouts file should contain a JSON object or array.");
        }

        void ValidateObject(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, LayoutRootFields, scope, report);
            ValidateActorId(relativePath, obj, scope);
            ValidateTracks(relativePath, obj["tracks"], $"{scope}.tracks");
        }

        void ValidateActorId(string relativePath, JObject obj, string scope)
        {
            JToken animationSetId = obj["animationSetId"];
            JToken actorId = obj["actorId"];
            bool hasAnimationSetId = IsNonEmptyString(animationSetId);
            bool hasActorId = IsNonEmptyString(actorId);

            if (animationSetId != null && animationSetId.Type != JTokenType.String)
                report.Warn($"{relativePath}: {scope}.animationSetId should be a string.");

            if (actorId != null && actorId.Type != JTokenType.String)
                report.Warn($"{relativePath}: {scope}.actorId should be a string.");

            if (!hasAnimationSetId && !hasActorId)
                report.Error($"{relativePath}: {scope}.animationSetId or {scope}.actorId is required.");
        }

        void ValidateTracks(string relativePath, JToken token, string scope)
        {
            if (token is not JArray arr)
            {
                report.Error($"{relativePath}: {scope} should be a non-empty array.");
                return;
            }

            if (arr.Count == 0)
            {
                report.Error($"{relativePath}: {scope} should be a non-empty array.");
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] is not JObject track)
                {
                    report.Warn($"{relativePath}: {scope}[{i}] should be an object.");
                    continue;
                }

                ValidateTrack(relativePath, track, $"{scope}[{i}]");
            }
        }

        void ValidateTrack(string relativePath, JObject obj, string scope)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, TrackFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "trackId", report, scope);

            ValidateVec3(relativePath, obj["localPosition"], $"{scope}.localPosition");
            ValidateVec3(relativePath, obj["localEulerAngles"], $"{scope}.localEulerAngles");
            ValidateVec3(relativePath, obj["localScale"], $"{scope}.localScale");
            ValidateVec2(relativePath, obj["size"], $"{scope}.size");

            JsonValidationUtil.WarnString(relativePath, obj, "sortingLayerName", report, scope);
            JsonValidationUtil.WarnInteger(relativePath, obj, "sortingOrder", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "flipX", report, scope);
            JsonValidationUtil.WarnBoolean(relativePath, obj, "flipY", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "drawMode", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "maskInteraction", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "spriteSortPoint", report, scope);
        }

        void ValidateVec2(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object with numeric x and y fields.");
                return;
            }

            if (!IsNumber(obj["x"]) || !IsNumber(obj["y"]))
                report.Warn($"{relativePath}: {scope} should define numeric x and y fields.");
        }

        void ValidateVec3(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token is not JObject obj)
            {
                report.Warn($"{relativePath}: {scope} should be an object with numeric x, y, and z fields.");
                return;
            }

            if (!IsNumber(obj["x"]) || !IsNumber(obj["y"]) || !IsNumber(obj["z"]))
                report.Warn($"{relativePath}: {scope} should define numeric x, y, and z fields.");
        }

        static bool IsNonEmptyString(JToken token)
        {
            return token != null
                && token.Type == JTokenType.String
                && !string.IsNullOrWhiteSpace(token.Value<string>());
        }

        static bool IsNumber(JToken token)
        {
            return token != null && (token.Type == JTokenType.Integer || token.Type == JTokenType.Float);
        }
    }
}
