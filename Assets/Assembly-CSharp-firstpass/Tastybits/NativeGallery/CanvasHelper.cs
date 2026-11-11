using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tastybits.NativeGallery
{
	public class CanvasHelper
	{
		public static Canvas GetCanvas(bool createIfNotExists = true)
		{
			Canvas canvas = null;
			Canvas[] array = Object.FindObjectsOfType<Canvas>();
			Canvas[] array2 = array;
			foreach (Canvas canvas2 in array2)
			{
				if (canvas2.enabled && canvas2.name == "Canvas" && canvas2.gameObject.activeInHierarchy)
				{
					canvas = canvas2;
					break;
				}
			}
			if (canvas == null)
			{
				Canvas[] array3 = array;
				foreach (Canvas canvas3 in array3)
				{
					if (canvas3.enabled)
					{
						canvas = canvas3;
						break;
					}
				}
			}
			if (canvas == null && createIfNotExists)
			{
				Debug.Log("MobyShop: Canvas not found - creating one.");
				GameObject gameObject = new GameObject("Canvas");
				canvas = gameObject.AddComponent<Canvas>();
				canvas.pixelPerfect = false;
				canvas.sortingOrder = 0;
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
				canvasScaler.scaleFactor = 1f;
				canvasScaler.referencePixelsPerUnit = 100f;
				canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
				GraphicRaycaster graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
				graphicRaycaster.ignoreReversedGraphics = true;
				graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
				gameObject.AddComponent<RectTransform>();
				if (Object.FindObjectOfType<EventSystem>() == null)
				{
					GameObject gameObject2 = new GameObject("EventSystem");
					gameObject2.AddComponent<EventSystem>();
					gameObject2.AddComponent<StandaloneInputModule>();
				}
			}
			return canvas;
		}
	}
}
