using System.Linq;
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
            fileSystem.GetDirectory(destination.FullPath).MustExist();

            foreach (var sourcePath in Items)
            {
                var source = fileSystem.GetFile(sourcePath.FullPath);
                var target = fileSystem.GetDirectory(destination.FullPath).GetFile(source.Name);

                if (target.Exists || !source.Exists)
                    return;

                source.CopyTo(target);
            }
		}

		public bool ChangedAfter(DateTime? timestamp)
		{
            if (!timestamp.HasValue)
                return true;

		    return Items.Any(item => fileSystem.GetFile(item.FullPath).LastModifiedTimeUtc > timestamp);
		}

		public bool ChangedAfter(Path path)
		{
            return ChangedAfter(fileSystem.GetFile(path.FullPath).LastModifiedTimeUtc);
		}

		public IEnumerator<Path> GetEnumerator(){ return items.GetEnumerator(); }
	}
}
