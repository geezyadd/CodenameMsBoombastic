using System;
using FMODUnity;
using RSG.Muffin.AudioServiceModule.Core;
using UnityEngine;

namespace Features.AudioModule.Scripts {
    public interface IFmodAudioService {
        public event Action<EventReference> OnPlaySound;
        public void PlayAudioOneShot(EventReference eventReference);
        public void PlayAudioOneShot(EventReference eventReference, Vector3 position);
        public ISoundEventInstanceEntity GetFmodEventInstanceEntity(EventReference eventReference);
        public void RemoveFmodEventInstanceEntity(EventReference eventReference);
    }
}