using System;
using ReactiveUI;

namespace Freighter.ViewModels;

public class ContainersPageModel : ReactiveObject, IRoutableViewModel{
	
	public IScreen HostScreen { get; }
	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
	public ContainersPageModel(IScreen hostScreen) {
		HostScreen = hostScreen;
	}
}
