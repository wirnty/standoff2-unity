using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public class ExtendedSlider : Field<float>
	{
		private Text _numText;

		private bool _initialized;

		public Slider Slider { get; private set; }

		private void Awake()
		{
			InitIfNeed();
		}

		private void Init()
		{
			_numText = base.transform.Find("NumField").GetComponentInChildren<Text>();
			_numText.text = "";
			Slider = GetComponentInChildren<Slider>();
			Slider.onValueChanged.AddListener(InternalSetValue);
			Slider.value = Slider.minValue;
			_initialized = true;
		}

		private void InitIfNeed()
		{
			if (!_initialized)
			{
				Init();
			}
		}

		protected override void SetValue(float value)
		{
			InitIfNeed();
			Slider.value = value;
		}

		private void InternalSetValue(float value)
		{
			_numText.text = value.ToString("0.00");
			base.SetValue(value);
		}
	}
}
