using System;

public class CellMap : CsvHelper.Configuration.ClassMap<Cell>
{
	public CellMap()
	{
		Map(m => m.oem).Name("oem");
		Map(m => m.model).Name("model");
		Map(m => m.launch_announced).Name("launch_announced");
		Map(m => m.launch_status).Name("launch_status");
		Map(m => m.body_dimensions).Name("body_dimensions");
		Map(m => m.body_weight).Name("body_weight");
		Map(m => m.body_sim).Name("body_sim");
		Map(m => m.display_type).Name("display_type");
		Map(m => m.display_size).Name("display_size");
		Map(m => m.display_resolution).Name("display_resolution");
		Map(m => m.features_sensors).Name("features_sensors");
		Map(m => m.platform_os).Name("platform_os");
	}
}
