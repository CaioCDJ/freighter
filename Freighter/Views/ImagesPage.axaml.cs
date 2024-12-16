using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Freighter.ViewModels;
using ReactiveUI;

namespace Freighter.Views;

public partial class ImagesPage : ReactiveUserControl<ImagesPageViewModel> {
	public ImagesPage() {
		this.WhenActivated(dispoables => { });
		AvaloniaXamlLoader.Load(this);
	}
}

