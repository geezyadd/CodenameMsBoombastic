using System.IO;
using System.Text;
using UnityEditor;

namespace CodeGenerator {
    public static class FileWriter {
        public static void WriteContent(string path, string content) {
            string directory = Path.GetDirectoryName(path)!;
            if (Directory.Exists(directory) is false)
                Directory.CreateDirectory(directory);

            File.WriteAllText(path, content, Encoding.UTF8);

            AssetDatabase.Refresh();
        }
    }
}