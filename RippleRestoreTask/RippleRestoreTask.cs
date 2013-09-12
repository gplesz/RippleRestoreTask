using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FubuCore.Logging;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using ripple;
using ripple.Commands;
using ripple.Model;

namespace RippleRestoreTask
{
    public class RestoreTask : Task, ICancelableTask
    {
        [Required]
        public string SolutionDir { get; set; }

        public override bool Execute()
        {
            if (SolutionTracker.HasProcessed(SolutionDir))
            {
                Log.LogMessage(string.Format("Already processed '{0}' skipping package restore.", SolutionDir));
                return true;
            }
            var logListener = new BuildListener(Log);
            var rippleConfigPath = RippleConfigFinder.TreeWalkForRippleConfig(SolutionDir);
            if (rippleConfigPath == null)
            {
                Log.LogError("Could not find 'ripple.config' in the directory hierarchy.");
                return false;
            }
            Log.LogMessage(string.Format("Found 'ripple.config' in at '{0}'.", rippleConfigPath));
            try
            {
                Locker.ExecuteLocked(SolutionDir, () => InnerExecute(logListener, Path.GetDirectoryName(rippleConfigPath)));
                return !logListener.ErrorOccurred;
            }
            catch (Exception exception)
            {
                logListener.Error("Error occurred: ", exception);
                return false;
            }
            finally
            {
                RippleLog.Reset();
                SolutionFiles.CheckForClassic = true;
            }
        }

        public void Cancel()
        {
            //TODO: cancel restore
        }

        public static void InnerExecute(ILogListener logListener, string targetDirectory)
        {
            SolutionFiles.CheckForClassic = false;

            var restoreCommand = new RestoreCommand();
            var solution = SolutionBuilder.ReadFrom(targetDirectory);
            if (RestoreAlreadyDone(solution))
            {
                return;
            }

            var expression = RippleOperation
                .With(solution);

            RippleLog.RemoveFileListener();
            RippleLog.RegisterListener(logListener);
            expression.Execute(new RestoreInput(), restoreCommand);
        }

        static bool RestoreAlreadyDone(Solution solution)
        {
            var packagesDirectory = solution.PackagesDirectory();
            return Directory.EnumerateDirectories(packagesDirectory)
                .Count(x => !Path.GetFileName(x).StartsWith("RippleRestoreTask")) > 0;
        }
    }
}