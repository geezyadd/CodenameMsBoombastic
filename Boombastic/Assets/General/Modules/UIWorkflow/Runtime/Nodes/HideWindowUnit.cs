using DependenciesProvider;
using Generations.Data;
using JetBrains.Annotations;
using UIWorkflow.Core;
using Unity.VisualScripting;

namespace UIWorkflow.Nodes {
    [PublicAPI]
    [UnitTitle("Hide Window")]
    [UnitCategory("UI Workflow")]
    public class HideWindowUnit : Unit {
        [DoNotSerialize] public ValueInput Window;
        [DoNotSerialize] public ValueInput Close;
        [DoNotSerialize] public ControlInput Enter;
        [DoNotSerialize] public ControlOutput Exit;
        
        protected override void Definition() {
            Window = ValueInput<WindowNames>("Window", default);
            Close = ValueInput<bool>("Close?", default);
            Enter = ControlInput("In", flow => {
                string windowName = flow.GetValue<WindowNames>(Window).ToString();
                bool toClose = flow.GetValue<bool>(Close);
                WindowBehaviour windowBehaviour = (WindowBehaviour)ZenjectDependenciesProvider.Get(WindowsTypeMapper.GetType(windowName));
                if (toClose)
                    windowBehaviour.Close();
                else
                    windowBehaviour.Hide();
                
                return Exit;
            });

            Exit = ControlOutput("Out");
            Succession(Enter, Exit);
        }
    }
}