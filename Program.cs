﻿using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

class Program : Cell
{
	static void Main()
	{
		Program program = new Program();
		List<Cell> cells = new List<Cell>();
		// Change this to the file path if the file path is different from the default
		string file_path = "cells.csv";
		try
		{
			cells = program.ReadCSVFile(file_path);
		}
		catch
		{
			// Change the file_path string if this event occurs
			Console.WriteLine($"Error: {file_path} is not a valid file path");
			return;
		}
		// Calculate Average weight for each OEM
		Dictionary<string, float?> averageWeightDict = CalculateAverageWeight(cells);
		float? maxWeight = 0;
		string? heaviestOEM = null;
		foreach (var oem in averageWeightDict.Keys)
		{
			// Find max weight
			if (averageWeightDict[oem] > maxWeight)
			{
				maxWeight = averageWeightDict[oem];
				heaviestOEM = oem;
			}
		}
		// Output OEM and average weight of company with the heaviest phones
		Console.WriteLine($"Company: {heaviestOEM} \nAverage Weight: {maxWeight}g\n");

		// Find phones that were released in a later year than they were announced
		List<Cell> lateReleases = FindLateReleases(cells);
		foreach (var cell in lateReleases)
		{
			Console.WriteLine($"Model: {cell.model} OEM: {cell.oem}");
		}

		// Find phones with 1 feature sensor
		Console.WriteLine($"\nThere are {FindOneFeatureSensor(cells)} phones with 1 feature sensor");

		// Find what year had the most phones release after 1999
		int year = FindMostPhones(cells);
		Console.WriteLine($"\nThe most phones were launched in the year {year}");

		// Output unique vaues for each attribute
		Dictionary<string, List<string>> uniqueValues = FindUniqueValues(cells);
		Console.WriteLine("\nWhich attribute would you like to find unique values for: \nOEM \nModel \nLaunch Announced \nLaunch Year \nBody Dimensions \nBody Weight \nBody SIM \nDisplay Type \nDisplay Size \nDisplay Resolution \nFeatures Sensors \nPlatform OS");
		string? key = Console.ReadLine();
		if (key != null)
		{
			try
			{
				string values = string.Join(",", uniqueValues[key]);
				Console.WriteLine($"{key}: {values}");
			}
			catch
			{
				Console.WriteLine("Error: Attribute not found");
			}
		}
	}

	public List<Cell> ReadCSVFile(string filePath)
	{
		List<Cell> cellList = new List<Cell>();

		using (var reader = new StreamReader(filePath))
		using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
		{
			csv.Context.RegisterClassMap<CellMap>();

			while (csv.Read())
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
			Console.WriteLine($"Cell {i + 1}: ");
			cell.Output();
		}
	}
}