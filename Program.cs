using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

class Program
{
	public void ReadCSVFile(string filePath)
	{
		List<Cell> cellList = new List<Cell>();

		using (var reader = new StreamReader(filePath))
		using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
		{
			csv.Context.RegisterClassMap<CellMap>();

			while (csv.Read())
			{
				var record = csv.GetRecord<Cell>();
				cellList.Add(record);
			}
		}

		PrintFirst10Cells(cellList);
	}

	public void PrintFirst10Cells(List<Cell> cellList)
	{
		for (int i = 0; i < cellList.Count; i++)
		{
			var cell = cellList[i];
			Console.WriteLine($"Cell {i + 1}: OEM - {cell.oem}, Model - {cell.model}, ");
			// Add additional properties as needed
		}
	}

	static void Main()
	{
		Program program = new Program();
		program.ReadCSVFile("C:\\Users\\Aes\\Desktop\\Fall_2024\\Programming Languages\\ALP3\\ALP\\cells.csv");
	}
}