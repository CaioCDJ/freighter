using System;
using Freighter.ViewModels;
using Freighter.Views;
using ReactiveUI;

namespace Freighter;

public class AppViewLocator : ReactiveUI.IViewLocator{
		public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
		{
			ContainersPageModel context => new ContainersPage { DataContext = context },
			_ => throw new ArgumentOutOfRangeException(nameof(viewModel))
		};
	
}
