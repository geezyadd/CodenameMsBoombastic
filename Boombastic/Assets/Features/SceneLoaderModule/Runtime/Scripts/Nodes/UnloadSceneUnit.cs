using DependenciesProvider;
using Generations.Data;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

namespace SceneLoaderModule {
    [PublicAPI]
    [UnitTitle("Unload Scene")]
    [UnitCategory("Scene Loader")]
    public class UnloadSceneUnit : Unit {
        [DoNotSerialize] public ValueInput Scene;
        [DoNotSerialize] public ValueInput UnloadSceneOptions;
        [DoNotSerialize] public ControlOutput Exit;

        protected override void Definition() {
            Scene = ValueInput<SceneInBuild>("Scene", default); 
            UnloadSceneOptions = ValueInput<UnloadSceneOptions>("UnloadSceneOptions", default); 
            
            ControlInput("In", flow => {
                SceneInBuild scene = flow.GetValue<SceneInBuild>(Scene);
                UnloadSceneOptions unloadSceneOptions = flow.GetValue<UnloadSceneOptions>(UnloadSceneOptions);
                ISceneSwitchService sceneSwitchService = ZenjectDependenciesProvider.Get<ISceneSwitchService>();
                sceneSwitchService.UnloadScene(scene.ToString(), unloadSceneOptions);
                return Exit;
            });

            Exit = ControlOutput("Out");
        }
    }
}