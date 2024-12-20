using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Immutable;
using Docker.DotNet;
using Docker.DotNet.Models;
using DynamicData;
using Freighter.Entities;
using Microsoft.VisualBasic.CompilerServices;
using ReactiveUI;

namespace Freighter.ViewModels;

public class ContainersPageViewModel : ReactiveObject, IRoutableViewModel {

	public IScreen HostScreen { get; }
	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	private readonly DockerClient _docker_client;

	public ObservableCollection<Container> containers { get; set; }


	public ContainersPageViewModel(IScreen hostScreen) {
		HostScreen = hostScreen;

		containers = new ObservableCollection<Container>();

		_docker_client = new DockerClientConfiguration(
				new Uri("unix:///var/run/docker.sock"))
			.CreateClient();

		var items = _docker_client.Containers.ListContainersAsync(new ContainersListParameters() {
			Limit = 10,
		}).Result;

		foreach (var item in items) {
			containers.Add(new Container() {
				id = item.ID,
				name = item.Names[0].Replace("/", ""),
				state = item.State,
				image_name = item.Image,
				status_message = item.Status,
				ports = item.Ports
			});
		}
	}

	public async Task refresh_data() {
		containers.Clear();

		var items = await _docker_client.Containers.ListContainersAsync(new ContainersListParameters() {
			Limit = 10,
		});

		foreach (var item in items) {
			containers.Add(new Container() {
				id = item.ID,
				name = item.Names[0].Replace("/", ""),
				state = item.State,
				image_name = item.Image,
				status_message = item.Status,
				ports = item.Ports
			});
		}
	}
}
