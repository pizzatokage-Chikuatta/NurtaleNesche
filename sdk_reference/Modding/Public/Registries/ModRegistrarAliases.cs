namespace NurtaleNesche.Modding.Registries
{
    /// <summary>
    /// Experimental public alias for task-provider registrar classes.
    /// </summary>
    public interface ITaskProviderModRegistrar : Patroller.ITaskProviderModRegistrar
    {
    }

    /// <summary>
    /// Experimental public alias for event-task option registrar classes.
    /// </summary>
    public interface IEventTaskOptionModRegistrar : Patroller.IEventTaskOptionModRegistrar
    {
    }

    /// <summary>
    /// Experimental public alias for captive-treating task option registrar classes.
    /// </summary>
    public interface ICaptiveTreatingTaskOptionModRegistrar : Patroller.ICaptiveTreatingTaskOptionModRegistrar
    {
    }

    /// <summary>
    /// Experimental public alias for sense registrar classes.
    /// </summary>
    public interface ISenseModRegistrar : global::ISenseModRegistrar
    {
    }

    /// <summary>
    /// Experimental public alias for status-effect factory registrar classes.
    /// </summary>
    public interface IStatusEffectFactoryModRegistrar : global::IStatusEffectFactoryModRegistrar
    {
    }

    /// <summary>
    /// Experimental public alias for item-selection request factory registrar classes.
    /// </summary>
    public interface IItemSelectionRequestFactoryModRegistrar : global::IItemSelectionRequestFactoryModRegistrar
    {
    }

    /// <summary>
    /// Experimental helper methods for registrar classes. These wrap current runtime registries
    /// without making those registries stable SDK abstractions.
    /// </summary>
    public static class ModRegistryExtensions
    {
        public static void RegisterTaskProvider<TProvider>(
            this global::TaskProviderRegistry registry,
            string id,
            bool allowOverride = true)
            where TProvider : Patroller.TaskProviderCore, new()
        {
            registry.RegisterPlainProvider(id, () => new TProvider(), allowOverride);
        }

        public static void RegisterComponentTaskProvider<TProvider>(
            this global::TaskProviderRegistry registry,
            string id,
            bool allowOverride = true)
            where TProvider : Patroller.TaskProvider
        {
            registry.Register(id, typeof(TProvider), allowOverride);
        }

        public static void RegisterCaptiveTreatingTaskOption<TOption>(
            this Patroller.CaptiveTreatingTaskOptionRegistry registry,
            string id,
            bool allowOverride = true)
            where TOption : Patroller.ICaptiveTreatingTaskOption
        {
            registry.Register(id, typeof(TOption), allowOverride);
        }

        public static void RegisterCollectCaptiveTaskOption<TOption>(
            this Patroller.CollectCaptiveTaskOptionRegistry registry,
            string id,
            bool allowOverride = true)
            where TOption : Patroller.ICollectCaptiveTaskOption
        {
            registry.Register(id, typeof(TOption), allowOverride);
        }

        public static void RegisterSoloActionTaskOption<TOption>(
            this Patroller.SoloActionTaskOptionRegistry registry,
            string id,
            bool allowOverride = true)
            where TOption : Patroller.ISoloActionTaskOption
        {
            registry.Register(id, typeof(TOption), allowOverride);
        }

        public static void RegisterChaseTaskOption<TOption>(
            this Patroller.ChaseTaskOptionRegistry registry,
            string id,
            bool allowOverride = true)
            where TOption : Patroller.IChaseTaskOption
        {
            registry.Register(id, typeof(TOption), allowOverride);
        }

        public static void RegisterAbilityTaskOption<TOption>(
            this Patroller.AbilityTaskOptionRegistry registry,
            string id,
            bool allowOverride = true)
            where TOption : Patroller.IAbilityTaskOption
        {
            registry.Register(id, typeof(TOption), allowOverride);
        }

        public static void RegisterArrowHitEventTaskOption<TOption>(
            this Patroller.EventTaskOptionRegistry registry,
            string id,
            bool allowOverride = true)
            where TOption : Patroller.IArrowHitEventTaskOption
        {
            registry.RegisterArrow(id, typeof(TOption), allowOverride);
        }

        public static void RegisterPotionHitEventTaskOption<TOption>(
            this Patroller.EventTaskOptionRegistry registry,
            string id,
            bool allowOverride = true)
            where TOption : Patroller.IPotionHitEventTaskOption
        {
            registry.RegisterPotion(id, typeof(TOption), allowOverride);
        }

        public static void RegisterSense<TSense>(
            this global::SenseFactoryRegistry registry,
            string id,
            bool allowOverride = true)
            where TSense : UnityEngine.Component
        {
            registry.Register(id, typeof(TSense), allowOverride);
        }

        public static void RegisterStatusEffect<TStatusEffect>(
            this global::StatusEffectFactoryRegistry registry,
            string effectId)
            where TStatusEffect : global::IStatusEffect, new()
        {
            registry.Register(effectId, () => new TStatusEffect());
        }

        public static void RegisterStatusEffect<TStatusEffect>(
            this global::StatusEffectFactoryRegistry registry,
            string effectId,
            string profileId)
            where TStatusEffect : global::IStatusEffect, new()
        {
            registry.Register(effectId, profileId, () => new TStatusEffect());
        }

        public static void RegisterItemSelectionRequestFactory<TFactory>(
            this global::ItemSelectionRequestFactoryRegistry registry,
            string factoryId)
            where TFactory : global::IItemSelectionRequestFactory, new()
        {
            registry.Register(factoryId, new TFactory());
        }
    }
}
