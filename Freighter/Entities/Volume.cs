namespace Freighter.Entities;

public class Volume {
	public string name { get; set; }
	public string mount_point { get; set; }
	public long? disk_usage { get; set; }
	public string created_at { get; set; }
}
