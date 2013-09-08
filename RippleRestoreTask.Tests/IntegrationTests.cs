using FubuCore.Logging;
using NUnit.Framework;
using RippleRestoreTask;

[TestFixture]
public class IntegrationTests
{
    [Test,Explicit]
    public void Foo()
    {
        RestoreTask.InnerExecute(new DebugListener(Level.All), @"C:\Code\Particular\ServiceControl\");
    }
}