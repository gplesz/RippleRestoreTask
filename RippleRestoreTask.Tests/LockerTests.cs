using System.Threading;
using NUnit.Framework;

[TestFixture]
public class LockerTests
{
    [Test]
    public void Ensure_only_one_action_is_run()
    {
        var secondIsRun = false;
        var firstIsRun = false;
        var thread1 = new Thread(() => Locker.ExecuteLocked("foo", () =>
            {
                Thread.Sleep(1000);
                firstIsRun = true;
            }));
        var thread2 = new Thread(() => Locker.ExecuteLocked("foo", () =>
            {
                Thread.Sleep(1000);
                secondIsRun = true;
            }));
        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();
        Assert.IsFalse(firstIsRun && secondIsRun);
    }
}