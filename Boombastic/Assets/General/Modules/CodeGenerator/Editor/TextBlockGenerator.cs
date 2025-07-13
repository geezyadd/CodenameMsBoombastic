namespace CodeGenerator {
    public class TextBlockGenerator : IContentGenerator {
        private readonly string _content;

        public TextBlockGenerator(string content) =>
            _content = content;

        public string GenerateContent() =>
            _content;
    }
}