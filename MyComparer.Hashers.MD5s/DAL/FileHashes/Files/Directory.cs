namespace MyComparer.Hashers.MD5s.DAL.FileHashes.Files
{
    using System;
    using System.Collections.Generic;
    using MyListDAL = MyList.DAL;
    using HashersDAL = Hashers.DAL;

    public abstract class Directory : File
    {
        public abstract MyListDAL.IDirectory Directories { get; }

        protected abstract void Load(MyListDAL.Entities.Directory directory);

        protected abstract MyListDAL.IDirectory ExceptDirectories(IEnumerable<MyListDAL.Entities.Directory> directories);

        public void ReloadDirectories()
        {
            var newDirectories = ExceptDirectories(Directories);

            Load(newDirectories);

            //testar
            //ReloadFiles();
        }

        public HashersDAL.IFileHash Duplicates(MyListDAL.Entities.Directory directory)
        {
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            Load(directory);

            return FileHashes.Duplicates();
        }

        public HashersDAL.IFileHash Distincts(MyListDAL.Entities.Directory directory)
        {
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            Load(directory);

            return FileHashes.Duplicates();
        }
    }
}
