using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NurtaleNesche.Modding.Validation
{
    internal sealed class CodeManifestValidator
    {
        static readonly ManifestShape[] CodeManifestShapes =
        {
            new("*.task_providers.json", "providers"),
            new("*.event_task_options.json", "arrowOptions", "potionOptions"),
            new("*.captive_treating_task_options.json", "taskOptions", "actionSets"),
            new("*.captive_treating_action_sets.json", "taskOptions", "actionSets"),
            new("*.senses.json", "senses"),
            new("*.status_effect_factories.json", "factories"),
            new("*.item_selection_request_factories.json", "factories"),
        };

        static readonly HashSet<string> DefaultCodeManifestEntryFields = new(StringComparer.Ordinal)
        {
            "id",
            "type",
            "overrideExisting",
        };

        static readonly HashSet<string> StatusFactoryManifestEntryFields = new(StringComparer.Ordinal)
        {
            "id",
            "profileId",
            "type",
            "overrideExisting",
        };

        readonly ValidationReport report;
        readonly CodeManifestTypeResolver typeResolver;

        public CodeManifestValidator(ValidationReport report, ValidationOptions options, string modsRoot)
        {
            this.report = report;
            typeResolver = CodeManifestTypeResolver.Create(modsRoot, options, report);
        }

        public void Validate(string modsRoot)
        {
            foreach (ManifestShape shape in CodeManifestShapes)
            {
                string[] paths = Directory.GetFiles(modsRoot, shape.Pattern, SearchOption.AllDirectories)
                    .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                foreach (string path in paths)
                    ValidateCodeManifest(path, shape);
            }
        }

        void ValidateCodeManifest(string path, ManifestShape shape)
        {
            JObject obj = JsonValidationUtil.TryReadObject(path, report);
            if (obj == null)
                return;

            string relativePath = report.ToRelativePath(path);
            ValidateCodeManifestVersion(relativePath, obj);
            ValidateCodeManifestRootFields(relativePath, obj, shape);

            bool foundAnyList = false;
            foreach (string listName in shape.ListNames)
            {
                JToken token = obj[listName];
                if (token == null)
                    continue;

                foundAnyList = true;
                if (token is not JArray arr)
                {
                    report.Error($"{relativePath}: {listName} must be an array.");
                    continue;
                }

                if (listName == "actionSets")
                    report.Warn($"{relativePath}: actionSets is a legacy code-manifest field. Prefer taskOptions.");

                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] is not JObject entry)
                    {
                        report.Error($"{relativePath}: {listName}[{i}] must be an object.");
                        continue;
                    }

                    ValidateCodeManifestEntryFields(relativePath, shape, listName, i, entry);

                    string id = TokenString(entry["id"]);
                    string type = TokenString(entry["type"]);
                    if (string.IsNullOrWhiteSpace(id))
                        report.Error($"{relativePath}: {listName}[{i}] is missing id.");
                    if (string.IsNullOrWhiteSpace(type))
                        report.Error($"{relativePath}: {listName}[{i}] is missing type.");
                    else if (typeResolver != null && !typeResolver.TryResolve(type))
                        report.Warn($"{relativePath}: {listName}[{i}].type '{type}' could not be resolved from validation-loaded assemblies.");
                }
            }

            if (!foundAnyList)
                report.Warn($"{relativePath}: no known registration list found for pattern {shape.Pattern}.");
        }

        void ValidateCodeManifestVersion(string relativePath, JObject obj)
        {
            JToken versionToken = obj["experimentalApiVersion"];
            string version = TokenString(versionToken);

            if (versionToken == null || string.IsNullOrWhiteSpace(version))
            {
                report.Warn($"{relativePath}: missing experimentalApiVersion. Current warning-only value is '{ModdingV1ValidationCore.SupportedExperimentalApiVersion}'.");
                return;
            }

            if (versionToken.Type != JTokenType.String)
                report.Warn($"{relativePath}: experimentalApiVersion should be a string.");

            if (version != ModdingV1ValidationCore.SupportedExperimentalApiVersion)
                report.Warn($"{relativePath}: unsupported experimentalApiVersion '{version}'. Current warning-only value is '{ModdingV1ValidationCore.SupportedExperimentalApiVersion}'.");
        }

        void ValidateCodeManifestRootFields(string relativePath, JObject obj, ManifestShape shape)
        {
            var allowedFields = new HashSet<string>(shape.ListNames, StringComparer.Ordinal)
            {
                "experimentalApiVersion",
            };

            foreach (JProperty property in obj.Properties())
            {
                if (allowedFields.Contains(property.Name))
                    continue;

                report.Warn($"{relativePath}: unknown top-level code-manifest field '{property.Name}' for pattern {shape.Pattern}.");
            }
        }

        void ValidateCodeManifestEntryFields(
            string relativePath,
            ManifestShape shape,
            string listName,
            int index,
            JObject entry)
        {
            HashSet<string> allowedFields = shape.Pattern == "*.status_effect_factories.json"
                ? StatusFactoryManifestEntryFields
                : DefaultCodeManifestEntryFields;

            foreach (JProperty property in entry.Properties())
            {
                if (allowedFields.Contains(property.Name))
                    continue;

                report.Warn($"{relativePath}: unknown code-manifest field '{listName}[{index}].{property.Name}'.");
            }

            JToken idToken = entry["id"];
            if (idToken != null && idToken.Type != JTokenType.String)
                report.Warn($"{relativePath}: {listName}[{index}].id should be a string.");

            JToken typeToken = entry["type"];
            if (typeToken != null && typeToken.Type != JTokenType.String)
                report.Warn($"{relativePath}: {listName}[{index}].type should be a string.");

            JToken profileIdToken = entry["profileId"];
            if (profileIdToken != null && profileIdToken.Type != JTokenType.String)
                report.Warn($"{relativePath}: {listName}[{index}].profileId should be a string.");

            JToken overrideExistingToken = entry["overrideExisting"];
            if (overrideExistingToken != null && overrideExistingToken.Type != JTokenType.Boolean)
                report.Warn($"{relativePath}: {listName}[{index}].overrideExisting should be a boolean.");
        }

        static string TokenString(JToken token)
        {
            if (token == null)
                return null;

            return Trim(token.Type == JTokenType.String ? token.Value<string>() : token.ToString());
        }

        static string Trim(string value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        readonly struct ManifestShape
        {
            public ManifestShape(string pattern, params string[] listNames)
            {
                Pattern = pattern;
                ListNames = listNames;
            }

            public string Pattern { get; }
            public string[] ListNames { get; }
        }

        sealed class CodeManifestTypeResolver
        {
            readonly List<Assembly> assemblies;

            CodeManifestTypeResolver(List<Assembly> assemblies)
            {
                this.assemblies = assemblies;
            }

            public static CodeManifestTypeResolver Create(string modsRoot, ValidationOptions options, ValidationReport report)
            {
                if (options == null || !options.ResolveCodeManifestTypes)
                    return null;

                var assemblies = new List<Assembly>();
                assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(asm => asm != null));

                int loadedDllCount = 0;
                if (options.LoadDllsFromModsRootForTypeResolution && Directory.Exists(modsRoot))
                {
                    foreach (string dllPath in Directory.GetFiles(modsRoot, "*.dll", SearchOption.AllDirectories)
                        .OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
                    {
                        Assembly loaded = TryLoadAssemblyForDiagnostics(dllPath, report);
                        if (loaded == null)
                            continue;

                        assemblies.Add(loaded);
                        loadedDllCount++;
                    }
                }

                foreach (string path in options.AdditionalAssemblyPaths ?? Array.Empty<string>())
                {
                    Assembly loaded = TryLoadAssemblyForDiagnostics(path, report);
                    if (loaded != null)
                        assemblies.Add(loaded);
                }

                report.Info($"Code manifest type-resolution diagnostics enabled. validationLoadedDlls={loadedDllCount}, assemblies={assemblies.Count}.");
                return new CodeManifestTypeResolver(assemblies);
            }

            static Assembly TryLoadAssemblyForDiagnostics(string path, ValidationReport report)
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                    return null;

                try
                {
                    return Assembly.Load(File.ReadAllBytes(path));
                }
                catch (Exception e)
                {
                    report.Warn($"{report.ToRelativePath(path)}: failed to load DLL for code-manifest type-resolution diagnostics: {e.GetType().Name}: {e.Message}");
                    return null;
                }
            }

            public bool TryResolve(string typeName)
            {
                if (string.IsNullOrWhiteSpace(typeName))
                    return false;

                typeName = typeName.Trim();
                foreach (Assembly assembly in assemblies)
                {
                    try
                    {
                        if (assembly.GetType(typeName, throwOnError: false) != null)
                            return true;
                    }
                    catch
                    {
                        // Validation diagnostics must never fail the whole report.
                    }
                }

                return false;
            }
        }
    }
}
