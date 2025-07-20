using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Features.AudioModule.Scripts.Nodes {
    [PublicAPI]
    [UnitTitle(nameof(AudioInstanceControlNode))]
    [UnitCategory("Audio/FMOD")]
    public class AudioInstanceControlNode : Unit {
        [DoNotSerialize] private ValueInput _audioInstance;
        [DoNotSerialize] private ValueInput _controlMode;
        [DoNotSerialize] private ValueInput _parameterName;
        [DoNotSerialize] private ValueInput _parameterValue;
        [DoNotSerialize] private ValueInput _isPaused;
        [DoNotSerialize] private ControlOutput _exit;

        protected override void Definition() {
            _audioInstance = ValueInput<ISoundEventInstanceEntity>("AudioInstance");
            _controlMode = ValueInput("ControlMode", ControlType.None);
            _parameterName = ValueInput("ParameterName", "");
            _parameterValue = ValueInput("ParameterValue", 0f);
            _isPaused = ValueInput("IsPaused", true);
            ControlInput("In", OnEnter);
            _exit = ControlOutput("Out");
        }
        
        private ControlOutput OnEnter(Flow flow) {
            ISoundEventInstanceEntity audioInstance = flow.GetValue<ISoundEventInstanceEntity>(_audioInstance);
            ControlType controlMode = flow.GetValue<ControlType>(_controlMode);
            string parameterName = flow.GetValue<string>(_parameterName);
            float parameterValue = flow.GetValue<float>(_parameterValue);
            bool isPaused = flow.GetValue<bool>(_isPaused);
                
            if (audioInstance == null || !audioInstance.IsValid()) {
                Debug.LogWarning("Audio instance is null or invalid!");
                return _exit;
            }
                
            switch (controlMode) {
                case ControlType.None:
                    break;
                case ControlType.Pause:
                    audioInstance.SetPaused(isPaused);
                    break;
                case ControlType.Resume:
                    audioInstance.SetPaused(false);
                    break;
                case ControlType.Restart:
                    audioInstance.RestartSound();
                    break;
                case ControlType.SetParameter:
                    if (!string.IsNullOrEmpty(parameterName)) {
                        audioInstance.SetParameterValue(parameterName, parameterValue);
                    }
                    break;
                case ControlType.SetTimelinePosition:
                    audioInstance.SetTimelinePosition((int)parameterValue);
                    break;
            }
            return _exit;
        }
    }
} 