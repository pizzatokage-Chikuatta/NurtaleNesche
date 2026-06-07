using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NurtaleNesche.Modding.Patrollers.Senses;
using Patroller;
using UnityEngine;

namespace ExampleMods.Senses
{
    public sealed class ExampleNearbyPlayerSense : Sense, ICaptiveDetector, global::IJsonConfigurable
    {
        const string SourceId = "sense.example.nearby_player";

        readonly List<CaptiveInfoOnOthers> cache = new();
        BaseController controller;
        PatrollerMemory memory;
        float distance = 4.5f;
        int priority = 2;

        public void ApplyConfig(JObject cfg)
        {
            if (cfg == null) return;

            distance = cfg["distance"]?.Value<float>() ?? distance;
            priority = cfg["priority"]?.Value<int>() ?? priority;
            distance = Mathf.Max(0.1f, distance);
        }

        public override bool Initialize()
        {
            if (!base.Initialize()) return false;

            controller = GetComponentInParent<BaseController>(true);
            memory = GetComponentInParent<PatrollerMemory>(true);
            return true;
        }

        public override bool UpdateList()
        {
            if (!base.UpdateList()) return false;

            memory?.StageForgetCaptivesBySource(SourceId);

            var region = controller?.Commons?.CurrentRegion;
            if (region?.CaptivesStaying == null)
                return true;

            foreach (var captive in region.CaptivesStaying)
            {
                TryObserve(captive);
            }

            return true;
        }

        void TryObserve(ICaptive captive)
        {
            if (controller == null || memory == null) return;
            if (captive == null) return;
            if (!captive.IsPlayer()) return;
            if (captive.GetTransform() == null) return;
            if (captive.GetCheckpoint() == null) return;
            if (captive.GetCheckpoint().Commons.CurrentRegion != controller.Commons.CurrentRegion) return;
            if (Vector2.Distance(controller.transform.position, captive.GetTransform().position) > distance) return;

            memory.StageCaptiveObservation(SourceId, captive, priority);
        }

        public void Sweep()
        {
            memory?.StageForgetCaptivesBySource(SourceId);
        }

        public List<CaptiveInfoOnOthers> GetCaptivesDetected()
        {
            if (memory == null)
                cache.Clear();
            else
                memory.CopyCaptiveObservationsTo(SourceId, cache, clone: false);

            return cache;
        }
    }
}
