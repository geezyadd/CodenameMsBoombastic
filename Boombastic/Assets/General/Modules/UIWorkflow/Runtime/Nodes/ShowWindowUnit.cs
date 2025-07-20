using Generations.Data;
using JetBrains.Annotations;
using Unity.VisualScripting;

namespace UIWorkflow.Nodes {
    [PublicAPI]
    [UnitTitle("Show Window")]
    [UnitCategory("UI Workflow")]
    public class ShowWindowUnit : Unit {
        [DoNotSerialize] public ValueInput Window;
        [DoNotSerialize] public ControlOutput Exit;
        
        protected override void Definition() {
            Window = ValueInput<WindowNames>("Window", default);
            ControlInput("In", flow => {
                
                return Exit;
            });

            Exit = ControlOutput("Out");
        }
    }
}
