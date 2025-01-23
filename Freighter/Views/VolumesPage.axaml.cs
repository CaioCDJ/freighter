using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Freighter.ViewModels;
using ReactiveUI;

namespace Freighter.Views;

public partial class VolumesPage : ReactiveUserControl<VolumesPageViewModel> {
	public VolumesPage() {
		this.WhenActivated(action => { });
		AvaloniaXamlLoader.Load(this);
	}
}

