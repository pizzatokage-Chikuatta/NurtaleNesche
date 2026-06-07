using NurtaleNesche.Modding.StatusEffects;
using UnityEngine;

namespace ExampleMods.StatusEffects
{
    public sealed class ExampleStatusEffect : StatusEffect
    {
        public override void OnEntry()
        {
            base.OnEntry();
            Debug.Log("[ExampleCodeMod] ExampleStatusEffect entered.");
        }

        public override void OnExit()
        {
            Debug.Log("[ExampleCodeMod] ExampleStatusEffect exited.");
            base.OnExit();
        }
    }
}
