namespace MyComparer.Hashers.MD5s
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class Md5 : IHasher
    {
        private readonly MD5 _instance = MD5.Create();
        public Func<Stream, byte[]> ComputeHash => _instance.ComputeHash;

        private static void Instantiate(Action<IHasher> action)
        {
            using (var md5Instance = new Md5())
            {
                action(md5Instance);
            }
        }

        private static T Instantiate<T>(Func<IHasher, T> action)
        {
            using (var md5Instance = new Md5())
            {
                return action(md5Instance);
            }
        }

        private static void HasherInstance(Action<Func<Stream, byte[]>> action)
        {
            void computeHash(IHasher hasherInstance) => action(hasherInstance.ComputeHash);

            Instantiate(computeHash);
        }

        private static T HasherInstance<T>(Func<Func<Stream, byte[]>, T> function)
        {
            T computeHashFunction(IHasher hasherInstance) => function(hasherInstance.ComputeHash);

            return Instantiate(computeHashFunction);
        }

        public Md5()
        { }

        public virtual void Dispose()
            => _instance.Dispose();
    }
}
