using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class LevelLoadingView : View
	{
		private const int SettingsMaxCount = 10;

		[NotNull]
		[SerializeField]
		private Image _levelImage;

		[NotNull]
		[SerializeField]
		private Text _levelNameText;

		[NotNull]
		[SerializeField]
		private Image _levelSchemaImage;

		[NotNull]
		[SerializeField]
		private Text _gameModeText;

		[NotNull]
		[SerializeField]
		private Text _gameModeDescriptionText;

		[NotNull]
		[SerializeField]
		private TextView _gameModeSettingPrefab;

		[NotNull]
		[SerializeField]
		private Text _loadingText;

		[NotNull]
		[SerializeField]
		private Image _progressBar;

		public float Progress
		{
			set
			{
				_progressBar.fillAmount = value;
			}
		}

		public LevelDefinition Level
		{
			set
			{
				_levelNameText.text = value.DisplayName;
				_levelSchemaImage.sprite = value.SchemaImage;
				_levelImage.sprite = value.Image;
			}
		}

		public GameMode GameMode
		{
			set
			{
				_gameModeText.text = value.LocalizedName;
				_gameModeDescriptionText.text = value.LocalizedDescription;
			}
		}

		public string LoadingText
		{
			set
			{
				_loadingText.text = value;
			}
		}

		private void Awake()
		{
		}

		public override void Show()
		{
			base.Show();
			Progress = 0f;
		}
	}
}
