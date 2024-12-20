using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
	public ReactiveCommand<Unit, IRoutableViewModel> GoContainersPage { get; }

	// The command that navigates a user back.
	public ReactiveCommand<Unit, IRoutableViewModel> GoImagesPage { get; }

	private ViewModelBase _currentPage;

	private readonly ImagesPageViewModel _images_page;
	private readonly ContainersPageViewModel _containers_page;

	public ViewModelBase CurrentPage {
		get { return _currentPage; }
		set => this.RaiseAndSetIfChanged(ref _currentPage, value);
	}

	public MainWindowViewModel() {

		_images_page = new ImagesPageViewModel(this);
		_containers_page = new ContainersPageViewModel(this);

		GoContainersPage = ReactiveCommand.CreateFromObservable(
			() => refresh_page(_containers_page)
		);

		GoImagesPage = ReactiveCommand.CreateFromObservable(
			() => refresh_page(_images_page)
			);

		Router.Navigate.Execute(_containers_page);
	}

	private IObservable<IRoutableViewModel> refresh_page(IRoutableViewModel page) {
		return Observable.FromAsync(async () => {

			await Router.Navigate.Execute(page);

			if (page is ContainersPageViewModel containers_page) {
				await containers_page.refresh_data();
			}

			if (page is ImagesPageViewModel images_page) {
				await images_page.refresh_data();
			}

			return page;
		});
	}

}
