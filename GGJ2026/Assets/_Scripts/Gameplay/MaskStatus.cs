using UnityEngine;
using UnityEngine.UI;

public class MaskStatus : MonoBehaviour
{
    public static MaskStatus Instance { get; private set; }
    public Sprite activeMaskSprite;
    public Sprite inactiveMaskSprite;
    public Image maskBar;

    private SpriteRenderer maskRenderer;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleMaskGraphic(bool masked)
    {
        maskRenderer.sprite = masked ? activeMaskSprite : inactiveMaskSprite;
    }
}
