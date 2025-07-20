using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Features.AudioModule.Scripts.Nodes {
    [PublicAPI]
    [UnitTitle(nameof(PlayAudioInstanceNode))]
    [UnitCategory("Audio/FMOD")]
    public class PlayAudioInstanceNode : Unit {
        [DoNotSerialize] private ValueInput _audioInstance;
        [DoNotSerialize] private ValueInput _position;
        [DoNotSerialize] private ValueInput _transform;
        [DoNotSerialize] private ControlOutput _exit;

        protected override void Definition() {
            _audioInstance = ValueInput<ISoundEventInstanceEntity>("AudioInstance");
            _position = ValueInput("Position", Vector3.zero);
            _transform = ValueInput<Transform>("Transform", null);
            ControlInput("In", OnEnter);
            _exit = ControlOutput("Out");
        }
        
        private ControlOutput OnEnter(Flow flow) {
            ISoundEventInstanceEntity audioInstance = flow.GetValue<ISoundEventInstanceEntity>(_audioInstance);
            Vector3 position = flow.GetValue<Vector3>(_position);
            Transform transform = flow.GetValue<Transform>(_transform);
                
            if (audioInstance == null || !audioInstance.IsValid()) {
                Debug.LogWarning("Audio instance is null or invalid!");
                return _exit;
            }

            if (transform != null)
                audioInstance.PlaySound(transform);
            else if (position != Vector3.zero)
                audioInstance.PlaySound(position);
            else
                audioInstance.PlaySound();
            return _exit;
        }
    }
} 