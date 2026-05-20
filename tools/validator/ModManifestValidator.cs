using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class ModManifestValidator
    {
        static readonly HashSet<string> FolderBackedModTypes = new(StringComparer.Ordinal)
        {
            "statusMetadata",
            "statusLoadouts",
            "actionRules",
            "animationTrackLayouts",
            "dropTables",
            "dropTableLoadouts",
            "itemCombinations",
            "itemCombinationLoadouts",
            "bgmTracks",
            "bgmStageLoadouts",
            "audioFiles",
            "classificationID",
            "shadowProfiles",
            "itemNames",
            "itemDefinitions",
            "equipmentDefinitions",
            "itemBoxPrefabs",
            "itemBoxSpawns",
            "itemBoxSpawnLoadouts",
            "patrollerData",
            "playerStats",
            "patrollerSpawns",
            "patrollerSpawnLoadouts",
            "patrollerDirectors",
            "patrollerDirectorStageLoadouts",
            "equipmentUi",
            "sprites",
            "animationSFX",
            "potionEffects",
            "interactionOptionMeta",
            "interactionLoadouts",
            "itemSelectionRequestDefinitions",
            "chainPointDefinitions",
            "chainPointSpawns",
            "chainPointSpawnLoadouts",
        };

        static readonly HashSet<string> ActorFolderModTypes = new(StringComparer.Ordinal)
        {
            "animation",
            "animationClipOverride",
            "animatorController",
        };

        static readonly HashSet<string> StandaloneModTypes = new(StringComparer.Ordinal)
        {
            "language",
        };

        readonly ValidationReport report;

        public ModManifestValidator(ValidationReport report)
        {
            this.report = report;
        }

        public void Validate(string modsRoot)
        {
            string[] modJsonPaths = Directory.GetFiles(modsRoot, "mod.json", SearchOption.AllDirectories)
                .OrderByDescending(path => GetDepth(Path.GetDirectoryName(path)))
                .ThenBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            report.Info($"Discovered {modJsonPaths.Length} mod.json file(s).");

            foreach (string jsonPath in modJsonPaths)
                ValidateModJson(jsonPath);
        }

        void ValidateModJson(string jsonPath)
        {
            JObject obj = JsonValidationUtil.TryReadObject(jsonPath, report);
            if (obj == null)
                return;

            string type = Trim(obj.Value<string>("type"));
            string version = Trim(obj.Value<string>("experimentalApiVersion"));
            string relativePath = report.ToRelativePath(jsonPath);

            if (string.IsNullOrWhiteSpace(version))
                report.Warn($"{relativePath}: missing experimentalApiVersion. Current warning-only value is '{ModdingV1ValidationCore.SupportedExperimentalApiVersion}'.");
            else if (version != ModdingV1ValidationCore.SupportedExperimentalApiVersion)
                report.Warn($"{relativePath}: unsupported experimentalApiVersion '{version}'. Current warning-only value is '{ModdingV1ValidationCore.SupportedExperimentalApiVersion}'.");

            if (string.IsNullOrWhiteSpace(type))
            {
                report.Error($"{relativePath}: missing mod type.");
                return;
            }

            if (type == "animationClip")
            {
                report.Error($"{relativePath}: legacy mod type 'animationClip' is unsupported. Use 'animation'.");
                return;
            }

            if (FolderBackedModTypes.Contains(type))
            {
                ValidateFolderBackedMod(jsonPath, obj, type);
                return;
            }

            if (ActorFolderModTypes.Contains(type))
            {
                ValidateActorFolderMod(jsonPath, obj, type);
                return;
            }

            if (StandaloneModTypes.Contains(type))
            {
                ValidateStandaloneMod(jsonPath, obj, type);
                return;
            }

            report.Error($"{relativePath}: unknown mod type '{type}'.");
        }

        void ValidateFolderBackedMod(string jsonPath, JObject obj, string type)
        {
            string entry = Trim(obj.Value<string>("entry"));
            string relativePath = report.ToRelativePath(jsonPath);

            if (string.IsNullOrWhiteSpace(entry))
            {
                report.Error($"{relativePath}: folder-backed mod type '{type}' requires entry.");
                return;
            }

            string folderPath = Path.Combine(Path.GetDirectoryName(jsonPath) ?? string.Empty, entry);
            if (!Directory.Exists(folderPath))
            {
                report.Error($"{relativePath}: entry folder does not exist: {entry}");
                return;
            }

            if (type == "patrollerData")
                new PatrollerDataValidator(report).ValidateFolder(folderPath);

            if (type == "playerStats")
                new PlayerStatValidator(report).ValidateFolder(folderPath);

            if (type == "statusMetadata")
                new StatusMetadataValidator(report).ValidateFolder(folderPath);

            if (type == "statusLoadouts")
                new StatusLoadoutValidator(report).ValidateFolder(folderPath);

            if (type == "itemDefinitions")
                new ItemDefinitionValidator(report).ValidateItemDefinitionsFolder(folderPath);

            if (type == "equipmentDefinitions")
                new ItemDefinitionValidator(report).ValidateEquipmentDefinitionsFolder(folderPath);

            if (type == "itemNames")
                new ItemNameValidator(report).ValidateFolder(folderPath);

            if (type == "dropTables")
                new LootingValidator(report).ValidateDropTablesFolder(folderPath);

            if (type == "dropTableLoadouts")
                new LootingValidator(report).ValidateDropTableLoadoutsFolder(folderPath);

            if (type == "itemCombinations")
                new LootingValidator(report).ValidateItemCombinationsFolder(folderPath);

            if (type == "itemCombinationLoadouts")
                new LootingValidator(report).ValidateItemCombinationLoadoutsFolder(folderPath);

            if (type == "itemBoxPrefabs")
                new ItemBoxValidator(report).ValidatePrefabsFolder(folderPath);

            if (type == "itemBoxSpawns")
                new ItemBoxValidator(report).ValidateSpawnsFolder(folderPath);

            if (type == "itemBoxSpawnLoadouts")
                new ItemBoxValidator(report).ValidateSpawnLoadoutsFolder(folderPath);

            if (type == "patrollerSpawns")
                new PatrollerSpawnValidator(report).ValidateSpawnsFolder(folderPath);

            if (type == "patrollerSpawnLoadouts")
                new PatrollerSpawnValidator(report).ValidateSpawnLoadoutsFolder(folderPath);

            if (type == "patrollerDirectors")
                new PatrollerDirectorValidator(report).ValidateDirectorsFolder(folderPath);

            if (type == "patrollerDirectorStageLoadouts")
                new PatrollerDirectorValidator(report).ValidateStageLoadoutsFolder(folderPath);

            if (type == "actionRules")
                new ActionRuleValidator(report).ValidateFolder(folderPath);

            if (type == "animationTrackLayouts")
                new AnimationTrackLayoutValidator(report).ValidateFolder(folderPath);

            if (type == "potionEffects")
                new PotionEffectValidator(report).ValidateFolder(folderPath);

            if (type == "shadowProfiles")
                new ShadowProfileValidator(report).ValidateFolder(folderPath);

            if (type == "chainPointDefinitions")
                new ChainpointValidator(report).ValidateDefinitionsFolder(folderPath);

            if (type == "chainPointSpawns")
                new ChainpointValidator(report).ValidateSpawnsFolder(folderPath);

            if (type == "chainPointSpawnLoadouts")
                new ChainpointValidator(report).ValidateSpawnLoadoutsFolder(folderPath);

            if (type == "sprites")
                new AssetReferenceValidator(report).ValidateSpritesFolder(folderPath);

            if (type == "audioFiles")
                new AssetReferenceValidator(report).ValidateAudioFilesFolder(folderPath);

            if (type == "classificationID")
                new AssetReferenceValidator(report).ValidateClassificationFolder(folderPath);

            if (type == "bgmTracks")
                new AssetReferenceValidator(report).ValidateBgmTracksFolder(folderPath);

            if (type == "bgmStageLoadouts")
                new AssetReferenceValidator(report).ValidateBgmStageLoadoutsFolder(folderPath);
        }

        void ValidateActorFolderMod(string jsonPath, JObject obj, string type)
        {
            string target = Trim(obj.Value<string>("target"));
            string entry = Trim(obj.Value<string>("entry"));
            string legacyFile = Trim(obj.Value<string>("file"));
            string relativePath = report.ToRelativePath(jsonPath);

            if (string.IsNullOrWhiteSpace(target))
                report.Error($"{relativePath}: actor-targeted mod type '{type}' requires target.");

            if (string.IsNullOrWhiteSpace(entry))
            {
                if (!string.IsNullOrWhiteSpace(legacyFile))
                    report.Error($"{relativePath}: legacy 'file' contract is unsupported for '{type}'. Use entry folder.");
                else
                    report.Error($"{relativePath}: actor-targeted mod type '{type}' requires entry.");

                return;
            }

            string folderPath = Path.Combine(Path.GetDirectoryName(jsonPath) ?? string.Empty, entry);
            if (!Directory.Exists(folderPath))
                report.Error($"{relativePath}: entry folder does not exist: {entry}");
        }

        void ValidateStandaloneMod(string jsonPath, JObject obj, string type)
        {
            if (type != "language")
                return;

            string languageCode = Trim(obj.Value<string>("languageCode"));
            if (string.IsNullOrWhiteSpace(languageCode))
                report.Warn($"{report.ToRelativePath(jsonPath)}: language mod should declare languageCode.");
        }

        static int GetDepth(string path)
        {
            if (string.IsNullOrEmpty(path))
                return 0;

            return path.Count(c => c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar);
        }

        static string Trim(string value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
