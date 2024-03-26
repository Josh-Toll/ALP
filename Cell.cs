using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;

public class Cell
{
	public string oem { get; set; }
	public string model { get; set; }
	public string launch_announced { get; set; }
	public string launch_status { get; set; }
	public string body_dimensions { get; set; }
	public string body_weight { get; set; }
	public string body_sim { get; set; }
	public string display_type { get; set; }
	public string display_size { get; set; }
	public string display_resolution { get; set; }
	public string features_sensors { get; set; }
	public string platform_os { get; set; }

	public void ReadCSVFile(string filePath)
	{
		using (var reader = new StreamReader(filePath))
		using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
		{
			csv.Context.RegisterClassMap<CellMap>();

			for (int i = 0; i < 10; i++)
			{
				if (csv.Read())
				{
					var record = csv.GetRecord<Cell>();
					// Process the record as needed
				}
				else
				{
					break;
				}
			}
		}
	}

}