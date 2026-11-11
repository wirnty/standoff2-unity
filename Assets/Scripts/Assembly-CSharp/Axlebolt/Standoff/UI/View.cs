using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public class View : MonoBehaviour
	{
		public delegate void VisibleChangedHandler(bool isVisible);

		public bool IsVisible
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
			set
			{
				base.gameObject.SetActive(value);
				OnVisibleChanged(value);
			}
		}

		public event VisibleChangedHandler VisibleChanged;

		public virtual void Hide()
		{
			IsVisible = false;
		}

		public virtual void Show()
		{
			IsVisible = true;
		}

		protected virtual void OnVisibleChanged(bool isvisible)
		{
			if (this.VisibleChanged != null)
			{
				this.VisibleChanged(isvisible);
			}
		}
	}
}
