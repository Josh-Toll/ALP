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
		if(this.launch_announced != null && !regex.IsMatch(this.launch_announced))
		{
			//Console.WriteLine($"Invalid announcement date for phone of model: {this.model}");
			this.launch_announced = null;
		}

		// Change date to integer for calculations
		if(this.launch_announced != null)
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
		if(this.launch_status != null && this.launch_status != "Discontinued" && this.launch_status != "Cancelled")
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
		if(this.body_weight != null && !regex.IsMatch(this.body_weight))
		{
			//Console.WriteLine($"Invalid body weight for phone of model: {this.model}");
			this.body_weight = null;
		}
		else if(this.body_weight != null)
		{
			int index = this.body_weight.IndexOfAny(new char[] { ' ', 'g' });
			if(index > 0) 
			{
				string weightString = this.body_weight.Substring(0, index);
				this.body_weight_float = float.Parse(weightString);
			}
		}

		// Check body_sim validity
		if(this.body_sim == "No" || this.body_sim == "Yes" || this.body_sim == "no" || this.body_sim == "yes")
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
		if(this.features_sensors!= null)
		{
			foreach (var item in this.features_sensors.Split(','))
			{
				features_sensors_list.Add(item);
			}
		}
	}
}