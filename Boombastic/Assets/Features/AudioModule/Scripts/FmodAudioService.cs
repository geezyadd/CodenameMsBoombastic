using System;
using System.Collections.Generic;
using FMODUnity;
using RSG.Muffin.AudioServiceModule.Core;
using UnityEngine;

namespace Features.AudioModule.Scripts {
    public class FmodAudioService : IFmodAudioService {
        private readonly Dictionary<EventReference, ISoundEventInstanceEntity> _fmodEventInstanceEntities = new();
        private readonly ICoroutineRunner _coroutineRunner;
        public event Action<EventReference> OnPlaySound;

        public FmodAudioService(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void PlayAudioOneShot(EventReference audioType) {
            if(audioType.IsNull)
                return;
            
            RuntimeManager.PlayOneShot(audioType);
            OnPlaySound?.Invoke(audioType);
        }

        public void PlayAudioOneShot(EventReference eventReference, Vector3 position) {
           if(eventReference.IsNull)
               return;

           RuntimeManager.PlayOneShot(eventReference, position);
           OnPlaySound?.Invoke(eventReference);
        }
        
        public ISoundEventInstanceEntity GetFmodEventInstanceEntity(EventReference audioID) {
            if (audioID.IsNull)
                return new SoundEventInstanceEntity();
            
            RemoveFmodEventInstanceEntity(audioID);
            ISoundEventInstanceEntity soundEventInstanceEntity = new SoundEventInstanceEntity(audioID, _coroutineRunner);
            _fmodEventInstanceEntities.Add(audioID, soundEventInstanceEntity);
            return soundEventInstanceEntity;
        }
        
        public void RemoveFmodEventInstanceEntity(EventReference audioID) {
            if(_fmodEventInstanceEntities.ContainsKey(audioID) is false)
                return;
            
            _fmodEventInstanceEntities[audioID].ReleaseEventInstance();
            _fmodEventInstanceEntities.Remove(audioID);
        }
    }
}