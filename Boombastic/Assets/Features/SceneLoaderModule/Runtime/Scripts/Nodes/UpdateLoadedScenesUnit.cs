using System.Collections.Generic;
using DependenciesProvider;
using Generations.Data;
using JetBrains.Annotations;
using Unity.VisualScripting;

namespace SceneLoaderModule {
    [PublicAPI]
    [UnitTitle("Update Loaded Scenes")]
    [UnitCategory("Scene Loader")]
    public class UpdateLoadedScenesUnit : Unit {
        [DoNotSerialize] public ValueInput MainScene;
        [DoNotSerialize] public ValueInput SubScenes;
        [DoNotSerialize] public ControlInput Enter;
        [DoNotSerialize] public ControlOutput Exit;

        protected override void Definition() {
            MainScene = ValueInput<SceneInBuild>("MainScene", default);
            SubScenes = ValueInput<List<SceneInBuild>>("Sub Scenes");
            
            Enter = ControlInput("In", flow => {
                SceneInBuild mainScene = flow.GetValue<SceneInBuild>(MainScene);
                List<string> subScenes = flow.GetValue<List<SceneInBuild>>(SubScenes).ConvertAll(scene => scene.ToString());
                ISceneSwitchService sceneSwitchService = ZenjectDependenciesProvider.Get<ISceneSwitchService>();
                sceneSwitchService.UpdateLoadedScenes(subScenes, mainScene.ToString());
                return Exit;
            });

            Exit = ControlOutput("Out");
            Succession(Enter, Exit);
        }
    }
}