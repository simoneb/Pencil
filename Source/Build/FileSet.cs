using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build
{
	using System;
	using System.Collections.Generic;

    public class FileSet
	{
	    readonly IFileSystem fileSystem;
		readonly List<Path> items = new List<Path>();

        public FileSet(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public FileSet(): this(LocalFileSystem.Instance){}

        public IEnumerable<Path> Items
        {
            get
            {
                foreach(var item in items)
                    if(item.ToString().Contains("*"))
                    {
                        var files = fileSystem.GetFile(item.FullPath).Parent.Files(fileSystem.GetFile(item.FullPath).Name);
                        foreach(var file in files)
                            yield return file.Path;
                    }
                    else
                        yield return item;
            }
        }

		public FileSet Add(Path path)
		{
			items.Add(path);
			return this;
		}

		public void CopyTo(Path destination)
		{
            //fileSystem.EnsureDirectory(destination);
            //foreach(var file in Items)
            //{
            //    var target = destination + file.GetFileName();
            //    if(fileSystem.FileExists(target) || !fileSystem.FileExists(file))
            //        return;
            //    fileSystem.CopyFile(file, target, true);
            //}
		}

		public bool ChangedAfter(DateTime timestamp)
		{
            //foreach(var item in Items)
            //    if(fileSystem.GetLastWriteTime(item) > timestamp)
            //        return true;
		    return false;
		}

		public bool ChangedAfter(Path path)
		{
            //return ChangedAfter(fileSystem.GetLastWriteTime(path));
		    return false;
		}

		public IEnumerator<Path> GetEnumerator(){ return items.GetEnumerator(); }
	}
}
