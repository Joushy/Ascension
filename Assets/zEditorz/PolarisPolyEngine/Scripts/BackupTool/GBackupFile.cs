#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Pinwheel.Griffin.BackupTool
{
    public static class GBackupFile
    {
        public const string DIRECTORY = "GriffinBackup";
        public const string INITIAL_HISTORY_PREFIX = "Begin";
        public const string EXTENSION = ".gbackup";
        public const string HEIGHT_SUFFIX = "height";
        public const string SUBDIV_SUFFIX = "subdiv";
        public const string VISIBILITY_SUFFIX = "visibility";
        public const string ALBEDOR_SUFFIX = "albedor";
        public const string ALBEDOG_SUFFIX = "albedog";
        public const string ALBEDOB_SUFFIX = "albedob";
        public const string ALBEDOA_SUFFIX = "albedoa";
        public const string METALLIC_SUFFIX = "metallic";
        public const string SMOOTHNESS_SUFFIX = "smoothness";
        public const string CONTROLR_SUFFIX = "controlr";
        public const string CONTROLG_SUFFIX = "controlg";
        public const string CONTROLB_SUFFIX = "controlb";
        public const string CONTROLA_SUFFIX = "controla";
        public const string TREE_SUFFIX = "tree";
        public const string GRASS_SUFFIX = "grass";
        public const string PROTOTYPEINDEX_SUFFIX = "protoindex";
        public const string POSITION_SUFFIX = "position";
        public const string ROTATION_SUFFIX = "rotation";
        public const string SCALE_SUFFIX = "scale";

        private static string GetRootDirectory()
        {
            string assetsFolder = Application.dataPath;
            string projectFolder = Directory.GetParent(assetsFolder).FullName;
            return Path.Combine(projectFolder, DIRECTORY);
        }

        public static string GetFileDirectory(string backupName)
        {
            return Path.Combine(GetRootDirectory(), backupName);
        }

        public static string GetFilePath(string backupName, string fileNameNoExt)
        {
            return Path.Combine(GetFileDirectory(backupName), fileNameNoExt + EXTENSION);
        }

        public static bool Exist(string backupName, string fileNameNoExt)
        {
            string filePath = GetFilePath(backupName, fileNameNoExt);
            return File.Exists(filePath);
        }

        public static string Create(string backupName, string fileNameNoExt, byte[] data)
        {
            GUtilities.EnsureDirectoryExists(Path.Combine(GetRootDirectory(), backupName));
            string filePath = GetFilePath(backupName, fileNameNoExt);
            File.WriteAllBytes(filePath, data);
            return filePath;
        }

        public static string[] GetAllFilePaths(string backupName)
        {
            List<string> files = new List<string>(Directory.GetFiles(GetFileDirectory(backupName)));
            files.RemoveAll(f => !f.EndsWith(EXTENSION));
            return files.ToArray();
        }

        public static void SetBackupCreationTime(string backupName, System.DateTime time)
        {
            string folder = GetFileDirectory(backupName);
            if (Directory.Exists(folder))
            {
                Directory.SetCreationTime(folder, time);
            }
        }

        public static System.DateTime GetBackupCreationTime(string backupName)
        {
            string folder = GetFileDirectory(backupName);
            if (Directory.Exists(folder))
            {
                return Directory.GetCreationTime(folder);
            }
            else
            {
                return System.DateTime.MaxValue;
            }
        }

        public static string[] GetAllBackupNames()
        {
            GUtilities.EnsureDirectoryExists(GetRootDirectory());
            List<string> folders = new List<string>(Directory.GetDirectories(GetRootDirectory()));
            for (int i = 0; i < folders.Count; ++i)
            {
                folders[i] = Path.GetFileNameWithoutExtension(folders[i]);
            }
            folders.Sort((b0, b1) =>
            {
                return GBackupFile.GetBackupCreationTime(b0).CompareTo(GBackupFile.GetBackupCreationTime(b1));
            });
            return folders.ToArray();
        }

        public static void Delete(string backupName)
        {
            string folder = GetFileDirectory(backupName);
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }

        public static byte[] ReadAllBytes(string backupName, string fileNameNoExtension)
        {
            if (Exist(backupName, fileNameNoExtension))
            {
                return File.ReadAllBytes(GetFilePath(backupName, fileNameNoExtension));
            }
            else
            {
                return null;
            }
        }

        public static string GetBackupNameByTimeNow()
        {
            System.DateTime d = System.DateTime.Now;
            string s = string.Format("{0}{1}{2}{3}{4}{5}", d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
            return s;
        }

        public static void ClearHistory()
        {
            string[] backupNames = GetAllBackupNames();
            for (int i = 0; i < backupNames.Length; ++i)
            {
                if (backupNames[i].StartsWith("~"))
                {
                    Delete(backupNames[i]);
                }
            }
        }

        public static string GetInitialHistoryPrefix(string prefixWithoutWaveSymbol)
        {
            return string.Format("~{0} {1}", INITIAL_HISTORY_PREFIX, prefixWithoutWaveSymbol);
        }

        public static string GetHistoryPrefix(string prefixWithoutWaveSymbol)
        {
            return string.Format("~{0}", prefixWithoutWaveSymbol);
        }
    }
}
#endif