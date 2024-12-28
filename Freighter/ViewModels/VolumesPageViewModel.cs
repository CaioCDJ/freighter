using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Docker.DotNet;
using Freighter.Entities;
using Freighter.Services;
using ReactiveUI;

namespace Freighter.ViewModels;

public class VolumesPageViewModel : ReactiveObject, IRoutableViewModel {

	public IScreen HostScreen { get; }
	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	private readonly DockerClient _docker_client;

	private readonly DockerService _docker_service;

	public ObservableCollection<Volume> volumes { get; set; }

	public VolumesPageViewModel(IScreen hostScreen, DockerService dockerService) {
		HostScreen = hostScreen;
		_docker_service = dockerService;

		volumes = new ObservableCollection<Volume>();
	}

	public async Task refresh_data() {
		volumes.Clear();

		var docker_volumes = await _docker_service.list_volumes();
		
		foreach (var item in docker_volumes.Volumes) {
			volumes.Add(new Volume() {
				name = item.Name,
				mount_point = item.Mountpoint,
				disk_usage = null,
				created_at = item.CreatedAt
			});
		}
	}
}
