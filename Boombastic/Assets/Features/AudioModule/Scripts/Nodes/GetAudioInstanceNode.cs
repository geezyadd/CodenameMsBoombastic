using DependenciesProvider;
using JetBrains.Annotations;
using Unity.VisualScripting;

namespace Features.AudioModule.Scripts.Nodes {
    [PublicAPI]
    [UnitTitle(nameof(GetAudioInstanceNode))]
    [UnitCategory("Audio/FMOD")]
    public class GetAudioInstanceNode : Unit {
        [DoNotSerialize] private ValueInput _audioType;
        [DoNotSerialize] private ValueOutput _audioInstance;
        [DoNotSerialize] private ControlOutput _exit;

        protected override void Definition()
        {
            _audioType = ValueInput<AudioId>("AudioType", default);
            _audioInstance = ValueOutput<ISoundEventInstanceEntity>("AudioInstance");
            ControlInput("In", OnEnter);
        }

        private ControlOutput OnEnter(Flow flow) {
            AudioId audioType = flow.GetValue<AudioId>(_audioType);
            IFmodAudioService audioService = ZenjectDependenciesProvider.Get<IFmodAudioService>();
            ISoundEventInstanceEntity audioInstance = audioService.GetFmodEventInstanceEntity(audioType);
            flow.SetValue(_audioInstance, audioInstance);
            return _exit;
        }
    }
} 