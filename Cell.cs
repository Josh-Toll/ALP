using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;

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
	public float? body_weight_float;
	public float? display_size_float;
	public int launch_year = 0; // launch_year of 0 means that it was never released
	public List<string> features_sensors_list = [];


	// Data Transformation
	public void CleanCell()
	{
		// Set each variable to null if it is empty or "-"
		if (this.oem == "" || this.oem == " " || this.oem == "-")
		{
			this.oem = null;
		}
		if (this.model == "" || this.model == " " || this.model == "-")
		{
			this.model = null;
		}
		if (this.launch_announced == "" || this.launch_announced == " " || this.launch_announced == "-")
		{
			this.launch_announced = null;
		}
		if (this.launch_status == "" || this.launch_status == " " || this.launch_status == "-")
		{
			this.launch_status = null;
		}
		if (this.body_dimensions == "" || this.body_dimensions == " " || this.body_dimensions == "-")
		{
			this.body_dimensions = null;
		}
		if (this.body_weight == "" || this.body_weight == " " || this.body_weight == "-")
		{
			this.body_weight = null;
		}
		if (this.body_sim == "" || this.body_sim == " " || this.body_sim == "-")
		{
			this.body_sim = null;
		}
		if (this.display_type == "" || this.display_type == " " || this.display_type == "-")
		{
			this.display_type = null;
		}
		if (this.display_size == "" || this.display_size == " " || this.display_size == "-")
		{
			this.display_size = null;
		}
		if (this.display_resolution == "" || this.display_resolution == " " || this.display_resolution == "-")
		{
			this.display_resolution = null;
		}
		if (this.features_sensors == "" || this.features_sensors == " " || this.features_sensors == "-")
		{
			this.features_sensors = null;
		}
		if (this.platform_os == "" || this.platform_os == " " || this.platform_os == "-")
		{
			this.platform_os = null;
		}

		// Check date format
		Regex regex = new Regex(@"\b\d{4}\b|Discontinued|Cancelled");
		if (this.launch_announced != null && !regex.IsMatch(this.launch_announced))
		{
			//Console.WriteLine($"Invalid announcement date for phone of model: {this.model}");
			this.launch_announced = null;
		}

		// Change date to integer for calculations
		if (this.launch_announced != null)
		{
			MatchCollection matches = Regex.Matches(this.launch_announced, @"\b\d{4}\b");
			foreach (Match match in matches)
			{
				if (match.Success && match.Value.Length >= 4)
				{
					if (int.TryParse(match.Value.Substring(0, 4), out int year))
					{
						this.launch_announced_int = year;
					}
				}
			}
		}


		if (this.launch_status != "Discontinued" && this.launch_status != "Cancelled" && !regex.IsMatch(this.launch_status))
		{
			Console.WriteLine($"Invalid launch status for phone of model: {this.model}");
			this.launch_status = null;
			this.launch_year = 0;
		}


		// Find the year an item was/will be released
		if (this.launch_status != null && this.launch_status != "Discontinued" && this.launch_status != "Cancelled")
		{
			string pattern = @"\d\d\d\d";
			MatchCollection matches = Regex.Matches(this.launch_status, pattern);

			foreach (Match match in matches)
			{
				if (match.Success && match.Value.Length >= 4)
				{
					if (int.TryParse(match.Value.Substring(0, 4), out int year))
					{
						this.launch_year = year;
					}
				}
			}
		}

		// Convert body_weight
		regex = new Regex(@"(\d+(\.\d+)?)\s*g");
		if (this.body_weight != null && !regex.IsMatch(this.body_weight))
		{
			//Console.WriteLine($"Invalid body weight for phone of model: {this.model}");
			this.body_weight = null;
		}
		else if (this.body_weight != null)
		{
			int index = this.body_weight.IndexOfAny(new char[] { ' ', 'g' });
			if (index > 0)
			{
				string weightString = this.body_weight.Substring(0, index);
				this.body_weight_float = float.Parse(weightString);
			}
		}

		// Check body_sim validity
		if (this.body_sim == "No" || this.body_sim == "Yes" || this.body_sim == "no" || this.body_sim == "yes")
		{
			//Console.WriteLine($"Invalid body sim for phone of model: {this.model}");
			this.body_sim = null;
		}

		// Convert display_size
		regex = new Regex(@"(\d+(\.\d+)?) inches");
		if (this.display_size != null && !regex.IsMatch(this.display_size))
		{
			//Console.WriteLine($"Invalid display size for phone of model: {this.model}");
			this.display_size = null;
		}
		else if (this.display_size != null)
		{
			int index = this.display_size.IndexOfAny(new char[] { ' ', 'i' });
			if (index > 0)
			{
				string sizeString = this.display_size.Substring(0, index);
				this.display_size_float = float.Parse(sizeString);

			}
		}

		// Check features_sensors validity
		if (this.features_sensors != null && float.TryParse(this.features_sensors, out _))
		{
			//Console.WriteLine($"Invalid features sensors for phone of model: {this.model}");
			this.features_sensors = null;
		}

		// Check platform_os validity
		if (this.platform_os != null && float.TryParse(this.platform_os, out _))
		{
			//Console.WriteLine($"Invalid platform os for phone of model: {this.model}");
			this.platform_os = null;
		}

		// Create a list of features
		if (this.features_sensors != null)
		{
			foreach (var item in this.features_sensors.Split(','))
			{
				features_sensors_list.Add(item);
			}
		}
	}

	// Constructor function


	// Output Cell
	public void Output()
	{
		Console.WriteLine($"OEM - {this.oem}, Model - {this.model}, Body Weight - {this.body_weight_float}, Display Size - {this.display_size_float}, Launch Announced - {this.launch_announced_int}, Launch Year - {this.launch_year}, Body Dimensions - {this.body_dimensions}, Body Sim - {this.body_sim}, Display Type - {this.display_type}, Display Resolution - {this.display_resolution}, Features Sensors - {this.features_sensors}, Platform OS - {this.platform_os}");
	}

	// Utility Functions
	public static Dictionary<string, float?> CalculateAverageWeight(List<Cell> cellList)
	{
		Dictionary<string, List<float?>> oemWeightDict = new Dictionary<string, List<float?>>();
		foreach (var cell in cellList)
		{
			// Add the company and weight of the current phone to the dictionary
			if (oemWeightDict.ContainsKey(cell.oem))
			{
				oemWeightDict[cell.oem].Add(cell.body_weight_float);
			}
			// If the company is already in the dictionary, add the weight of the current phone to the list for that particular OEM
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

	public static List<Cell> FindLateReleases(List<Cell> cellList)
	{
		List<Cell> lateReleases = new List<Cell>();
		foreach (var cell in cellList)
		{
			if (cell.launch_announced_int != null && cell.launch_year != 0)
			{
				// Find phones where the launch_announced and launc_year are different
				if (cell.launch_announced_int != cell.launch_year)
				{
					lateReleases.Add(cell);
				}
			}
		}
		return lateReleases;
	}

	public static int FindOneFeatureSensor(List<Cell> cellList)
	{
		int numOneSensor = 0;
		foreach (var cell in cellList)
		{
			// Find the number of phones with only 1 feature sensor
			if (cell.features_sensors_list.Count > 1)
			{
				numOneSensor++;
			}
		}
		return numOneSensor;
	}

	public static int FindMostPhones(List<Cell> cellList)
	{
		int mostLaunched = 0;
		int year = 0;
		// Create a dictionary of the form {year: number_of_phones}
		Dictionary<int, int> years = new Dictionary<int, int>();
		foreach (var cell in cellList)
		{
			if (cell.launch_year > 1999 && !years.ContainsKey(cell.launch_year))
			{
				years.Add(cell.launch_year, 1);
			}
			else if (cell.launch_year > 1999)
			{
				years[cell.launch_year]++;
			}
		}
		// Iterate through the dictionary to find the year with the highest value
		foreach (int num in years.Keys)
		{
			if (years[num] > mostLaunched)
			{
				mostLaunched = years[num];
				year = num;
			}
		}
		return year;
	}

	public static Dictionary<string, List<string>> FindUniqueValues(List<Cell> cellList)
	{
		Dictionary<string, List<string>> uniqueValues = new Dictionary<string, List<string>>{{ "OEM", new List<string>() },{ "Model", new List<string>() },{ "Launch Announced", new List<string>() },{ "Launch Year", new List<string>() },{ "Body Dimensions", new List<string>() },{ "Body Weight", new List<string>() },{ "Body SIM", new List<string>() },{ "Display Type", new List<string>() },{ "Display Size", new List<string>() },{ "Display Resolution", new List<string>() },{ "Platform OS", new List<string>() }};
		foreach (var cell in cellList)
		{
			if (!uniqueValues["OEM"].Contains(cell.oem))
			{
				uniqueValues["OEM"].Add(cell.oem);
			}
			if (!uniqueValues["Model"].Contains(cell.model))
			{
				uniqueValues["Model"].Add(cell.model);
			}
			if (!uniqueValues["Launch Announced"].Contains(cell.launch_announced_int.ToString()))
			{
				uniqueValues["Launch Announced"].Add(cell.launch_announced_int.ToString());
			}
			if (!uniqueValues["Launch Year"].Contains(cell.launch_year.ToString()))
			{
				uniqueValues["Launch Year"].Add(cell.launch_year.ToString());
			}
			if (!uniqueValues["Body Dimensions"].Contains(cell.body_dimensions))
			{
				uniqueValues["Body Dimensions"].Add(cell.body_dimensions);
			}
			if (!uniqueValues["Body Weight"].Contains(cell.body_weight_float.ToString()))
			{
				uniqueValues["Body Weight"].Add(cell.body_weight_float.ToString());
			}
			if (!uniqueValues["Body SIM"].Contains(cell.body_sim))
			{
				uniqueValues["Body SIM"].Add(cell.body_sim);
			}
			if (!uniqueValues["Display Type"].Contains(cell.display_type))
			{
				uniqueValues["Display Type"].Add(cell.display_type);
			}
			if (!uniqueValues["Display Size"].Contains(cell.display_size_float.ToString()))
			{
				uniqueValues["Display Size"].Add(cell.display_size_float.ToString());
			}
			if (!uniqueValues["Display Resolution"].Contains(cell.display_resolution))
			{
				uniqueValues["Display Resolution"].Add(cell.display_resolution);
			}
			if (!uniqueValues["Platform OS"].Contains(cell.platform_os))
			{
				uniqueValues["Platform OS"].Add(cell.platform_os);
			}
		}
		return uniqueValues;
	}
}