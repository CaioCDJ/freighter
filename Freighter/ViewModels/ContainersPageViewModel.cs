using System;
using System.Collections.Generic;
using Docker.DotNet.Models;
using Freighter.Entities;
using ReactiveUI;

namespace Freighter.ViewModels;

public class ContainersPageViewModel : ReactiveObject, IRoutableViewModel{
	
	public IScreen HostScreen { get; }
	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	public List<Container> containers { get; } = new List<Container>();
	
	public ContainersPageViewModel(IScreen hostScreen) {
		HostScreen = hostScreen;
		for(int i=0;i<4;i++)
			containers.Add(new Container() {
				id = "1983012830",
				image_name = "postgres",
				state = "exited",
				status_message = "exited",
				ports = new List<Port>()
			});
		foreach (var item in containers) {
			item.ports.Add(
				new Port()
				);
		}
	}
}
