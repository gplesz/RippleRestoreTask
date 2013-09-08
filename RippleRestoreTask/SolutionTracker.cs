using System.Collections.Generic;

namespace RippleRestoreTask
{
    public static class SolutionTracker
    {
        static List<string> processedSolutionsList = new List<string>();
        public static bool HasProcessed(string solutionDir)
        {
            if (processedSolutionsList.Contains(solutionDir))
            {
                return true;
            }
            lock (processedSolutionsList)
            {
                processedSolutionsList.Add(solutionDir);
            }
            return false;
        }
    }
}