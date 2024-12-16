using System.Collections.Generic;
using Docker.DotNet.Models;

namespace Freighter.Entities;

public class Container {
	public string id { get; set; }
	public string image_name { get; set; }
	public List<Port> ports { get; set; }
	public string state { get; set; }
	public string status_message { get; set; }
}
