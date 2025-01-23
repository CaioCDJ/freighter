using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Docker.DotNet;
using Docker.DotNet.Models;
using Freighter.Entities;
using Freighter.Services;
using ReactiveUI;
using TextCopy;

namespace Freighter.ViewModels;

public class ImagesPageViewModel : ReactiveObject, IRoutableViewModel {
	public IScreen HostScreen { get; }

	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	public ObservableCollection<Image> images { get; set; }

	private readonly DockerService _docker_service;

	public async Task refresh_data() {
		images.Clear();

		var images_data = await _docker_service.list_images();

		var command = ReactiveCommand.CreateFromTask<string>(onDeleteButtonClicked);
		var copy_command = ReactiveCommand.CreateFromTask<string>(copy_id);

		foreach (var item in images_data) {
			images.Add(new Image() {
				id = item.ID.Replace("sha256:",""),
				size = item.Size,
				repo_tag = item.RepoTags[0],
				name = item.RepoTags[0].Replace(":latest", ""),
				delete = command,
				copy = copy_command,
				created_at = item.Created.ToString("dd/MM/yyyy HH:mm"),
			});
		}
	}

	public ImagesPageViewModel(IScreen hostScreen, DockerService docker_service) {
		HostScreen = hostScreen;
		_docker_service = docker_service;

		images = new ObservableCollection<Image>();
	}

	private async Task copy_id(string id) {
		await ClipboardService.SetTextAsync(id);
	}

	private async Task onDeleteButtonClicked(string id) {
		string name = String.Empty;

		foreach (var item in images) {
			if (item.id == id) {
				name = item.name;
			}
		}

		try {
			await _docker_service.remove_image(id.Replace("sha256:", ""));

			images.Remove(images.SingleOrDefault(x => x.id == id));
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}
}
