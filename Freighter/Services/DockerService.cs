using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Runtime.InteropServices;

namespace Freighter.Services;

public class DockerService {

	private readonly DockerClient _client;

	public DockerService() {
		// WARNING: never tested on Windows
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
			_client = new DockerClientConfiguration(
					new Uri("unix:///var/run/docker.sock"))
				.CreateClient();
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
			_client = new DockerClientConfiguration(
					new Uri("npipe://./pipe/docker_engine"))
				.CreateClient();
		}
	}

	public async Task<IList<ContainerListResponse>> list_containers()
		=> await _client.Containers.ListContainersAsync(
			new ContainersListParameters() { Limit = 10, });

	public async Task start_container(string id)
		=> await _client.Containers.StartContainerAsync(
			id,
			new ContainerStartParameters());

	public async Task stop_container(string id)
		=> await _client.Containers.StopContainerAsync(
			id,
			new ContainerStopParameters());

	public async Task remove_container(string id)
		=> await _client.Containers.RemoveContainerAsync(
			id, new ContainerRemoveParameters());

	public async Task<IList<ImagesListResponse>> list_images()
		=> await _client.Images.ListImagesAsync(
			new ImagesListParameters());

	public async Task remove_image(string id)
		=> await _client.Images.DeleteImageAsync(
			id.Replace("sha256:", ""),
			new ImageDeleteParameters(),
		default);

	public async Task<VolumesListResponse> list_volumes()
		=> await _client.Volumes.ListAsync();
}
