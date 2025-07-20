using System.IO;
using CodeGenerator;
using UIWorkflow.Core;

namespace UIWorkflow.Editor {
    public static class WindowClassTemplateGenerator {
        private static string _className;
        private static string _classNamespace;
        private static string _path;

        public static void SetClassName(string className) =>
            _className = className;

        public static void SetNamespace(string classNamespace) =>
            _classNamespace = classNamespace;

        public static void SetPath(string path) =>
            _path = path;

        public static void Generate() {
            FileGenerator fileGenerator = new() {
                Children = {
                    new NamespaceGenerator(_classNamespace) {
                        Children = {
                            new ClassGenerator {
                                Name = _className,
                                Modifiers = {
                                    ModifierKeyword.Public
                                },
                                Extends = {
                                    typeof(WindowBehaviour)
                                },
                                Children = {
                                    new TextBlockGenerator("public " + _className + "(" + typeof(IWindowFactory).FullName + " windowFactory, " + typeof(IPresenterFactory).FullName + " presenterFactory) : base(windowFactory, presenterFactory) { }")
                                }
                            }
                        }
                    }
                }
            };
            
            FileWriter.WriteContent(Path.Combine(_path, _className) + ".cs", fileGenerator.GenerateContent());
        }
    }
}