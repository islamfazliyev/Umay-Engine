using System;
using System.Diagnostics;

class buildGame
{
    public static void Main(string destination)
    {
        // string assetsPath = Path.Combine(destination, "assets");
        // string buildPath = Path.Combine(destination, "build");

        // // Create directories
        // Directory.CreateDirectory(assetsPath);
        // Directory.CreateDirectory(buildPath);
        
        CopyFolder(@"C:\Users\islam\Desktop\codes\projects\Umay-Engine\BuildSys\src",
                   $"{destination}");

        // CopyFolder(@"C:\Users\islam\Desktop\codes\projects\Umay-Engine\assets",
        //            assetsPath);
        
        RunNimbleBuild(destination);
    }

    static void CopyFolder(string source, string destination)
    {
        string command = $"xcopy \"{source}\" \"{destination}\" /E /I /Y";
        
        var processInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process())
        {
            process.StartInfo = processInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine($"Copying {source} -> {destination}");
            Console.WriteLine("Output:\n" + output);
            if (!string.IsNullOrEmpty(error))
                Console.WriteLine("Error:\n" + error);
        }
    }

    static void RunNimbleBuild(string destination)
    {
        // Change working directory to the destination
        var processInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c nimble build -r", // Command to run
            WorkingDirectory = destination,  // Set the working directory to the destination
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process())
        {
            process.StartInfo = processInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine($"Running 'nimble build -r' in {destination}");
            Console.WriteLine("Output:\n" + output);
            if (!string.IsNullOrEmpty(error))
                Console.WriteLine("Error:\n" + error);
        }
    }
}
