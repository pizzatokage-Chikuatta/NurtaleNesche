using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class AssetReferenceValidator
    {
        static readonly HashSet<string> SpriteFields = new(StringComparer.Ordinal)
        {
            "id",
            "builtInSpriteAddress",
            "spriteSourceId",
            "assetBundleFileName",
            "spriteAssetName",
            "pngFileName",
            "pixelsPerUnit",
            "_comment",
            "_howToUse",
            "_example",
        };

        static readonly HashSet<string> SfxClipFields = new(StringComparer.Ordinal)
        {
            "id",
            "file",
            "_comment",
            "_howToUse",
            "_example",
        };

        static readonly HashSet<string> SfxClassificationFields = new(StringComparer.Ordinal)
        {
            "id",
            "clips",
            "_comment",
            "_howToUse",
            "_example",
        };

        static readonly HashSet<string> SfxWeightedClipFields = new(StringComparer.Ordinal)
        {
            "clipId",
            "weight",
        };

        static readonly HashSet<string> BgmTrackFields = new(StringComparer.Ordinal)
        {
            "id",
            "addressableKey",
            "file",
            "volumeMultiplier",
            "_comment",
            "_howToUse",
            "_example",
        };

        static readonly HashSet<string> BgmStageLoadoutFields = new(StringComparer.Ordinal)
        {
            "stageId",
            "bgmTrackId",
            "tenseBgmTrackId",
            "_comment",
            "_howToUse",
            "_example",
        };

        readonly ValidationReport report;

        public AssetReferenceValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void ValidateSpritesFolder(string folderPath)
        {
            ValidateFolder(folderPath, "sprites", "sprite", ValidateSpriteObject, "sprite.json");
        }

        public void ValidateAudioFilesFolder(string folderPath)
        {
            ValidateFolder(folderPath, "audioFiles", "audioFile", ValidateSfxClipObject);
        }

        public void ValidateClassificationFolder(string folderPath)
        {
            ValidateFolder(folderPath, "classificationID", "classification", ValidateSfxClassificationObject);
        }

        public void ValidateBgmTracksFolder(string folderPath)
        {
            ValidateFolder(folderPath, "bgmTracks", "bgmTrack", ValidateBgmTrackObject);
        }

        public void ValidateBgmStageLoadoutsFolder(string folderPath)
        {
            ValidateFolder(folderPath, "bgmStageLoadouts", "bgmStageLoadout", ValidateBgmStageLoadoutObject);
        }

        void ValidateFolder(
            string folderPath,
            string surfaceName,
            string scopeName,
            Action<string, JObject, string, string> validateObject,
            string preferredFileName = null)
        {
            string[] paths = JsonValidationUtil.EnumerateJsonFiles(folderPath, preferredFileName);

            if (paths.Length == 0)
            {
                report.Warn($"{report.ToRelativePath(folderPath)}: {surfaceName} entry folder has no .json files.");
                return;
            }

            foreach (string path in paths)
                ValidateObjectOrArrayFile(path, surfaceName, scopeName, validateObject);
        }

        void ValidateObjectOrArrayFile(
            string path,
            string surfaceName,
            string scopeName,
            Action<string, JObject, string, string> validateObject)
        {
            JToken token = JsonValidationUtil.TryReadToken(path, report);
            if (token == null)
                return;

            string relativePath = report.ToRelativePath(path);
            if (token is JObject obj)
            {
                validateObject(relativePath, obj, scopeName, path);
                return;
            }

            if (token is JArray arr)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is JObject entry)
                    {
                        validateObject(relativePath, entry, $"{scopeName}[{i}]", path);
                        continue;
                    }

                    report.Error($"{relativePath}: {scopeName}[{i}] should be an object.");
                }

                return;
            }

            report.Error($"{relativePath}: {surfaceName} file should contain a JSON object or array.");
        }

        void ValidateSpriteObject(string relativePath, JObject obj, string scope, string sourcePath)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SpriteFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "builtInSpriteAddress", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "spriteSourceId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "assetBundleFileName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "spriteAssetName", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "pngFileName", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "pixelsPerUnit", report, scope);

            bool hasBuiltInAddress = HasNonEmptyString(obj["builtInSpriteAddress"]);
            bool hasSpriteSource = HasNonEmptyString(obj["spriteSourceId"]);
            bool hasPng = HasNonEmptyString(obj["pngFileName"]);
            bool hasBundleFile = HasNonEmptyString(obj["assetBundleFileName"]);
            bool hasBundleAsset = HasNonEmptyString(obj["spriteAssetName"]);
            bool hasBundleSource = hasBundleFile && hasBundleAsset;

            if (hasBundleFile != hasBundleAsset)
                report.Warn($"{relativePath}: {scope} should define both assetBundleFileName and spriteAssetName for bundle sprites.");

            int sourceCount = (hasBuiltInAddress ? 1 : 0) + (hasSpriteSource ? 1 : 0) + (hasPng ? 1 : 0) + (hasBundleSource ? 1 : 0);
            if (sourceCount == 0)
                report.Error($"{relativePath}: {scope} must define one sprite source: builtInSpriteAddress, spriteSourceId, pngFileName, or assetBundleFileName + spriteAssetName.");
            else if (sourceCount > 1)
                report.Warn($"{relativePath}: {scope} should define exactly one sprite source.");
        }

        void ValidateSfxClipObject(string relativePath, JObject obj, string scope, string sourcePath)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SfxClipFields, scope, report);

            if (obj["id"] == null && Path.GetFileNameWithoutExtension(sourcePath)?.StartsWith("sfx.clip.", StringComparison.Ordinal) == true)
                report.Warn($"{relativePath}: {scope}.id is omitted; runtime will use the sfx.clip.* file name as the clip id.");
            else
                JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);

            JsonValidationUtil.RequireString(relativePath, obj, "file", report, scope);
        }

        void ValidateSfxClassificationObject(string relativePath, JObject obj, string scope, string sourcePath)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, SfxClassificationFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            ValidateWeightedClips(relativePath, obj["clips"], $"{scope}.clips");
        }

        void ValidateWeightedClips(string relativePath, JToken token, string scope)
        {
            if (token is not JArray arr)
            {
                report.Error($"{relativePath}: {scope} should be a non-empty array.");
                return;
            }

            if (arr.Count == 0)
            {
                report.Warn($"{relativePath}: {scope} should include at least one clip entry.");
                return;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] is not JObject clip)
                {
                    report.Warn($"{relativePath}: {scope}[{i}] should be an object.");
                    continue;
                }

                string clipScope = $"{scope}[{i}]";
                JsonValidationUtil.ValidateUnknownFields(relativePath, clip, SfxWeightedClipFields, clipScope, report);
                JsonValidationUtil.RequireString(relativePath, clip, "clipId", report, clipScope);
                JsonValidationUtil.WarnInteger(relativePath, clip, "weight", report, clipScope);
                WarnPositiveInteger(relativePath, clip["weight"], $"{clipScope}.weight");
            }
        }

        void ValidateBgmTrackObject(string relativePath, JObject obj, string scope, string sourcePath)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, BgmTrackFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "id", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "addressableKey", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "file", report, scope);
            JsonValidationUtil.WarnNumber(relativePath, obj, "volumeMultiplier", report, scope);

            bool hasAddressable = HasNonEmptyString(obj["addressableKey"]);
            bool hasFile = HasNonEmptyString(obj["file"]);
            if (hasAddressable == hasFile)
                report.Error($"{relativePath}: {scope} must define exactly one BGM source: addressableKey or file.");
        }

        void ValidateBgmStageLoadoutObject(string relativePath, JObject obj, string scope, string sourcePath)
        {
            JsonValidationUtil.ValidateUnknownFields(relativePath, obj, BgmStageLoadoutFields, scope, report);
            JsonValidationUtil.RequireString(relativePath, obj, "stageId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "bgmTrackId", report, scope);
            JsonValidationUtil.WarnString(relativePath, obj, "tenseBgmTrackId", report, scope);
        }

        void WarnPositiveInteger(string relativePath, JToken token, string scope)
        {
            if (token == null)
                return;

            if (token.Type != JTokenType.Integer)
                return;

            if (token.Value<int>() <= 0)
                report.Warn($"{relativePath}: {scope} should be greater than 0.");
        }

        static bool HasNonEmptyString(JToken token)
        {
            return token != null
                && token.Type == JTokenType.String
                && !string.IsNullOrWhiteSpace(token.Value<string>());
        }
    }
}
