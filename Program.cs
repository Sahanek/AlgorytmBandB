using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var path = Path.Combine(currentPath + @"\dane9.txt"); // Zmień nazwe pliku, jeśli chcesz inny wynik
var tasks = new List<Task>();
using (StreamReader sr = new(path))
{
    var line = sr.ReadLine();
    while ((line = sr.ReadLine()) is not null)
    {
        var splitLine = line.Split(' ');
        tasks.Add(new Task
        {
            CzasWykononania = int.Parse(splitLine[0]),
            WspolczynnikKary = int.Parse(splitLine[1]),
            ZadanyTerminEnd = int.Parse(splitLine[2])
        });

    }
}

Console.WriteLine($"Wynik:{BandB(tasks)} ");


static int BandB(List<Task> tasks)
{
    List<int> opts = new();
    for (int i = 0; i <= Math.Pow(2, tasks.Count) - 1; i++)
    {
        opts.Add(CountOpt(tasks, Convert.ToString(i, 2), opts));
    }
    return opts.Last();
}

static int CountOpt(List<Task> tasks, string bitString, List<int> opt)
{
    int min = int.MaxValue;
    if (bitString.Length == 1 && bitString[0] == '0') return 0;
    for (int i = bitString.Length - 1; i >= 0; i--)
    {
        if (IfBitChecked(bitString, i))// Ostatni bit zawsze jest 1 elementem w taskach
        {
            var elementAt = Convert.ToInt32(bitString.ReplaceAt(i, '0'), 2);
            var optCounted = opt[elementAt] + tasks[bitString.Length -1 - i].WspolczynnikKary
                * Math.Max(0, SumTaskWorkTime(bitString, tasks) - tasks[bitString.Length - 1 - i].ZadanyTerminEnd);
            if (min > optCounted)
                min = optCounted;
        }
    }

    return min;
}

static bool IfBitChecked(string bitString, int index)
{
    return bitString[index] == '1';
}

static int SumTaskWorkTime(string bitString, IEnumerable<Task> tasks)
{
    int sum = 0;
    for (int i = 0; i < bitString.Length; i++)
    {
        if (IfBitChecked(bitString, bitString.Length -1 - i))//ostatni bit => pierwszym zadaniem
            sum += tasks.ElementAt(i).CzasWykononania;
    }

    return sum;
}

public static class StringExtensions
{
    public static string ReplaceAt(this string input, int index, char newChar)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input");
        }
        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }
}

class Task
{
    //J - zadanie
    public int CzasWykononania { get; set; } // pj
    public int ZadanyTerminEnd { get; set; } // dj - żadnay termin zakończenia wykonywania
    public int WspolczynnikKary { get; set; } // wj



}