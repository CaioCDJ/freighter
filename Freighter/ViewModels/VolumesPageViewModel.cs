using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Docker.DotNet;
using Freighter.Entities;
using ReactiveUI;

namespace Freighter.ViewModels;

public class VolumesPageViewModel : ReactiveObject, IRoutableViewModel {

	public IScreen HostScreen { get; }
	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	private readonly DockerClient _docker_client;

	public ObservableCollection<Volume> volumes { get; set; }

	public VolumesPageViewModel(IScreen hostScreen) {
		HostScreen = hostScreen;
		_docker_client = new DockerClientConfiguration(
				new Uri("unix:///var/run/docker.sock"))
			.CreateClient();

		volumes = new ObservableCollection<Volume>();

	}

	public async Task refresh_data() {
		volumes.Clear();

		var docker_volumes = await _docker_client.Volumes.ListAsync();

		foreach (var item in docker_volumes.Volumes) {
			volumes.Add(new Volume() {
				name = item.Name,
				mount_point = item.Mountpoint,
				disk_usage = null,
				created_at = item.CreatedAt
			});
			Console.WriteLine(item.Name);
		}
	}
}
