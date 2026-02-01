using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MirralLogger.Runtime.Util
{
    public static class LogFileManager
    {
        //日志名
        public static readonly string fileName = $"MLogger_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.log";

        //日志完整路径
        public static string path { private set; get; }

        //日志目录
        public static string dir { private set; get; }

        public static int MaxLogCount = 10;

        static LogFileManager()
        {
            path = Path.Combine(Application.persistentDataPath, "MLogger", fileName);
            dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public static void CleanUpLast()
        {
            var files = Directory.GetFiles(dir, "MLogger_*.log", SearchOption.AllDirectories);

            //小于需要清理的数量
            if (files.Length <= MaxLogCount) return;
            files = files.OrderByDescending(File.GetLastWriteTime).ToArray();

            var length = files.Length;
            
            while (length >= MaxLogCount)
            {
                File.Delete(files[length - 1]);
                length--;
            }
        }
    }
}