namespace MyComparer.Hashers.MD5s.DAL.FileHashes
{
    using System;
    using System.Collections.Generic;
    using MyListDAL = MyList.DAL;

    public abstract class File : FileHash
    {
        public abstract MyListDAL.IFile Files { get; }

        protected abstract void Load(MyListDAL.IDirectory directories);

        protected abstract MyListDAL.IFile ExceptFiles(IEnumerable<MyListDAL.Entities.File> files);

        public void ReloadFiles()
        {
            var newDirectories = ExceptFiles(Files);

            Load(newDirectories);
        }

        public Hashers.DAL.IFileHash Duplicates(MyListDAL.IDirectory directories)
        {
            if (directories == null)
                throw new ArgumentNullException(nameof(directories));

            Load(directories);

            return FileHashes.Duplicates();
        }

        public Hashers.DAL.IFileHash Distincts(MyListDAL.IDirectory directories)
        {
            if (directories == null)
                throw new ArgumentNullException(nameof(directories));

            Load(directories);

            return FileHashes.Duplicates();
        }
    }
}
