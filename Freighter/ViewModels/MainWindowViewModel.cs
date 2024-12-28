using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Freighter.Views;
using Avalonia.Reactive;
using ExCSS;
using Freighter.Services;
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

	public ReactiveCommand<Unit, IRoutableViewModel> GoVolumesPage { get; }

	private ViewModelBase _currentPage;

	private readonly ImagesPageViewModel _images_page;
	private readonly ContainersPageViewModel _containers_page;
	private readonly VolumesPageViewModel _volumes_page;
	private readonly DockerService _docker_service;

	public ViewModelBase CurrentPage {
		get { return _currentPage; }
		set => this.RaiseAndSetIfChanged(ref _currentPage, value);
	}

	public MainWindowViewModel() {
		_docker_service = new DockerService();
		_images_page = new ImagesPageViewModel(this, _docker_service);
		_containers_page = new ContainersPageViewModel(this, _docker_service);
		_volumes_page = new VolumesPageViewModel(this, _docker_service);

		GoContainersPage = ReactiveCommand.CreateFromObservable(
			() => refresh_page(_containers_page)
		);

		GoImagesPage = ReactiveCommand.CreateFromObservable(
			() => refresh_page(_images_page)
		);

		GoVolumesPage = ReactiveCommand.CreateFromObservable(
			() => refresh_page(_volumes_page)
			);

		refresh_page(_containers_page).Wait();
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

			if (page is VolumesPageViewModel volumes_page) {
				await volumes_page.refresh_data();
			}

			return page;
		});
	}
}
