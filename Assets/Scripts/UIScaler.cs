using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gravitime.UI
{
    public class UIScaler : MonoBehaviour
    {
        RectTransform rectTransform;
        Rect safeArea;
        Vector2 minAnchor;
        Vector2 maxAnchor;
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            safeArea = Screen.safeArea;
            minAnchor = safeArea.position;
            maxAnchor = minAnchor + safeArea.size;
            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
        }
    }
}

