using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inixe.InixDialogs.Testing
{
	public class SimpleTestMediator : IDialogMediator
	{
		public void ShowDialog<TState>(Action<TState, object> nextAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
		{
			throw new NotImplementedException();
		}

		public void ShowDialog<TState>(Action<TState, object> yesAction, Action<TState, object> noAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
		{
			throw new NotImplementedException();
		}
	}
}
