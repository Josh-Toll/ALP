using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

class Program : Cell
{
	static void Main()
	{
		Program program = new Program();
		Console.WriteLine("How many entries would you like to read?: ");
		int num_emtries = Convert.ToInt32(Console.ReadLine());
		List<Cell> cells;
		cells = program.ReadCSVFile("C:\\Users\\Aes\\Desktop\\Fall_2024\\Programming Languages\\ALP3\\ALP\\cells.csv", num_emtries);
		//program.PrintCells(cells);
	}

	public List<Cell> ReadCSVFile(string filePath, int num_entries)
	{
		List<Cell> cellList = new List<Cell>();

		using (var reader = new StreamReader(filePath))
		using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
		{
			csv.Context.RegisterClassMap<CellMap>();

			for (int i = 0; i < num_entries; i++)
			{
				csv.Read();
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
}