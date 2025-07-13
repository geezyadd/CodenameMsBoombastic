using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Features.AudioModule.Scripts.Nodes {
    [PublicAPI]
    [UnitTitle(nameof(StopAudioInstanceNode))]
    [UnitCategory("Audio/FMOD")]
    public class StopAudioInstanceNode : Unit {
        [DoNotSerialize] private ValueInput _audioInstance;
        [DoNotSerialize] private ValueInput _stopMode;
        [DoNotSerialize] private ValueInput _fadeDuration;
        [DoNotSerialize] private ControlOutput _exit;

        protected override void Definition() {
            _audioInstance = ValueInput<ISoundEventInstanceEntity>("AudioInstance");
            _stopMode = ValueInput("StopMode", STOP_MODE.IMMEDIATE);
            _fadeDuration = ValueInput("FadeDuration", 0f);
            ControlInput("In", OnEnter);
            _exit = ControlOutput("Out");
        }
        
        private ControlOutput OnEnter(Flow flow) {
            ISoundEventInstanceEntity audioInstance = flow.GetValue<ISoundEventInstanceEntity>(_audioInstance);
            STOP_MODE stopMode = flow.GetValue<STOP_MODE>(_stopMode);
            float fadeDuration = flow.GetValue<float>(_fadeDuration);
                
            if (audioInstance == null || !audioInstance.IsValid()) {
                Debug.LogWarning("Audio instance is null or invalid!");
                return _exit;
            }

            if (fadeDuration > 0f)
                audioInstance.StopSoundWithFade(fadeDuration);
            else
                audioInstance.StopSound(stopMode);

            return _exit;
        }
    }
} 