using DependenciesProvider;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Features.AudioModule.Scripts.Nodes {
    [PublicAPI]
    [UnitTitle(nameof(PlayAudioOneShotNode))]
    [UnitCategory("Audio/FMOD")]
    public class PlayAudioOneShotNode : Unit {
        [DoNotSerialize] private ValueInput _audioType;
        [DoNotSerialize] private ValueInput _position;
        [DoNotSerialize] private ControlOutput _exit;

        protected override void Definition() {
            _audioType = ValueInput("AudioType", AudioId.None);
            _position = ValueInput("Position", Vector3.zero);
            ControlInput("In", OnEnter);
            _exit = ControlOutput("Out");
        }
        
        private ControlOutput OnEnter(Flow flow) {
            AudioId audioType = flow.GetValue<AudioId>(_audioType);
            Vector3 position = flow.GetValue<Vector3>(_position);
            IFmodAudioService audioService = ZenjectDependenciesProvider.Get<IFmodAudioService>();
            if (position == Vector3.zero)
                audioService.PlayAudioOneShot(audioType);
            else
                audioService.PlayAudioOneShot(audioType, position);

            return _exit;
        }
    }
}