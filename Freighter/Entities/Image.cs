namespace Freighter.Entities;

public class Image {
	public string id { get; set; }
	public long size { get; set; }
	public string repo_tag { get; set; }
	public string name { get; set; }
	public int containers { get; set; }
}
