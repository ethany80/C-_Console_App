using System;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;

public class ConsumerComplaint
{
    public DateTime Date { get; set; }
    public string Product { get; set; }
    public List<string> Issues { get; set; }
    public string Company { get; set; }
    public string State { get; set; }
    public ConsumerComplaint() 
    { 
        Issues = new List<string>(); 
    }
}
public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Reading file...\n");

        List<ConsumerComplaint> complaints = new List<ConsumerComplaint>();

        //enter path to consumer_complaints.csv
        TextFieldParser parser = new TextFieldParser(@"path");


        parser.HasFieldsEnclosedInQuotes = true;
        parser.SetDelimiters(",");

        string[] columns;
        string format = "MM/dd/yyyy";

        while (!parser.EndOfData)
        {
            columns = parser.ReadFields();

            ConsumerComplaint complaint = new ConsumerComplaint
            {
                Date = DateTime.ParseExact(columns[0], format, CultureInfo.InvariantCulture),
                Product = columns[1],
                Company = columns[7],
                State = columns[8]
            };

            if (columns[3].Contains(","))
            {
                string[] issues = columns[3].Split(',');
                foreach (string issue in issues)
                {
                    complaint.Issues.Add(issue);
                }
            }
            else
            {
                complaint.Issues.Add(columns[3]);
            }

            complaints.Add(complaint);

        }

        Console.WriteLine("\n\nLoaded " + complaints.Count() + " complaints");

        int userInput = -1;

        while (userInput != 0)
        {
            Console.WriteLine("\nSelect data to view:\n1. Complaints per Year\n2. Complaints per Product\n3. Complaints per Company\n0. Exit\n");
            userInput = int.Parse(Console.ReadLine());

            if (userInput == 1)
            {
                SortedDictionary<int, int> map = new SortedDictionary<int, int>();

                foreach (ConsumerComplaint complaint in complaints)
                {
                   if (!map.ContainsKey(complaint.Date.Year))
                    {
                        map.Add(complaint.Date.Year, 1);
                    }
                   else
                    {
                        map[complaint.Date.Year]++;
                    }
                }

                Console.WriteLine("\nYear\tTotal\n----\t-----");

                foreach (var year in map)
                {
                    Console.WriteLine(year.Key + "\t" + year.Value);
                }
            }
            else if (userInput == 2)
            {
                Dictionary<string, int> map = new Dictionary<string, int>();

                foreach (ConsumerComplaint complaint in complaints)
                {
                    if (!map.ContainsKey(complaint.Product))
                    {
                        map.Add(complaint.Product, 1);
                    }
                    else
                    {
                        map[complaint.Product]++;
                    }
                }

                Console.WriteLine("\nProduct\t\t\t   Total\n-------\t\t\t   -----");

                var sortedMap = from entry in map orderby entry.Value descending select entry;

                foreach (var node in sortedMap)
                {
                    string space = "";
                    for (int i = 0; i < (27 - node.Key.Count()); ++i)
                    {
                        space += " ";
                    }
                    Console.WriteLine(node.Key + space + node.Value);
                }
            }
            else if (userInput == 3)
            {
                Dictionary<string, int> map = new Dictionary<string, int>();

                foreach (ConsumerComplaint complaint in complaints)
                {
                    if (!map.ContainsKey(complaint.Company))
                    {
                        map.Add(complaint.Company, 1);
                    }
                    else
                    {
                        map[complaint.Company]++;
                    }
                }

                Console.WriteLine("Choose number of entries to view in descending order (type -1 to view all):\n");

                int entries = int.Parse(Console.ReadLine());

                Console.WriteLine("\nCompany\t\t\t\t\t   Total\n-------\t\t\t\t\t   -----");

                var sortedMap = from entry in map orderby entry.Value descending select entry;

                if (entries == -1)
                {
                    foreach (var node in sortedMap)
                    {
                        string space = "";
                        for (int i = 0; i < (43 - node.Key.Count()); ++i)
                        {
                            space += " ";
                        }
                        Console.WriteLine(node.Key + space + node.Value);
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var node in sortedMap)
                    {
                        if (count++ == entries) break;

                        string space = "";
                        for (int i = 0; i < (43 - node.Key.Count()); ++i)
                        {
                            space += " ";
                        }
                        Console.WriteLine(node.Key + space + node.Value);
                    }
                }
            }
        }

        return;
    }
}
