namespace MyComparer.Hashers.MD5s
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using MyListDAL = MyList.DAL;
    using HashersDAL = Hashers.DAL;
    using MyComparer.Hashers.DAL;

    public class MemoryParallel : DAL.FileHashes.Files.Directory
    {
        private readonly MyListDAL.Memories.Directory _directories = new MyListDAL.Memories.Directory();

        public override MyListDAL.IDirectory Directories => _directories;

        private readonly MyListDAL.Memories.File _files = new MyListDAL.Memories.File();

        public override MyListDAL.IFile Files => _files;

        private readonly HashersDAL.Memories.FileHash _fileHashes = new HashersDAL.Memories.FileHash();

        public override HashersDAL.IFileHash FileHashes => _fileHashes;

        public override bool Equals(MyListDAL.Entities.File file1, MyListDAL.Entities.File file2)
        {
            var files = CastParallel(file1, file2);

            return files[0] == files[1];
        }

        protected override void Load(IEnumerable<MyListDAL.Entities.File> files)
            => files.ListParallelAggregatingExceptions(_fileHashes.Add, this);

        protected override void Load(MyListDAL.IDirectory directories)
        {
            var exceptions = new ConcurrentBag<Exception>();

            _files.Add(Directories.ListFilesParallel, exceptions.Add, out MyListDAL.Memories.File newFiles);
            _fileHashes.Add(Files.ListParallel, this, exceptions.Add, out HashersDAL.Memories.FileHash newFileHashes);

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }

        protected override void Load(MyListDAL.Entities.Directory directory)
        {
            var exceptions = new ConcurrentBag<Exception>();

            _directories.Add(directory.ListAll, exceptions.Add, out MyListDAL.Memories.Directory newDirectories);
            _files.Add(Directories.ListFilesParallel, exceptions.Add, out MyListDAL.Memories.File newFiles);
            _fileHashes.Add(Files.ListParallel, this, exceptions.Add, out HashersDAL.Memories.FileHash newFileHashes);

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }

        protected override MyListDAL.IDirectory ExceptDirectories(IEnumerable<MyListDAL.Entities.Directory> directories)
        {
            var filesDirectories = _files.GetDirectories();
            var directoriesEnumerable = directories.Except(filesDirectories);
            return new MyListDAL.Memories.Directory(directoriesEnumerable);
        }

        protected override MyListDAL.IFile ExceptFiles(IEnumerable<MyListDAL.Entities.File> files)
        {
            var fileHashesFiles = _fileHashes.GetFiles();
            var fileHashesFilesEnumerable = files.Except(fileHashesFiles);
            return new MyListDAL.Memories.File(fileHashesFilesEnumerable);
        }
    }
}
