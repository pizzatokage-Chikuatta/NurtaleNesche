using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.EventTaskOptions
{
    [ArrowHitEventTaskOptionId("event_task_option.arrow_hit.example_ignore")]
    public sealed class ExampleArrowHitOption : IArrowHitEventTaskOption
    {
        public int Priority => 0;
        public string OptionId => "event_task_option.arrow_hit.example_ignore";

        public bool CanHandle(Patroller.ArrowHitEventContext context)
        {
            return false;
        }

        public Patroller.ArrowHitEventResult Handle(Patroller.ArrowHitEventContext context)
        {
            return Patroller.ArrowHitEventResult.NotHandled;
        }
    }
}
