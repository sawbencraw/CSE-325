using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var salesFiles = FindFiles("stores");

        decimal totalSales = 0;
        List<string> details = new List<string>();

        foreach (var file in salesFiles)
        {
            // Extract the store number from the file path
            var storeNumber = Path.GetFileName(Path.GetDirectoryName(file));

            // Get the overall total from the JSON file
            decimal overallTotal = GetOverallTotal(file);

            // Add to the total sales sum
            totalSales += overallTotal;

            // Add the store details to the list
            details.Add($"{storeNumber}: ${overallTotal:N2}");
        }

        // Generate the sales report
        GenerateSalesReport(totalSales, details);
    }

    static IEnumerable<string> FindFiles(string folderName)
    {
        List<string> salesFiles = new List<string>();

        var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

        foreach (var file in foundFiles)
        {
            // Collect only the 'salestotals.json' files
            if (file.EndsWith("salestotals.json"))
            {
                salesFiles.Add(file);
            }
        }

        return salesFiles;
    }

    static decimal GetOverallTotal(string filePath)
    {
        // Read the file content
        var fileContent = File.ReadAllText(filePath);

        // Simple parsing to get the OverallTotal value
        var startIndex = fileContent.IndexOf(":") + 1;
        var endIndex = fileContent.IndexOf("}");
        var totalString = fileContent.Substring(startIndex, endIndex - startIndex).Trim();

        // Parse to decimal
        if (decimal.TryParse(totalString, out decimal overallTotal))
        {
            return overallTotal;
        }
        else
        {
            Console.WriteLine($"Error parsing total in file: {filePath}");
            return 0;
        }
    }

    static void GenerateSalesReport(decimal totalSales, List<string> details)
    {
        Console.WriteLine("Sales Summary");
        Console.WriteLine("----------------------------");
        Console.WriteLine($" Total Sales: ${totalSales:N2}");
        Console.WriteLine("\n Details:");

        foreach (var detail in details)
        {
            Console.WriteLine($"  {detail}");
        }
    }
}