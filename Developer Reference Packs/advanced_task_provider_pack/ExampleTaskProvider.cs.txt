using Newtonsoft.Json.Linq;
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Tasks
{
    [TaskProviderId("task_provider.example.wait")]
    public sealed class ExampleWaitTaskProvider : TaskSetCore
    {
        float seconds = 1.5f;

        public override void ApplyConfig(JObject cfg)
        {
            float? configuredSeconds = cfg?["seconds"]?.Value<float?>();
            if (configuredSeconds.HasValue && configuredSeconds.Value > 0f)
                seconds = configuredSeconds.Value;
        }

        public override Patroller.PatrollerTaskInfoContainer GetNewTask()
        {
            if (!IsConditionMetForPriority(TaskPriorities.MiscTaskLow))
                return null;

            if (controller == null || controller.baseIdleState == null)
                return null;

            return new TaskSequenceBuilder()
                .Then(controller.baseIdleState, new global::StateArgs_Idle(seconds))
                .Build(this, TaskPriorities.MiscTaskLow, "ExampleWait");
        }
    }
}
