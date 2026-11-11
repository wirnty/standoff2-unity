using UnityEngine;
using UnityEngine.UI;

public class UpdateAspectEveryFrame : MonoBehaviour
{
	private void Start()
	{
		UpdateAspect();
	}

	private void Update()
	{
		UpdateAspect();
	}

	private void LateUpdate()
	{
		UpdateAspect();
	}

	private void UpdateAspect()
	{
		AspectRatioFitter component = GetComponent<AspectRatioFitter>();
		if (component == null)
		{
			return;
		}
		Image component2 = GetComponent<Image>();
		float num = 1f;
		if (component2 == null)
		{
			RawImage component3 = GetComponent<RawImage>();
			if (component3.texture == null)
			{
				return;
			}
			num = (float)component3.texture.width / (float)component3.texture.height;
		}
		else
		{
			if (component2.sprite == null)
			{
				return;
			}
			num = (float)component2.sprite.texture.width / (float)component2.sprite.texture.height;
		}
		component.aspectRatio = num;
	}
}
