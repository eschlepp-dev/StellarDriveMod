using MelonLoader;
using SDModKit;
using SDModKit.Api;
using SDModKit.Configuration;
using SDModKit.Content;
using SDModKit.Input;
using SDModKit.UI;
using SDModKit.Util;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(ChaosMod.Plugin), "Chaos Mod", "1.0.0", "Zedaich")]
[assembly: MelonGame("CuriousOwlGames", "StellarDrive")]

namespace ChaosMod
{
    // 1) MelonLoader entry point
    public sealed class Plugin : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("ChaosMod initializing...");
            // ALWAYS register behaviours before loading the pack
            ContentRegistry.Behaviour("chaosmod.plant", () => new PlantBehaviour());

            LoggerInstance.Msg("Behaviors registered!");

            // pack.json as an EmbeddedResource in the DLL
            ContentRegistry.LoadEmbedded(typeof(Plugin).Assembly, "pack.json");

            LoggerInstance.Msg("Pack registered!");

            // register the mod with the kit (own .cfg file under UserData/SDModKit/)
            ModKit.Register(new ChaosKitMod(), ConfigFile.For("chaosmod"));

            LoggerInstance.Msg("ChaosKitMod registered!");
            LoggerInstance.Msg("ChaosMod initialized!");
        }
    }

    // 2) The actual SDModKit mod
    public sealed class ChaosKitMod : SDMod
    {
        public override string Id   => "chaosmod";
        public override string Name => "Chaos Mod";

        public override bool Dev => true;

        private ConfigEntry<float> _speed;
        private Keybind _hello;

        public override void OnRegistered()
        {
            _speed = Config.Bind("General", "Speed", 1.5f, "Speed");
            _hello = Keybinds.Register(Config, "Keys", "Hello", Key.H, "Say hello");
            AddPage("Settings", DrawPage);
        }

        public override void OnWorldEnter() => Log.Info("World loaded with ChaosMod");

        public override void OnUpdate()
        {
            if (_hello.Pressed) Toasts.Show("Hello from ChaosMod");
        }

        private void DrawPage()
        {
            ModUi.Section("General", () =>
            {
                ModUi.Row("Speed", () =>
                    _speed.Value = ModUi.Slider(_speed.Value, 0f, 5f));
            });
        }
    }
}