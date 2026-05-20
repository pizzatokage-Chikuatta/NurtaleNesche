using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.EventTaskOptions
{
    [PotionHitEventTaskOptionId("event_task_option.potion_hit.example_ignore")]
    public sealed class ExamplePotionHitOption : IPotionHitEventTaskOption
    {
        public int Priority => 0;
        public string OptionId => "event_task_option.potion_hit.example_ignore";

        public bool CanHandle(Patroller.PotionHitEventContext context)
        {
            return false;
        }

        public Patroller.PotionHitEventResult Handle(Patroller.PotionHitEventContext context)
        {
            return Patroller.PotionHitEventResult.NotHandled;
        }
    }
}
