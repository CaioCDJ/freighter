using System;
using System.Collections.Generic;
using Avalonia.ReactiveUI;
using Freighter.Entities;
using ReactiveUI;

namespace Freighter.ViewModels;

public class ImagesPageViewModel : ReactiveObject, IRoutableViewModel {
	public IScreen HostScreen { get; }
	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
	public ImagesPageViewModel (IScreen hostScreen) {
		HostScreen = hostScreen;
		
		images = new List<Image>() { };
		for(int i=0;i<10;i++) 
			images.Add(new Image() {
				id = "oal",
				name = "postgres",
				size = 3993,
				repo_tag = "latest",
				containers = 2
			});
	}

	public List<Image> images { get; set; }
}
