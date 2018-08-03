namespace MyComparer.Hashers.MD5s
{
    using System.Collections.Generic;
    using MyListDAL = MyList.DAL;

    public static class MyExtension
    {
        public static IEnumerable<MyListDAL.Entities.File> DuplicateFiles(
            this MyListDAL.Entities.Directory directory, 
            DAL.FileHashes.Files.Directory comparer)
            => comparer.Duplicates(directory);
    }
}
