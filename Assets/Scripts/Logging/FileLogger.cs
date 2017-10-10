using System.IO;
using UnityEngine;

namespace Logging {
    public class FileLogger {
        public static string FilePath = Application.dataPath + "/Resources/log.txt";

        public static void Log(string format, params object[] args) {
#if UNITY_EDITOR
            Debug.LogFormat(format, args);
            
            using (var writer = File.AppendText(FilePath))
            {
                writer.WriteLine(format, args);
                writer.Close();
            }
#endif
        }

        public static void ClearFile() {
            if (File.Exists(FilePath)) {
                File.WriteAllText(FilePath, "");
            }
        }
    }
}
