using DependenciesProvider;
using Generations.Data;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

namespace SceneLoaderModule {
    [PublicAPI]
    [UnitTitle("Load Scene")]
    [UnitCategory("Scene Loader")]
    public class LoadSceneUnit : Unit {
        [DoNotSerialize] public ValueInput Scene;
        [DoNotSerialize] public ValueInput LoadSceneMode;
        [DoNotSerialize] public ControlOutput Exit;

        protected override void Definition() {
            Scene = ValueInput<SceneInBuild>("Scene", default); 
            LoadSceneMode = ValueInput<LoadSceneMode>("LoadMode", default); 
            
            ControlInput("In", flow => {
                SceneInBuild scene = flow.GetValue<SceneInBuild>(Scene);
                LoadSceneMode loadSceneMode = flow.GetValue<LoadSceneMode>(LoadSceneMode);
                ISceneSwitchService sceneSwitchService = ZenjectDependenciesProvider.Get<ISceneSwitchService>();
                sceneSwitchService.LoadScene(scene.ToString(), loadSceneMode);
                return Exit;
            });

            Exit = ControlOutput("Out");
        }
    }
}