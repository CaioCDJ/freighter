using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Freighter.ViewModels;
using ReactiveUI;

namespace Freighter.Views;

public partial class ContainersPage : ReactiveUserControl<ContainersPageModel> {
	public ContainersPage() {

		this.WhenActivated(dispoables => { });
		AvaloniaXamlLoader.Load(this);
		//	InitializeComponent();
	}
}

