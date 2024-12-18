using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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
	
	public ReactiveCommand<string, Unit> delete_command { get; }
	
	public ObservableCollection<Image> images { get; set; }
	
	public ImagesPageViewModel (IScreen hostScreen) {
		HostScreen = hostScreen;
		_docker_client = new DockerClientConfiguration(
				new Uri("unix:///var/run/docker.sock"))
			.CreateClient();
		/*
		for(int i=0;i<10;i++) 
			images_data.Add(new Image() {
				id = "oal",
				name = "postgres",
				size = 3993,
				repo_tag = "latest",
				containers = 2
			});
		images = new ObservableCollection<Image>(images_data);*/
		var images_data =  _docker_client.Images.ListImagesAsync(
			new ImagesListParameters( )
		).Result;

		var lst = new List<Image>();
		foreach (var item in images_data) {
			lst.Add(new Image() {
				id = item.ID,
				size = item.Size,
				repo_tag = item.RepoTags[0],
				name = item.RepoTags[0].Replace(":latest",""),
				delete = ReactiveCommand.Create<string>(onDeleteButtonClicked),
				created_at = item.Created.ToString("dd/MM/yyyy HH:mm"),
				
			});
			
		}
		images = new ObservableCollection<Image>(lst);

		//delete_command = ReactiveCommand.Create<string>(onDeleteButtonClicked);
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
				id.Replace("sha256:","") ,
				new ImageDeleteParameters(), 
				default);
			images.Remove(images.SingleOrDefault(x=>x.id == id));
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
		
		
	}
}
