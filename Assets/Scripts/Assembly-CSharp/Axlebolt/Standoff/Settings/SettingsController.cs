using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings
{
	public class SettingsController : View
	{
		[SerializeField]
		[NotNull]
		private Text _titleText;

		[NotNull]
		[SerializeField]
		private Button _backButton;

		[NotNull]
		[SerializeField]
		private Button _restoreDefaultsButton;

		[NotNull]
		[SerializeField]
		private Button _applyButton;

		[SerializeField]
		[NotNull]
		private VideoSettingsController _videoSettingsController;

		[SerializeField]
		[NotNull]
		private ControlSettingsController _controlSettingsController;

		private SettingsTabController _tabController;

		public static SettingsController Create(Transform parent)
		{
			GameObject original = ResourcesUtility.Load<GameObject>("UI/SettingsController");
			SettingsController requireComponent = Object.Instantiate(original, parent, false).GetRequireComponent<SettingsController>();
			requireComponent.Show();
			requireComponent.Hide();
			return requireComponent;
		}

		private void Awake()
		{
			_videoSettingsController.ChangedHandler = OnChanged;
			_controlSettingsController.ChangedHandler = OnChanged;
			_videoSettingsController.Show();
			_videoSettingsController.Hide();
			_controlSettingsController.Show();
			_controlSettingsController.Hide();
			_backButton.onClick.AddListener(Back);
			_restoreDefaultsButton.onClick.AddListener(RestoreDefaults);
			_applyButton.onClick.AddListener(Apply);
		}

		public void ShowVideoSettings()
		{
			ShowTab(_videoSettingsController);
		}

		public void ShowControlSettings()
		{
			ShowTab(_controlSettingsController);
		}

		private void ShowTab(SettingsTabController tabController)
		{
			Show();
			_applyButton.interactable = false;
			tabController.Show();
			_tabController = tabController;
			_tabController.InitValues();
			_titleText.text = _tabController.Title;
		}

		private void OnChanged()
		{
			_applyButton.interactable = true;
		}

		private void Back()
		{
			Hide();
		}

		private void RestoreDefaults()
		{
			_tabController.RestoreDefaults();
		}

		private void Apply()
		{
			_tabController.Apply();
			_applyButton.interactable = false;
		}

		public override void Hide()
		{
			base.Hide();
			if ((object)_tabController != null)
			{
				_tabController.Hide();
			}
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				Back();
			}
		}
	}
}
