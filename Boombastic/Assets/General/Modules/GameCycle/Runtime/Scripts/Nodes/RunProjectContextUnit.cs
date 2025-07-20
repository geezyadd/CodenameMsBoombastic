using JetBrains.Annotations;
using Unity.VisualScripting;
using Zenject;

namespace GameCycle {
    [PublicAPI]
    [UnitTitle("Run Project Context")]
    [UnitCategory("Zenject")]
    public class RunProjectContextUnit : Unit {
        [DoNotSerialize] public ControlInput Enter;
        [DoNotSerialize] public ControlOutput Exit;

        protected override void Definition() {
            Enter = ControlInput("In", _ => {
                ProjectContext projectContext = ProjectContext.Instance;
                if (projectContext != null)
                    projectContext.EnsureIsInitialized();
                
                return Exit;
            });

            Exit = ControlOutput("Out");
            Succession(Enter, Exit);
        }
    }
}