using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.CaptiveTreating
{
    [CaptiveTreatingTaskOptionId("task_option.captive_treating.example_noop")]
    public sealed class ExampleCaptiveTreatingOption : ICaptiveTreatingTaskOption
    {
        public Patroller.PatrollerTaskForTreatingCaptive GetCaptiveTreatmentTask(
            Patroller.BaseController controller,
            Patroller.PatrollerMemory memory)
        {
            return null;
        }
    }
}
