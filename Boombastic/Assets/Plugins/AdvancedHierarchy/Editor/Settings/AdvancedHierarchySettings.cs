using UnityEditor.SettingsManagement;

namespace AdvancedHierarchy.Editor.Settings {
    internal static class AdvancedHierarchySettings {
        private const string PACKAGE_NAME = "com.heavensforge.advanced-hierarchy";
        private static UnityEditor.SettingsManagement.Settings _instance;
        
        private static UserSetting<int> _maxComponents;
        private static UserSetting<int> _iconsSize;
        private static UserSetting<bool> _showTransforms;
        private static UserSetting<bool> _showTransformsWhenSingle;
        private static UserSetting<bool> _showDefaultScriptIcons;
        private static UserSetting<bool> _openComponentProperties;
        
        private static UserSetting<StrippingMode> _playModeSetting;
        private static UserSetting<StrippingMode> _buildSetting;
        private static UserSetting<bool> _capitalizeName;
        private static UserSetting<bool> _stripFoldersFromPrefabsInPlayMode;
        private static UserSetting<bool> _stripFoldersFromPrefabsInBuild;

        public static StrippingMode PlayMode {
            get {
                if (_instance == null)
                    Initialize();

                return _playModeSetting.value;
            }
            set =>
                _playModeSetting.value = value;
        }

        public static StrippingMode Build {
            get {
                if (_instance == null)
                    Initialize();

                return _buildSetting.value;
            }
            set =>
                _buildSetting.value = value;
        }

        public static bool CapitalizeName {
            get {
                if (_instance == null)
                    Initialize();

                return _capitalizeName.value;
            }
            set =>
                _capitalizeName.value = value;
        }

        public static bool StripFoldersFromPrefabsInPlayMode {
            get {
                if (_instance == null)
                    Initialize();

                return _stripFoldersFromPrefabsInPlayMode.value;
            }
            set =>
                _stripFoldersFromPrefabsInPlayMode.value = value;
        }

        public static bool StripFoldersFromPrefabsInBuild {
            get {
                if (_instance == null)
                    Initialize();
                
                return _stripFoldersFromPrefabsInBuild.value;
            }
            set =>
                _stripFoldersFromPrefabsInBuild.value = value;
        }
        
        public static int MaxComponents {
            get {
                if (_instance == null)
                    Initialize();
                
                return _maxComponents.value;
            }
            set =>
                _maxComponents.value = value;
        }
        
        public static int IconsSize {
            get {
                if (_instance == null)
                    Initialize();
                
                return _iconsSize.value;
            }
            set =>
                _iconsSize.value = value;
        }
        
        public static bool ShowTransforms {
            get {
                if (_instance == null)
                    Initialize();
                
                return _showTransforms.value;
            }
            set =>
                _showTransforms.value = value;
        }
        
        public static bool ShowTransformsWhenSingle {
            get {
                if (_instance == null)
                    Initialize();
                
                return _showTransformsWhenSingle.value;
            }
            set =>
                _showTransformsWhenSingle.value = value;
        }
        
        public static bool ShowDefaultScriptIcons {
            get {
                if (_instance == null)
                    Initialize();
                
                return _showDefaultScriptIcons.value;
            }
            set =>
                _showDefaultScriptIcons.value = value;
        }
        
        public static bool OpenComponentProperties {
            get {
                if (_instance == null)
                    Initialize();
                
                return _openComponentProperties.value;
            }
            set =>
                _openComponentProperties.value = value;
        }

        private static void Initialize() {
            _instance = new UnityEditor.SettingsManagement.Settings(PACKAGE_NAME);
            
            _maxComponents = new UserSetting<int>(_instance, nameof(_maxComponents), 3);
            _iconsSize = new UserSetting<int>(_instance, nameof(_iconsSize), 16);
            _showTransforms = new UserSetting<bool>(_instance, nameof(_showTransforms), true);
            _showTransformsWhenSingle = new UserSetting<bool>(_instance, nameof(_showTransformsWhenSingle), true);
            _showDefaultScriptIcons = new UserSetting<bool>(_instance, nameof(_showDefaultScriptIcons), false);
            _openComponentProperties = new UserSetting<bool>(_instance, nameof(_openComponentProperties), true);
            
            _playModeSetting = new UserSetting<StrippingMode>(_instance, nameof(_playModeSetting), StrippingMode.DoNothing);
            _buildSetting = new UserSetting<StrippingMode>(_instance, nameof(_buildSetting), StrippingMode.Delete);
            _capitalizeName = new UserSetting<bool>(_instance, nameof(_capitalizeName), true);
            _stripFoldersFromPrefabsInPlayMode = new UserSetting<bool>(_instance, nameof(_stripFoldersFromPrefabsInPlayMode), false);
            _stripFoldersFromPrefabsInBuild = new UserSetting<bool>(_instance, nameof(_stripFoldersFromPrefabsInBuild), true);
        }
    }
}