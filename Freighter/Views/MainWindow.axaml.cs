using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Freighter.ViewModels;
using ReactiveUI;

namespace Freighter.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel> {
	public MainWindow() {
		this.WhenActivated(disposables => { });
		AvaloniaXamlLoader.Load(this);
	}
}
