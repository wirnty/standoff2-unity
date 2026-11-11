using UnityEngine;

namespace Axlebolt.Standoff.TouchControl
{
	public class TouchData
	{
		public Vector3 position;

		public float time = 0f;

		public TouchPhase touchPhase;

		public int fingerId = -1;

		public int id = -1;

		public bool isAffected = false;

		public RectTransform interactiveArea = null;

		public Vector3 positionConverted
		{
			get
			{
				float num = position.x / (float)Screen.width;
				float num2 = position.y / (float)Screen.height;
				float num3 = interactiveArea.rect.yMax - interactiveArea.rect.yMin;
				float num4 = interactiveArea.rect.xMax - interactiveArea.rect.xMin;
				Vector3 vector = new Vector3(num4 * num, num3 * num2, 0f);
				Vector3 vector2 = new Vector3(num4 * interactiveArea.pivot.x, num3 * interactiveArea.pivot.y, 0f);
				return vector - vector2;
			}
			private set
			{
			}
		}

		public TouchData Clone()
		{
			return (TouchData)MemberwiseClone();
		}
	}
}
