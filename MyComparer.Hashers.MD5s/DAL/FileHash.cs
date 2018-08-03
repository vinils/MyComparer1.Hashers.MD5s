namespace MyComparer.Hashers.MD5s.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HashersDAL = Hashers.DAL;
    using MyListDAL = MyList.DAL;

    public abstract class FileHash : Md5, IMyEqualityComparer<MyListDAL.Entities.File>
    {
        public abstract HashersDAL.IFileHash FileHashes { get; }

        protected HashersDAL.Entities.FileHash Cast(MyListDAL.Entities.File file)
            => HashersDAL.Entities.FileHash.Cast(file, this);

        protected HashersDAL.Entities.FileHash[] Cast(params MyListDAL.Entities.File[] files)
        {
            var ret = new HashersDAL.Entities.FileHash[files.Length];

            for (var i = 0; i < files.Length; i++)
                ret[i] = HashersDAL.Entities.FileHash.Cast(files[i], this);

            return ret;
        }

        protected HashersDAL.Entities.FileHash[] CastParallel(params MyListDAL.Entities.File[] files)
        {
            var ret = new HashersDAL.Entities.FileHash[files.Length];

            Parallel.For(0, files.Length, i => ret[i] = HashersDAL.Entities.FileHash.Cast(files[i], this));

            return ret;
        }

        protected abstract void Load(IEnumerable<MyListDAL.Entities.File> files);

        public HashersDAL.IFileHash Distincts(IEnumerable<MyListDAL.Entities.File> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            Load(files);

            return FileHashes.Distincts();
        }

        public HashersDAL.IFileHash Duplicates(IEnumerable<MyListDAL.Entities.File> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            Load(files);

            return FileHashes.Duplicates();
        }

        public HashersDAL.IFileHash Equals(IEnumerable<MyListDAL.Entities.File> files, MyListDAL.Entities.File file)
        {
            Load(files);

            return FileHashes.Equals(file, this);
        }

        public HashersDAL.IFileHash Equals(MyListDAL.Entities.File file, MyListDAL.IFile files)
        {
            Load(files);

            return FileHashes.Equals(file, this);
        }

        public int GetHashCode(MyListDAL.Entities.File obj)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(MyListDAL.Entities.File file1, MyListDAL.Entities.File file2);

        public HashersDAL.IFileHash Equals(MyListDAL.Entities.File file, IEnumerable<MyListDAL.Entities.File> files)
            => Equals(files, file);

        IEnumerable<MyListDAL.Entities.File> IMyEqualityComparer<MyListDAL.Entities.File>.Equals(
            IEnumerable<MyListDAL.Entities.File> files, 
            MyListDAL.Entities.File file)
            => Equals(files, file);

        IEnumerable<MyListDAL.Entities.File> IMyEqualityComparer<MyListDAL.Entities.File>.Equals(
            MyListDAL.Entities.File file, 
            IEnumerable<MyListDAL.Entities.File> files)
            => Equals(files, file);

        IEnumerable<MyListDAL.Entities.File> IMyEqualityComparer<MyListDAL.Entities.File>.Duplicates(
            IEnumerable<MyListDAL.Entities.File> files)
            => Duplicates(files);

        IEnumerable<MyListDAL.Entities.File> IMyEqualityComparer<MyListDAL.Entities.File>.Distincts(
            IEnumerable<MyListDAL.Entities.File> files)
            => Distincts(files);
    }
}