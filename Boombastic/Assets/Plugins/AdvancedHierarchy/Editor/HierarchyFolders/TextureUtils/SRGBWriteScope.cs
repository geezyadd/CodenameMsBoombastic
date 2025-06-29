using System;
using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders.TextureUtils {
    public readonly struct SRGBWriteScope : IDisposable {
        private readonly bool _previousValue;
            
        public SRGBWriteScope(bool enableWrite) {
            _previousValue = GL.sRGBWrite;
            GL.sRGBWrite = enableWrite;
        }

        public void Dispose() {
            GL.sRGBWrite = _previousValue;
        }
    }
}