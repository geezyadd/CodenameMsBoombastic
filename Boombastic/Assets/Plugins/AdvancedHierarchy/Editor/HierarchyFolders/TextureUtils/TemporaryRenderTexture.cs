using System;
using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders.TextureUtils {
    public class TemporaryRenderTexture : IDisposable {
        public static implicit operator RenderTexture(TemporaryRenderTexture temporaryRenderTexture) =>
            temporaryRenderTexture._value;

        private readonly RenderTexture _value;

        public TemporaryRenderTexture(int width, int height, int depthBuffer) =>
            _value = RenderTexture.GetTemporary(width, height, depthBuffer);

        public void Dispose() {
            RenderTexture.ReleaseTemporary(_value);
        }
    }
}