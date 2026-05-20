namespace NurtaleNesche.Modding.Json
{
    /// <summary>
    /// Experimental public alias for the shared mod.json metadata root.
    /// This mirrors the current runtime contract without moving the existing loader type.
    /// </summary>
    [System.Serializable]
    public class ModDataBase : Mods.Types.ModDataBase
    {
    }

    /// <summary>
    /// Experimental public alias for standalone file-backed mods such as language.
    /// </summary>
    [System.Serializable]
    public class FileModDataBase : Mods.Types.FileModDataBase
    {
    }

    /// <summary>
    /// Experimental public alias for standalone actor-targeted file mods.
    /// </summary>
    [System.Serializable]
    public class ActorFileModDataBase : Mods.Types.ActorFileModDataBase
    {
    }

    /// <summary>
    /// Experimental public alias for actor-targeted folder-backed mods such as animation,
    /// animationClipOverride, and animatorController.
    /// </summary>
    [System.Serializable]
    public class ActorFolderModDataBase : Mods.Types.ActorFolderModDataBase
    {
    }

    /// <summary>
    /// Experimental public alias for folder-backed registry mods.
    /// </summary>
    [System.Serializable]
    public class FolderModDataBase : Mods.Types.FolderModDataBase
    {
    }

    /// <summary>
    /// Experimental public alias for folder-backed registry mod.json contracts used by ModLoader.
    /// </summary>
    [System.Serializable]
    public class FolderRegistryModData : Mods.Types.FolderRegistryModData
    {
    }
}
