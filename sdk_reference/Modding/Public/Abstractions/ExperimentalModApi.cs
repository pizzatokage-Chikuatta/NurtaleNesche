namespace NurtaleNesche.Modding
{
    /// <summary>
    /// Public constants for the current experimental modding API surface.
    /// v1 is warning-only and does not imply a compatibility guarantee.
    /// </summary>
    public static class ExperimentalModApi
    {
        public const string CurrentExperimentalApiVersion = "v1";
        public const string NamespaceRoot = "NurtaleNesche";
        public const string ModdingNamespaceRoot = "NurtaleNesche.Modding";

        public static string CurrentGameVersion => UnityEngine.Application.version;
    }
}
