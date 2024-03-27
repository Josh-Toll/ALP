using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Text.RegularExpressions;

public class Cell 
{
	public string? oem { get; set; }
	public string? model { get; set; }
	public string? launch_announced { get; set; }
	public string? launch_status { get; set; }
	public string? body_dimensions { get; set; }
	public string? body_weight { get; set; }
	public string? body_sim { get; set; }
	public string? display_type { get; set; }
	public string? display_size { get; set; }
	public string? display_resolution { get; set; }
	public string? features_sensors { get; set; }
	public string? platform_os { get; set; }

	// Cell columns after type conversion
	public int? launch_announced_int { get; set; }
	public float? body_weight_float { get; set; }
	public float? display_size_float { get; set; }
	public int? launch_year { get; set; }



	// Data Transformation Methods
	public List<Cell> ReadCSVFile(string filePath, int num_entries)
	{
		List<Cell> cellList = new List<Cell>();

		using (var reader = new StreamReader(filePath))
		using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
		{
			csv.Context.RegisterClassMap<CellMap>();

			for(int i = 0; i < num_entries; i++)
			{
				csv.Read();
				Cell record = csv.GetRecord<Cell>();
				CleanCell(record);
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
	private void CleanCell(Cell record)
	{
		// Set each variable to null if it is empty or "-"
		if (record.oem == "" || record.oem == " " || record.oem == "-")
		{
			record.oem = null;
		}
		if (record.model == "" || record.model == " " || record.model == "-")
		{
			record.model = null;
		}
		if (record.launch_announced == "" || record.launch_announced == " " || record.launch_announced == "-")
		{
			record.launch_announced = null;
		}
		if (record.launch_status == "" || record.launch_status == " " || record.launch_status == "-")
		{
			record.launch_status = null;
		}
		if (record.body_dimensions == "" || record.body_dimensions == " " || record.body_dimensions == "-")
		{
			record.body_dimensions = null;
		}
		if (record.body_weight == "" || record.body_weight == " " || record.body_weight == "-")
		{
			record.body_weight = null;
		}
		if (record.body_sim == "" || record.body_sim == " " || record.body_sim == "-")
		{
			record.body_sim = null;
		}
		if (record.display_type == "" || record.display_type == " " || record.display_type == "-")
		{
			record.display_type = null;
		}
		if (record.display_size == "" || record.display_size == " " || record.display_size == "-")
		{
			record.display_size = null;
		}
		if (record.display_resolution == "" || record.display_resolution == " " || record.display_resolution == "-")
		{
			record.display_resolution = null;
		}
		if (record.features_sensors == "" || record.features_sensors == " " || record.features_sensors == "-")
		{
			record.features_sensors = null;
		}
		if (record.platform_os == "" || record.platform_os == " " || record.platform_os == "-")
		{
			record.platform_os = null;
		}

		// Check date format
		Regex regex = new Regex(@"\b\d{4}\b|Discontinued|Cancelled");
		if(record.launch_announced != null && !regex.IsMatch(record.launch_announced))
		{
			//Console.WriteLine($"Invalid announcement date for phone of model: {record.model}");
			//Console.WriteLine(record.launch_announced);
			record.launch_announced = null;
		}
		else if(record.launch_announced != null)
		{
			record.launch_announced_int = int.Parse(record.launch_announced.Substring(0,4));
			//Console.WriteLine(record.launch_announced_int);
		}
		if(record.launch_status != null && record.launch_status != "Discontinued" && record.launch_status != "Cancelled" && !regex.IsMatch(record.launch_status))
		{
			Console.WriteLine($"Invalid launch status for phone of model: {record.model}");
			record.launch_status = null;
		}

		// Find the year an item was/will be released
		Regex regex1 = new Regex(@"");
		if(record.launch_status != null && record.launch_status != "Discontinued" && record.launch_status != "Cancelled")
		{
			string numberPattern = @"\d\d\d\d";
			MatchCollection matches = Regex.Matches(record.launch_status, numberPattern);

			foreach (Match match in matches)
			{
				if (match.Success && match.Value.Length >= 4)
				{
					if (int.TryParse(match.Value.Substring(0, 4), out int year))
					{
						record.launch_year = year;
					}
				}
			}
		}

		// Convert body_weight
		regex = new Regex(@"(\d+(\.\d+)?)\s*g");
		if(record.body_weight != null && !regex.IsMatch(record.body_weight))
		{
			//Console.WriteLine($"Invalid body weight for phone of model: {record.model}");
			record.body_weight = null;
		}
		else if(record.body_weight != null)
		{
			int index = record.body_weight.IndexOfAny(new char[] { ' ', 'g' });
			if(index > 0) 
			{
				string weightString = record.body_weight.Substring(0, index);
				record.body_weight_float = float.Parse(weightString);
			}
		}

		// Check body_sim validity
		if(record.body_sim == "No" || record.body_sim == "Yes" || record.body_sim == "no" || record.body_sim == "yes")
		{
			//Console.WriteLine($"Invalid body sim for phone of model: {record.model}");
			record.body_sim = null;
		}

		// Convert display_size
		regex = new Regex(@"(\d+(\.\d+)?) inches");
		if (record.display_size != null && !regex.IsMatch(record.display_size))
		{
			//Console.WriteLine($"Invalid display size for phone of model: {record.model}");
			record.display_size = null;
		}
		else if (record.display_size != null)
		{
			int index = record.display_size.IndexOfAny(new char[] { ' ', 'i' });
			if (index > 0)
			{
				string sizeString = record.display_size.Substring(0, index);
				record.display_size_float = float.Parse(sizeString);

			}
		}

		// Check features_sensors validity
		if (record.features_sensors != null && float.TryParse(record.features_sensors, out _))
		{
			//Console.WriteLine($"Invalid features sensors for phone of model: {record.model}");
			record.features_sensors = null;
		}

		// Check platform_os validity
		if (record.platform_os != null && float.TryParse(record.platform_os, out _))
		{
			//Console.WriteLine($"Invalid platform os for phone of model: {record.model}");
			record.platform_os = null;
		}

	}
}