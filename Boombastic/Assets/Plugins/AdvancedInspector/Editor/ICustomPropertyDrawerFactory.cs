using System.Reflection;

namespace AdvancedInspector.Editor {
    internal interface ICustomPropertyDrawerFactory {
        public ICustomPropertyDrawer Create(FieldInfo fieldInfo);
    }
}