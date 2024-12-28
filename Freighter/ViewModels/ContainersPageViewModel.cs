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
using Freighter.Services;
using Microsoft.VisualBasic.CompilerServices;
using ReactiveUI;

namespace Freighter.ViewModels;

public class ContainersPageViewModel : ReactiveObject, IRoutableViewModel {
	public IScreen HostScreen { get; }

	// Unique identifier for the routable view model.
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	public ObservableCollection<Container> containers { get; set; }

	private ReactiveCommand<string, Unit> start_command { get; set; }
	private ReactiveCommand<string, Unit> stop_command { get; set; }
	private ReactiveCommand<string, Unit> delete_command { get; set; }

	private readonly DockerService _docker_service;


	public ContainersPageViewModel(IScreen hostScreen, DockerService dockerService) {
		HostScreen = hostScreen;
		_docker_service = dockerService;


		start_command = ReactiveCommand.CreateFromTask<string>(onStartContainer);
		stop_command = ReactiveCommand.CreateFromTask<string>(onStopContainer);
		delete_command = ReactiveCommand.CreateFromTask<string>(onDeleteContainer);

		containers = new ObservableCollection<Container>();
	}

	public async Task refresh_data() {
		containers.Clear();

		var items = await _docker_service.list_containers();
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

		if (container is null) { }

		await _docker_service.start_container(id);

		await refresh_data();
	}

	private async Task onStopContainer(string id) {
		var container = containers.SingleOrDefault(x => x.id == id);

		if (container is null) { }

		await _docker_service.stop_container(id);
		await refresh_data();
	}

	private async Task onDeleteContainer(string id) {

		var container = containers.SingleOrDefault(x => x.id == id);

		if (container is null) { }

		if (container.is_running) {
			await onStopContainer(id);
		}

		await _docker_service.remove_container(id);
		await refresh_data();
	}
}
