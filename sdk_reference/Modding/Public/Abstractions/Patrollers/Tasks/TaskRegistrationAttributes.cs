using System;

namespace NurtaleNesche.Modding.Patrollers.Tasks
{
    /// <summary>
    /// Experimental public id attribute for code-mod task providers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TaskProviderIdAttribute : Attribute
    {
        public string Id { get; }

        public TaskProviderIdAttribute(string id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Experimental public id attribute for captive-treating task options.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CaptiveTreatingTaskOptionIdAttribute : Attribute
    {
        public string Id { get; }

        public CaptiveTreatingTaskOptionIdAttribute(string id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Experimental public id attribute for arrow-hit event task options.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ArrowHitEventTaskOptionIdAttribute : Attribute
    {
        public string Id { get; }

        public ArrowHitEventTaskOptionIdAttribute(string id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Experimental public id attribute for potion-hit event task options.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PotionHitEventTaskOptionIdAttribute : Attribute
    {
        public string Id { get; }

        public PotionHitEventTaskOptionIdAttribute(string id)
        {
            Id = id;
        }
    }
}
