using UnityEngine;

namespace AdvancedHierarchy.Editor.HierarchyFolders.TextureUtils {
    internal static class TextureHelper {
        private static Material _tintMaterial;
        private static Material _colorReplaceMaterial;

        public static Texture2D GetTintedTexture(Texture2D original, Color tint, string name) =>
            GetTextureWithMaterial(original, GetTintMaterial(tint), name);

        public static Texture2D GetWhiteTexture(Texture2D original, string name) =>
            GetTextureWithMaterial(original, GetColorReplaceMaterial(Color.white), name);

        private static Material GetTintMaterial(Color tint) {
            if (_tintMaterial == null)
                _tintMaterial = new Material(Shader.Find("UI/Default"));

            _tintMaterial.color = tint;
            return _tintMaterial;
        }

        private static Material GetColorReplaceMaterial(Color color) {
            if (_colorReplaceMaterial == null)
                _colorReplaceMaterial = new Material(Shader.Find("Hidden/AdvancedHierarchy/HierarchyFolderColor"));

            _colorReplaceMaterial.color = color;
            return _colorReplaceMaterial;
        }

        private static Texture2D GetTextureWithMaterial(Texture2D original, Material material, string name) {
            Texture2D newTexture;

            using (new SRGBWriteScope(true))
            using (TemporaryActiveTexture temporary = new TemporaryActiveTexture(original.width, original.height, 0)) {
                GL.Clear(false, true, Color.clear);

                Graphics.Blit(original, temporary, material);

                newTexture = new Texture2D(original.width, original.width, TextureFormat.ARGB32, false, true) {
                    name = name, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.DontSave
                };

                newTexture.ReadPixels(new Rect(0f, 0f, original.width, original.width), 0, 0);
                newTexture.alphaIsTransparency = true;
                newTexture.Apply();
            }

            return newTexture;
        }
    }
}