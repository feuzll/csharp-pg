using System;
using System.IO;

class Program
{
    static string oldestFilePath = string.Empty;
    static DateTime oldestFileTime = DateTime.Now;

    static void Main(string[] args)
    {
        // Start with the root directory of the disk you want to search
        string rootDirectory = @"T:\";

        FindOldestFile(rootDirectory);

        if (!string.IsNullOrEmpty(oldestFilePath))
        {
            Console.WriteLine($"The oldest file is: {oldestFilePath}");
            Console.WriteLine($"Creation time: {oldestFileTime}");
        }
        else
        {
            Console.WriteLine("No files were found.");
        }

        Console.ReadKey();
    }

    static void FindOldestFile(string directory)
    {
        try
        {
            // Get all files in the current directory
            foreach (var filePath in Directory.GetFiles(directory))
            {
                try
                {
                    var creationTime = File.GetCreationTime(filePath);
                    if (creationTime < oldestFileTime)
                    {
                        oldestFileTime = creationTime;
                        oldestFilePath = filePath;
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors (e.g., file access issues) if necessary
                    Console.WriteLine($"Error accessing file {filePath}: {ex.Message}");
                }
            }

            // Recursively search subdirectories
            foreach (var subDir in Directory.GetDirectories(directory))
            {
                FindOldestFile(subDir);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            // Handle access errors (e.g., permission issues) if necessary
            Console.WriteLine($"Access denied to directory {directory}: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Handle other errors if necessary
            Console.WriteLine($"Error processing directory {directory}: {ex.Message}");
        }
    }
}