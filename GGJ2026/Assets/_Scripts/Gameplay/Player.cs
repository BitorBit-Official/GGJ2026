using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int startingLives;
    [SerializeField] Sprite unmasked;
    [SerializeField] Sprite masked;
    int lives; //Private integer holding the current number of lives.
    RectTransform rectTransform;
    bool _isMasked; //Boolean that says whether this player has the mask on or not.
    bool isMasked //This is a property.
    {
        get => _isMasked; //Delegate the getter to isMasked.
        set
        {
            if (_isMasked == value) return; //Do nothing because the bool has not changed
            _isMasked = value; //Set it to the given value
            onMaskEquipChange?.Invoke(); //And then fire the event
        }
    }
    (float current, float getMasked, float cannotMaskFor) cooldown = (2,3, 2); //This is a tuple that holds multiple values for the same variable.
    SpriteRenderer spriteRenderer;

    public event Action onMaskEquipChange;

    private void OnEnable()
    {
        GameplayManager.Instance.onLevelStart += LevelStart; //Subscribe to onLevelStart. must use GameplayManager.Instance to access it.
        onMaskEquipChange += HandleMask; //Subscribe to onMaskEquipChange. Because it's in the same class, no prefix is needed.
        onMaskEquipChange += ToggleMaskSprite; //Subscribe to onMaskEquipChange. Because it's in the same class, no prefix is needed.
    }
    private void OnDisable()
    {
        GameplayManager.Instance.onLevelStart -= LevelStart; //Unsubscribe when disabled
        onMaskEquipChange -= HandleMask; //Ditto
        onMaskEquipChange -= ToggleMaskSprite; //Ditto
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rectTransform = GetComponent<RectTransform>();
    }

    void LevelStart()
    {
        lives = startingLives; //Reset the lives to starting lives
        isMasked = false;
        transform.position = Vector3.zero; //Reset position
    }

    void ToggleMaskSprite()
    {
        spriteRenderer.sprite = isMasked ? //This is a ternary operator. It asks "Is isMasked true or false?" and based on that sets the sprite.
            masked //If isMasked is true
            :
            unmasked; //If isMasked is false
    }

    private void Update()
    {
        if (cooldown.current > 0) cooldown.current -= Time.deltaTime; //Do the cooldown only while it's not 0
        else
        {
            if (isMasked) isMasked = false; //Disable the mask. With the property field and checking this is actually set to true, it should only do it once.
        }
    }

    void HandleMask()
    {
        //This handles the mask logic including cooldowns etc.
        if (isMasked)
        {
            cooldown.current = cooldown.getMasked; //Sets a timer for how long the mask is effective
        }
        else
        {
            cooldown.current = cooldown.cannotMaskFor; //Sets the cooldown, by isMasked being false it assumes it was just set to that thanks to the property field.
        }
    }
}
