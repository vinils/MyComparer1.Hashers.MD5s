namespace MyComparer.Hashers.MD5s
{
    using MyComparer.Hashers.DAL;
    using MyList;
    using System;
    using System.Collections.Generic;
    using HashersDAL = Hashers.DAL;
    using MyListDAL = MyList.DAL;

    public class Linear : DAL.FileHashes.Files.Directory
    {
        public static Linear NewInMemory()
        {
            var directoriesMemory = new MyListDAL.Memories.Directory();
            var filesMemory = new MyListDAL.Memories.File();
            var fileHashesMemory = new HashersDAL.Memories.FileHash();

            return new Linear(directoriesMemory, filesMemory, fileHashesMemory);
        }

        public static Linear NewInCloud()
        {
            var directoriesMemory = new MyListDAL.Clouds.Directory();
            var filesMemory = new MyListDAL.Clouds.File();
            var fileHashesMemory = new HashersDAL.Clouds.FileHash();

            return new Linear(directoriesMemory, filesMemory, fileHashesMemory);
        }

        private readonly MyListDAL.IDirectory _directories;

        public override MyListDAL.IDirectory Directories => _directories;

        private readonly MyListDAL.IFile _files;

        public override MyListDAL.IFile Files => _files;

        private readonly HashersDAL.IFileHash _fileHashes;

        public override HashersDAL.IFileHash FileHashes => _fileHashes;

        protected Linear(MyListDAL.IDirectory directories, MyListDAL.IFile files, HashersDAL.IFileHash fileHashes)
        {
            _directories = directories;
            _files = files;
            _fileHashes = fileHashes;
        }

        public override bool Equals(MyListDAL.Entities.File file1, MyListDAL.Entities.File file2)
        {
            var files = Cast(file1, file2);

            return files[0] == files[1];
        }

        protected override void Load(IEnumerable<MyListDAL.Entities.File> files)
            => files.List(FileHashes.Add, this);

        protected override void Load(MyListDAL.IDirectory directories)
        {
            directories.ListFiles(Files.Add);
            Load(Files);
        }

        protected override void Load(MyListDAL.Entities.Directory directory)
        {
            directory.ListAll(Directories.Add);
            Load(Directories);
        }

        protected override MyListDAL.IDirectory ExceptDirectories(IEnumerable<MyListDAL.Entities.Directory> directories)
        {
            //var filesDirectories = _files.GetDirectories();
            //var directoriesEnumerable = directories.Except(filesDirectories);
            //return new MyList.DAL.Memories.Directory(directoriesEnumerable);
            throw new NotImplementedException("reload not implmented in this class yet");
        }

        protected override MyListDAL.IFile ExceptFiles(IEnumerable<MyListDAL.Entities.File> files)
        {
            //var fileHashesFiles = _fileHashes.GetFiles();
            //var fileHashesFilesEnumerable = files.Except(fileHashesFiles);
            //return new MyList.DAL.Memories.File(fileHashesFilesEnumerable);
            throw new NotImplementedException("reload not implmented in this class yet");
        }
    }
}
