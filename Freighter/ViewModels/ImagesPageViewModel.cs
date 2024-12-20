using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Docker.DotNet;
using Docker.DotNet.Models;
using Freighter.Entities;
using ReactiveUI;

namespace Freighter.ViewModels;

public class ImagesPageViewModel : ReactiveObject, IRoutableViewModel {
	public IScreen HostScreen { get; }
	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	private readonly DockerClient _docker_client;

	public ObservableCollection<Image> images { get; set; }


	public async Task refresh_data() {
		images.Clear();

		var images_data = await _docker_client.Images.ListImagesAsync(
			new ImagesListParameters()
		);

		var command = ReactiveCommand.Create<string>(onDeleteButtonClicked);

		foreach (var item in images_data) {
			images.Add(new Image() {
				id = item.ID,
				size = item.Size,
				repo_tag = item.RepoTags[0],
				name = item.RepoTags[0].Replace(":latest", ""),
				delete = command,
				created_at = item.Created.ToString("dd/MM/yyyy HH:mm"),
			});
		}
	}

	public ImagesPageViewModel(IScreen hostScreen) {

		HostScreen = hostScreen;
		_docker_client = new DockerClientConfiguration(
				new Uri("unix:///var/run/docker.sock"))
			.CreateClient();
		images = new ObservableCollection<Image>();

	}

	private void onDeleteButtonClicked(string id) {

		string name = String.Empty;

		foreach (var item in images) {
			if (item.id == id) {
				name = item.name;
			}
		}

		try {
			_docker_client.Images.DeleteImageAsync(
				id.Replace("sha256:", ""),
				new ImageDeleteParameters(),
				default);
			images.Remove(images.SingleOrDefault(x => x.id == id));
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}


	}
}
