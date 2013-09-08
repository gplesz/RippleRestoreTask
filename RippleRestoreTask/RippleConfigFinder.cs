using System.IO;

public class RippleConfigFinder
{

    public static string TreeWalkForRippleConfig(string currentDirectory)
    {
        while (true)
        {
            var possibleRippleConfigPath = Path.Combine(currentDirectory, "ripple.config");
            if (File.Exists(possibleRippleConfigPath))
            {
                return possibleRippleConfigPath;
            }
            var parent = Directory.GetParent(currentDirectory);
            if (parent == null)
            {
                break;
            }
            currentDirectory = parent.FullName;
        }
        return null;
    }
}