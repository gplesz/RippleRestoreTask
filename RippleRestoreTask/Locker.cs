using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

public static class Locker
{
    public static void ExecuteLocked(string name, Action action)
    {
        bool created;
        name = SanitizeName(name);
        using (var mutex = new Mutex(true, name, out created))
        {
            try
            {
                // We need to ensure only one instance of the executable performs the install. All other instances need to wait 
                // for the package to be installed. We'd cap the waiting duration so that other instances aren't waiting indefinitely.
                if (created)
                {
                    action();
                }
                else
                {
                    mutex.WaitOne(TimeSpan.FromMinutes(4));
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }

    static string SanitizeName(string name)
    {
        var bytes = Encoding.UTF8.GetBytes(name);
        using (var sha256Managed = new SHA256Managed())
        {
            return Convert.ToBase64String(sha256Managed.ComputeHash(bytes)).ToUpperInvariant();
        }
    }
}