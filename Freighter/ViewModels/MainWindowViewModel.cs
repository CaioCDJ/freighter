using System.Reactive;
using Freighter.Views;
using Avalonia.Reactive;
using ExCSS;
using ReactiveUI;

namespace Freighter.ViewModels;

public partial class MainWindowViewModel : ReactiveObject, IScreen {
	public string title { get; set; } = "Freighter";

	// Required by the IScreen interface.
	public RoutingState Router { get; } = new RoutingState();
	
	// The command that navigates a user to first view model.
	public ReactiveCommand<Unit, IRoutableViewModel> GoCotainersPage { get; }

	// The command that navigates a user back.
	public ReactiveCommand<Unit, IRoutableViewModel> GoImagesPage { get; }
	
	private ViewModelBase _currentPage;
	public ViewModelBase CurrentPage {
		get  {return _currentPage;}
		set => this.RaiseAndSetIfChanged(ref _currentPage, value);
	}

	public MainWindowViewModel() {
		GoCotainersPage = ReactiveCommand.CreateFromObservable(
			() => Router.Navigate.Execute(new ContainersPageViewModel(this) ) 
		);
		
		GoImagesPage = ReactiveCommand.CreateFromObservable(
			() => Router.Navigate.Execute(new ImagesPageViewModel(this) ) 
		);
		
		Router.Navigate.Execute(new ContainersPageViewModel(this));
	}
}
