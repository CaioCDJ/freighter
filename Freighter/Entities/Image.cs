using System.Reactive;
using ReactiveUI;

namespace Freighter.Entities;

public class Image {
	public string id { get; init; }
	public long size { get; init; }
	public string repo_tag { get; init; }
	public string name { get; init; }

	public int containers { get; init; } = 0;
	
	public string created_at { get; init; }
	
	public ReactiveCommand<string, Unit> delete { get; init; }
}
