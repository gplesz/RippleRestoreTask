using System;
using FubuCore.Descriptions;
using FubuCore.Logging;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

public class BuildListener : ILogListener
{
    TaskLoggingHelper taskLoggingHelper;

    public BuildListener(TaskLoggingHelper taskLoggingHelper)
    {
        this.taskLoggingHelper = taskLoggingHelper;
    }

    public bool ListensFor(Type type)
    {
        return true;
    }

    public void DebugMessage(object message)
    {
        taskLoggingHelper.LogMessage(MessageImportance.Low, message.ToDescriptionText());
    }

    public void InfoMessage(object message)
    {
        taskLoggingHelper.LogMessage(MessageImportance.Normal, message.ToDescriptionText());
    }

    public void Debug(string message)
    {
        taskLoggingHelper.LogMessage(MessageImportance.Low, message);
    }

    public void Info(string message)
    {
        taskLoggingHelper.LogMessage(MessageImportance.Normal, message);
    }

    public void Error(string message, Exception ex)
    {
        ErrorOccurred = true;
        taskLoggingHelper.LogMessage(MessageImportance.Normal, message.ToDescriptionText() + Environment.NewLine + ex.ToFriendlyString());
    }

    public void Error(object correlationId, string message, Exception ex)
    {
        ErrorOccurred = true;
        taskLoggingHelper.LogMessage(MessageImportance.Normal, message.ToDescriptionText() + Environment.NewLine + ex.ToFriendlyString());
    }

    public bool IsDebugEnabled
    {
        get { return true; }
    }

    public bool IsInfoEnabled
    {
        get { return true; }
    }

    public bool ErrorOccurred;
}