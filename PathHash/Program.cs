using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace PathHash
{
    class Program
    {
        static (string, string) PathHash(string path)
        {
            var absPath = Path.GetFullPath(path);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                absPath = absPath.ToLower();
            }
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(absPath));
            return (Convert.ToHexString(hash), absPath);
        }

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                return 1;
            }
            var (hash, absPath) = PathHash(args[0]);
            Console.WriteLine(absPath);
            Console.WriteLine(hash);
            return 0;
        }
    }
}
