using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class GameView : View
	{
		private static readonly Log Log = Log.Create(typeof(GameView));

		private readonly Dictionary<Type, View> _views = new Dictionary<Type, View>();

		private IPlayerPropSensitiveView[] _refreshViews = new IPlayerPropSensitiveView[0];

		private bool _initialized;

		public void Init()
		{
			_initialized = true;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				View component = base.transform.GetChild(i).GetComponent<View>();
				if (component != null)
				{
					_views[component.GetType()] = component;
					component.Show();
					component.Hide();
				}
			}
			_refreshViews = GetComponentsInChildren<IPlayerPropSensitiveView>(true);
		}

		public T GetView<T>() where T : View
		{
			CheckInitialized();
			return (T)_views[typeof(T)];
		}

		public void Refresh()
		{
			Refresh(null);
		}

		public void Refresh(IEnumerable<object> changedProps)
		{
			CheckInitialized();
			IPlayerPropSensitiveView[] refreshViews = _refreshViews;
			foreach (IPlayerPropSensitiveView playerPropSensitiveView in refreshViews)
			{
				if (playerPropSensitiveView.IsVisible)
				{
					string[] sensitivePlayerProperties = playerPropSensitiveView.SensitivePlayerProperties;
					if (changedProps == null || sensitivePlayerProperties.Any((string property) => changedProps.Any((object changedProp) => object.Equals(changedProp, property))))
					{
						playerPropSensitiveView.Refresh();
						Log.Debug(string.Format("{0} updated", playerPropSensitiveView.GetType().Name));
					}
				}
			}
		}

		private void CheckInitialized()
		{
			if (!_initialized)
			{
				throw new InvalidOperationException("GameView not initialzied");
			}
		}
	}
}
