using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public Texture2D cursorTexture;

    public CursorMode cursorMode = CursorMode.ForceSoftware;
    public Vector2 hotSpot = Vector2.zero;
    private void Start()
	{
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
