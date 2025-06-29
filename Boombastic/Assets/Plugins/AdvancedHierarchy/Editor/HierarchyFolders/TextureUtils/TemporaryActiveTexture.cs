using System;
using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders.TextureUtils {
    public class TemporaryActiveTexture : IDisposable {
        public static implicit operator RenderTexture(TemporaryActiveTexture temporaryTexture) =>
            temporaryTexture._value;

        private readonly RenderTexture _previousActiveTexture;
        private readonly TemporaryRenderTexture _value;
            
        public TemporaryActiveTexture(int width, int height, int depthBuffer) {
            _previousActiveTexture = RenderTexture.active;
            _value = new TemporaryRenderTexture(width, height, depthBuffer);
            RenderTexture.active = _value;
        }

        public void Dispose() {
            _value.Dispose();
            RenderTexture.active = _previousActiveTexture;
        }
    }
}