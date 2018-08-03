namespace MyComparer.Hashers.MD5s
{
    using MyComparer.Hashers.DAL;
    using MyList;
    using MyList.DAL.Clouds;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using HashersDAL = Hashers.DAL;
    using MyListDAL = MyList.DAL;

    public class Cloud : DAL.FileHashes.Files.Directory
    {
        private readonly MyListDAL.Memories.Directory _directories;

        public override MyListDAL.IDirectory Directories => _directories;

        private readonly MyListDAL.Memories.File _files;

        public override MyListDAL.IFile Files => _files;

        private readonly HashersDAL.Clouds.FileHash _fileHashes;

        public override IFileHash FileHashes => _fileHashes;

        public Cloud(Guid directoryId, Guid filesId, Guid fileHashesId)
        {
            _directories = new MyListDAL.Clouds.Directory(directoryId);
            _files = new MyListDAL.Clouds.File(filesId);
            _fileHashes = new HashersDAL.Clouds.FileHash(filesId);
        }

        public Cloud(Guid processId)
           :this(processId, processId, processId)
        { }

        public override bool Equals(MyListDAL.Entities.File file1, MyListDAL.Entities.File file2)
        {
            var files = CastParallel(file1, file2);

            return files[0] == files[1];
        }

        protected override void Load(IEnumerable<MyListDAL.Entities.File> files)
            => files.List(FileHashes.Add, this);

        protected override void Load(MyListDAL.IDirectory directories)
        {
            var exceptions = new ConcurrentBag<Exception>();

            _files.Add(directories.ListFilesParallel, exceptions.Add, out MyListDAL.Memories.File newFiles);
            _files.SaveOnCloud(newFiles, exceptions.Add);
            _fileHashes.Add(newFiles, this, exceptions.Add);

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }

        protected override MyListDAL.IDirectory ExceptDirectories(IEnumerable<MyListDAL.Entities.Directory> directories)
        {
            var filesDirectories = _files.GetDirectories();
            var directoriesEnumerable = directories.Except(filesDirectories);
            return new MyList.DAL.Memories.Directory(directoriesEnumerable);
        }

        protected override MyListDAL.IFile ExceptFiles(IEnumerable<MyListDAL.Entities.File> files)
        {
            var fileHashesFiles = _fileHashes.GetFiles();
            var fileHashesFilesEnumerable = files.Except(fileHashesFiles);
            return new MyListDAL.Memories.File(fileHashesFilesEnumerable);
        }

        protected override void Load(MyListDAL.Entities.Directory directory)
        {
            var exceptions = new ConcurrentBag<Exception>();

            _directories.Add(directory.ListAll, exceptions.Add, out MyListDAL.Memories.Directory newDirectories);
            _directories.SaveOnCloud(newDirectories, exceptions.Add);
            _files.Add(_directories.ListFilesParallel, exceptions.Add, out MyListDAL.Memories.File newFiles);
            _files.SaveOnCloud(newFiles, exceptions.Add);
            _fileHashes.Add(newFiles, this, exceptions.Add);

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }

        public override void Dispose()
        {
            _fileHashes.Dispose();
            base.Dispose();
        }
    }
}
