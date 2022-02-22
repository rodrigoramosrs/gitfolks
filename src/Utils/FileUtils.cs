using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GitFolks.Utils
{
    public static class FileUtils
    {
        public static FileInfo GetLastFileFromFolder(string Path)
        {
            var directory = new DirectoryInfo(Path);
            return directory.GetFiles("*.*", SearchOption.AllDirectories)
                     .Where(x => !x.FullName.EndsWith("diff"))   
                     .OrderByDescending(f => f.LastWriteTime)
                     ?.FirstOrDefault();
        }
    }
}
