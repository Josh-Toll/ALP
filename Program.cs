using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Linq;

class Program : Cell
{
	static void Main()
	{
		Program program = new Program();
		List<Cell> cells;
		cells = program.ReadCSVFile("C:\\Users\\Aes\\Desktop\\Fall_2024\\Programming Languages\\ALP3\\ALP\\cells.csv");
		Dictionary<string, float?> averageWeightDict = program.CalculateAverageWeight(cells);
		float? maxWeight = 0;
		string? heaviestOEM = null;
		foreach (var key in averageWeightDict.Keys)
		{
			if(averageWeightDict[key] > maxWeight )
			{
				maxWeight = averageWeightDict[key];
				heaviestOEM = key;
			}
		}
		//Console.WriteLine($"Company: {heaviestOEM} \nAverage Weight: {maxWeight}g\n");

		//List<Cell> lateReleases = program.FindLateReleases(cells);
		//foreach(var cell in lateReleases)
		//{
		//Console.WriteLine($"Model: {cell.model} \nOEM: {cell.oem}");
		//}

		//Console.WriteLine($"There are {program.FindOneFeatureSensor(cells)} phones with 1 feature sensor");

		Console.WriteLine($"The most phones were launched in the year {program.FindMostPhones(cells)}");

	}

	public List<Cell> ReadCSVFile(string filePath)
	{
		List<Cell> cellList = new List<Cell>();

		using (var reader = new StreamReader(filePath))
		using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
		{
			csv.Context.RegisterClassMap<CellMap>();

			while(csv.Read())
			{
				Cell record = csv.GetRecord<Cell>();
				record.CleanCell();
				cellList.Add(record);
			}
		}
		return cellList;
	}
	public void PrintCells(List<Cell> cellList)
	{
		for (int i = 0; i < cellList.Count; i++)
		{
			var cell = cellList[i];
			Console.WriteLine($"Cell {i + 1}: OEM - {cell.oem}, Model - {cell.model}, Body Weight - {cell.body_weight_float}, Display Size - {cell.display_size_float}");
			// Add additional properties as needed
		}
	}


	// Utility Functions
	private Dictionary<string, float?> CalculateAverageWeight(List<Cell> cellList)
	{
		Dictionary<string, List<float?>> oemWeightDict = new Dictionary<string, List<float?>>();
		foreach (var cell in cellList)
		{
			if(oemWeightDict.ContainsKey(cell.oem))
			{
				oemWeightDict[cell.oem].Add(cell.body_weight_float);
			}
			else
			{
				oemWeightDict.Add(cell.oem, [cell.body_weight_float]);
			}
		}
		Dictionary<string, float?> averageWeightDict = new Dictionary<string, float?>();
		foreach (string key in oemWeightDict.Keys)
		{
			averageWeightDict.Add(key, oemWeightDict[key].Average());
		}
		return averageWeightDict;
	}

	private List<Cell> FindLateReleases(List<Cell> cellList)
	{
		List<Cell> lateReleases = new List<Cell>();
		foreach (var cell in cellList)
		{
			if(cell.launch_announced_int != null && cell.launch_year != 0)
			{
				if(cell.launch_announced_int != cell.launch_year)
				{
					lateReleases.Add(cell);
				}
			}
		}
		return lateReleases;
	}

	private int FindOneFeatureSensor(List<Cell> cellList)
	{
		int numOneSensor = 0;
		foreach (var cell in cellList)
		{
			if(cell.features_sensors_list.Count > 1)
			{
				numOneSensor++;
			}
		}
		return numOneSensor;
	}

	private int FindMostPhones(List<Cell> cellList)
	{
		int mostLaunched = 0;
		Dictionary<int, int> years = new Dictionary<int, int>();
		foreach (var cell in cellList)
		{
			if(cell.launch_year > 1999 && !years.ContainsKey(cell.launch_year)) 
			{
				years.Add(cell.launch_year, 1);
			}
			else if(cell.launch_year > 1999)
			{
				years[cell.launch_year]++;
			}
		}
		foreach(int num in years.Keys)
		{
			if (years[num] > mostLaunched)
			{
				mostLaunched = num;
			}
		}
		return mostLaunched; 
	}
}