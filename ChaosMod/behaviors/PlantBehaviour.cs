using System.Collections.Generic;
using SDModKit.Content;
using SDModKit.UI;
using UnityEngine;
using Players;

namespace ChaosMod
{
    public sealed class PlantBehaviour : PartBehaviour
    {
        // Host: water slowly drains
        public override void ServerTick(ContentPart part, float dt)
        {
            float water = part.Config.GetFloat("water", 1f);
            float rate  = part.Config.GetInt("mode", 0) == 1 ? 0.005f : 0.01f;
            float next  = Mathf.Max(0f, water - rate * dt);
            if (Mathf.Abs(next - water) > 0.001f) part.Config.Set("water", next);
        }

        public override string ConfigTitle(ContentPart part) => "Potted plant";

        // Host + client: menu rows
        public override void DescribeConfig(ContentPart part, List<ConfigRow> rows)
        {
            float water = part.Config.GetFloat("water", 1f);
            int   mode  = part.Config.GetInt("mode", 0);

            rows.Add(new ConfigRow { Section = "Plant" });
            rows.Add(new ConfigRow
            {
                Label      = "Water",
                Value      = Mathf.RoundToInt(water * 100f) + " %",
                ValueColor = water < 0.2f ? Theme.Bad : Theme.Good
            });
            rows.Add(new ConfigRow
            {
                Label    = "Mode",
                Value    = mode == 0 ? "Normal" : "Economy",
                Choices  = new[] { "Normal", "Economy" },
                OnChoose = i => part.Config.Set("mode", i)      // menu refreshes itself afterwards
            });
            rows.Add(new ConfigRow
            {
                Label    = "Water now",
                Choices  = new[] { "Water" },
                OnChoose = _ => ContentActions.Send(part, "water") // runs on the host
            });
        }

        // Host: action from the player
        public override void OnAction(ContentPart part, TrackedPlayerServer player, string action)
        {
            if (action == "water") part.Config.Set("water", 1f);
        }
    }
}