using DependenciesProvider;
using Generations.Data;
using JetBrains.Annotations;
using UIWorkflow.Core;
using Unity.VisualScripting;

namespace UIWorkflow.Nodes {
    [PublicAPI]
    [UnitTitle("Show Window")]
    [UnitCategory("UI Workflow")]
    public class ShowWindowUnit : Unit {
        [DoNotSerialize] public ValueInput Window;
        [DoNotSerialize] public ControlInput Enter;
        [DoNotSerialize] public ControlOutput Exit;
        
        protected override void Definition() {
            Window = ValueInput<WindowNames>("Window", default);
            Enter = ControlInput("In", flow => {
                string windowName = flow.GetValue<WindowNames>(Window).ToString();
                WindowBehaviour windowBehaviour = (WindowBehaviour)ZenjectDependenciesProvider.Get(WindowsTypeMapper.GetType(windowName));
                windowBehaviour.Show();
                return Exit;
            });

            Exit = ControlOutput("Out");
            Succession(Enter, Exit);
        }
    }
}
