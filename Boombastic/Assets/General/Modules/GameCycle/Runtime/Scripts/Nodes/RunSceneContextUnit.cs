using JetBrains.Annotations;
using Unity.VisualScripting;
using Zenject;

namespace GameCycle {
    [PublicAPI]
    [UnitTitle("Run Scene Context")]
    [UnitCategory("Zenject")]
    public class RunSceneContextUnit : Unit {
        [DoNotSerialize] public ValueInput SceneContextInput;
        [DoNotSerialize] public ControlInput Enter;
        [DoNotSerialize] public ControlOutput Exit;

        protected override void Definition() {
            SceneContextInput = ValueInput<SceneContext>("SceneContext");
            Enter = ControlInput("In", flow => {
                SceneContext context = flow.GetValue<SceneContext>(SceneContextInput);
                if (context != null)
                    context.Run();
                
                return Exit;
            });

            Exit = ControlOutput("Out");
        }
    }
}