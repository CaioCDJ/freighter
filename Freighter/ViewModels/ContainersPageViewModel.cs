using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Immutable;
using CommunityToolkit.Mvvm.ComponentModel;
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

	private ReactiveCommand<string, Unit> start_command { get; set; }
	private ReactiveCommand<string, Unit> stop_command { get; set; }
	private ReactiveCommand<string, Unit> delete_command { get; set; }

	private ReactiveCommand<Unit, Unit> refresh { get; set; }

	private Func<Task> refreshAction { get; set; }
	
	public ContainersPageViewModel(IScreen hostScreen) {
		HostScreen = hostScreen;

		refreshAction = async () => await refresh_data();
		
		start_command = ReactiveCommand.CreateFromTask<string>(onStartContainer);
		stop_command = ReactiveCommand.CreateFromTask<string>(onStopContainer);
		delete_command = ReactiveCommand.CreateFromTask<string>(onDeleteContainer);
		
		containers = new ObservableCollection<Container>();

		_docker_client = new DockerClientConfiguration(
				new Uri("unix:///var/run/docker.sock"))
			.CreateClient();
		var items = _docker_client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = 10, })
			.Result;

		foreach (var item in items) {
			containers.Add(new Container() {
				id = item.ID,
				name = item.Names[0].Replace("/", ""),
				state = item.State,
				image_name = item.Image,
				status_message = item.State,
				is_running = item.State.Contains("running"),
				ports = item.Ports,
				stop_command = stop_command,
				start_command = start_command,
				delete_command = delete_command,
			});
		}
	}

	public async Task refresh_data() {
		containers.Clear();

		var items = await _docker_client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = 10, });

		foreach (var item in items) {
			containers.Add(new Container() {
				id = item.ID,
				name = item.Names[0].Replace("/", ""),
				state = item.State,
				image_name = item.Image,
				status_message = item.State,
				is_running = item.State.Contains("running"),
				ports = item.Ports,
				stop_command = stop_command,
				start_command = start_command,
				delete_command = delete_command,
			});
		}
	}

	private async Task onStartContainer(string id) {

		var container = containers.SingleOrDefault(x => x.id == id);
		
		if (container is null) {}

		await _docker_client.Containers.StartContainerAsync(
			id,
			new ContainerStartParameters());

		await refresh_data();
	}

	private async Task onStopContainer(string id) {
		var container = containers.SingleOrDefault(x => x.id == id);
		
		if (container is null) {}
		
		await _docker_client.Containers.StopContainerAsync(
			id,
			new ContainerStopParameters());
		await refresh_data();
	}

	private async Task onDeleteContainer(string id) {
		
		var container = containers.SingleOrDefault(x => x.id == id);
		
		if (container is null) {}

		if (container.is_running) {
			await onStopContainer(id);
		}

		await	_docker_client.Containers.RemoveContainerAsync(
			id, new ContainerRemoveParameters());

		await refresh_data();
	}
}
