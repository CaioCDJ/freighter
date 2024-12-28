using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Numerics;
using System.Reactive;
using Docker.DotNet.Models;
using ReactiveUI;

namespace Freighter.Entities;

public class Container {
	public string id { get; init; }
	public string name { get; init; }
	public string image_name { get; init; }
	public IList<Port> ports { get; init; }
	public string state { get; init; }
	public string status_message { get; init; }

	public bool is_running { get; init; }

	public bool not_running {
		get => !is_running;
		init => is_running = !value;
	}

	public ReactiveCommand<string, Unit> start_command { get; init; }
	public ReactiveCommand<string, Unit> stop_command { get; init; }
	public ReactiveCommand<string, Unit> delete_command { get; init; }

}
