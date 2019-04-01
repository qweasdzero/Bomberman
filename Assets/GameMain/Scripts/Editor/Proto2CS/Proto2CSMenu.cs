using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameFramework;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;
using UnityGameFramework.Editor.DataTableTools;
using Debug = UnityEngine.Debug;

namespace SG1.Editor.Proto2CS
{
    public sealed class Proto2CSMenu
    {
        public static string DataPath = Application.dataPath.Replace('/', '\\');

        public static string ProtoGenEXEPath = Path.Combine(Path.GetDirectoryName(DataPath), @"ProtoGen").Replace('/', '\\');

        public static string ProtoGenFileFullName = Path.Combine(ProtoGenEXEPath, @"protogen.exe").Replace('/', '\\');
        
        public static string ProtoFilesPath = Path.Combine(ProtoGenEXEPath, @"Protos").Replace('/', '\\');

        public static string outPath =
            Path.Combine(DataPath, @"GameMain/Scripts/Network/Packet/Protobuf").Replace('/', '\\');
        
        [MenuItem("Game Framework/Open Folder/Proto Folder", false, 10)]
        public static void OpenFolderDataPath()
        {
            OpenFolder.Execute(ProtoFilesPath);
        }
        
        [MenuItem(Constant.AssemblyInfo.Namespace + "/Proto2CS")]
        private static void Proto2CS()
        {
            try
            {
                using (Process progress = new Process())
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(ProtoFilesPath);
                    var protoFilesArray = directoryInfo.GetFiles(@"*.proto");

                    foreach (var protoFile in protoFilesArray)
                    {
                        string arguments =
                            string.Format("-i:{0} -o:{1}", protoFile.FullName, Path.Combine(outPath,
                                string.Format("{0}.cs", Path.GetFileNameWithoutExtension(protoFile.Name))));
                        ProcessStartInfo info = new ProcessStartInfo(ProtoGenFileFullName,arguments)
                        {
                            UseShellExecute = false,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = false,
                            WorkingDirectory = ProtoFilesPath,
                        };
                        progress.StartInfo = info;
                        progress.Start();
                        progress.WaitForExit();
                        Debug.Log(progress.StandardOutput.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}
