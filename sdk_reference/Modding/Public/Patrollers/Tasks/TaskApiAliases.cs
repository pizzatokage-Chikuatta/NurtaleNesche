using Newtonsoft.Json.Linq;

namespace NurtaleNesche.Modding.Patrollers.Tasks
{
    /// <summary>
    /// Experimental public alias for custom patroller task providers.
    /// Inherits the current plain C# runtime implementation so v1 code mods do not need Unity components.
    /// </summary>
    public abstract class TaskProviderCore : Patroller.TaskProviderCore, Patroller.IPatrollerTaskProvider, global::IJsonConfigurable
    {
        public virtual void ApplyConfig(JObject cfg)
        {
        }

        public abstract Patroller.PatrollerTaskInfoContainer GetNewTask();
    }

    /// <summary>
    /// Experimental public alias for task-set providers that choose from task options.
    /// </summary>
    public abstract class TaskSetCore : Patroller.TaskSetCore
    {
    }

    /// <summary>
    /// Experimental public alias for configurable task options.
    /// </summary>
    public abstract class TaskOption : Patroller.TaskOption
    {
    }

    public interface IPatrollerTaskProvider : Patroller.IPatrollerTaskProvider
    {
    }

    /// <summary>
    /// Experimental integer priority constants matching the current runtime task priorities.
    /// </summary>
    public static class TaskPriorities
    {
        public const int Fallback = (int)Patroller.CommonTaskPriorityEnum.Fallback;
        public const int Idle = (int)Patroller.CommonTaskPriorityEnum.Idle;
        public const int Patrol = (int)Patroller.CommonTaskPriorityEnum.Patrol;
        public const int Guard = (int)Patroller.CommonTaskPriorityEnum.Guard;
        public const int MiscTaskLow = (int)Patroller.CommonTaskPriorityEnum.MiscTaskLow;
        public const int HandleCaptives = (int)Patroller.CommonTaskPriorityEnum.HandleCaptives;
        public const int CollectSlavesOnField = (int)Patroller.CommonTaskPriorityEnum.CollectSlavesOnField;
        public const int MiscTaskMedium = (int)Patroller.CommonTaskPriorityEnum.MiscTaskMedium;
        public const int Suspicion = (int)Patroller.CommonTaskPriorityEnum.Suspicion;
        public const int MiscTaskHigh = (int)Patroller.CommonTaskPriorityEnum.MiscTaskHigh;
        public const int Chase = (int)Patroller.CommonTaskPriorityEnum.Chase;
        public const int Attack = (int)Patroller.CommonTaskPriorityEnum.Attack;
        public const int TakeDamage = (int)Patroller.CommonTaskPriorityEnum.TakeDamage;
        public const int Orgasm = (int)Patroller.CommonTaskPriorityEnum.Orgasm;
        public const int Event = (int)Patroller.CommonTaskPriorityEnum.Event;
        public const int Die = (int)Patroller.CommonTaskPriorityEnum.Die;
    }

    /// <summary>
    /// Experimental helper for constructing current runtime task containers.
    /// </summary>
    public static class TaskInfo
    {
        public static Patroller.PatrollerTaskInfoContainer Create(
            Patroller.IPatrollerTaskProvider provider,
            global::State task,
            global::StateArgs args,
            int priority,
            string tag,
            bool busy = false)
        {
            return Create(
                provider,
                new System.Collections.Generic.List<global::State> { task },
                new System.Collections.Generic.List<global::StateArgs> { args },
                priority,
                tag,
                busy);
        }

        public static Patroller.PatrollerTaskInfoContainer Create(
            Patroller.IPatrollerTaskProvider provider,
            System.Collections.Generic.IEnumerable<global::State> tasks,
            System.Collections.Generic.IEnumerable<global::StateArgs> args,
            int priority,
            string tag,
            bool busy = false)
        {
            return Create(
                provider,
                tasks == null ? null : new System.Collections.Generic.List<global::State>(tasks),
                args == null ? null : new System.Collections.Generic.List<global::StateArgs>(args),
                priority,
                tag,
                busy);
        }

        public static Patroller.PatrollerTaskInfoContainer Create(
            Patroller.IPatrollerTaskProvider provider,
            System.Collections.Generic.List<global::State> tasks,
            System.Collections.Generic.List<global::StateArgs> args,
            int priority,
            string tag,
            bool busy = false)
        {
            return new Patroller.PatrollerTaskInfoContainer(
                provider,
                tasks,
                args,
                priority,
                null,
                null,
                busy ? Patroller.HurriednessEnum.Busy : Patroller.HurriednessEnum.Free,
                tag);
        }
    }

    /// <summary>
    /// Experimental convenience builder for composing current runtime state sequences.
    /// State and StateArgs are still v1 runtime-coupled and not stable SDK DTOs.
    /// </summary>
    public sealed class TaskSequenceBuilder
    {
        private readonly System.Collections.Generic.List<global::State> _tasks = new();
        private readonly System.Collections.Generic.List<global::StateArgs> _args = new();

        public TaskSequenceBuilder Then(global::State state, global::StateArgs args = null)
        {
            _tasks.Add(state);
            _args.Add(args);
            return this;
        }

        public Patroller.PatrollerTaskInfoContainer Build(
            Patroller.IPatrollerTaskProvider provider,
            int priority,
            string tag,
            bool busy = false)
        {
            return TaskInfo.Create(provider, _tasks, _args, priority, tag, busy);
        }
    }

    public interface IChaseTaskOption : Patroller.IChaseTaskOption
    {
    }

    public interface IAbilityTaskOption : Patroller.IAbilityTaskOption
    {
    }

    public interface IAbilityTaskOptionStateful : Patroller.IAbilityTaskOptionStateful
    {
    }

    public interface ISoloActionTaskOption : Patroller.ISoloActionTaskOption
    {
    }

    public interface ICollectCaptiveTaskOption : Patroller.ICollectCaptiveTaskOption
    {
    }

    public interface ICaptiveTreatingTaskOption : Patroller.ICaptiveTreatingTaskOption
    {
    }

    public interface IArrowHitEventTaskOption : Patroller.IArrowHitEventTaskOption
    {
    }

    public interface IPotionHitEventTaskOption : Patroller.IPotionHitEventTaskOption
    {
    }

    public interface ITaskOptionTickable : Patroller.ITaskOptionTickable
    {
    }

    public interface IEventTaskOptionStateful : Patroller.IEventTaskOptionStateful
    {
    }
}
