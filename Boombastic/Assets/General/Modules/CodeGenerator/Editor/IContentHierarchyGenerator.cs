using System.Collections.Generic;

namespace CodeGenerator {
    public interface IContentHierarchyGenerator : IContentGenerator {
        public List<IContentGenerator> Children { get; }
    }
}