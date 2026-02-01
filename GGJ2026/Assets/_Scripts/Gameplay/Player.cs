using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] int startingLives;
    [SerializeField] Sprite unmasked;
    [SerializeField] Sprite masked;
    int lives; //Private integer holding the current number of lives.
    Rigidbody2D rb;
    bool _isMasked; //Boolean that says whether this player has the mask on or not.
    public bool isMasked //This is a property.
    {
        get => _isMasked; //Delegate the getter to isMasked.
        set
        {
            if (_isMasked == value) return; //Do nothing because the bool has not changed
            _isMasked = value; //Set it to the given value
            onMaskEquipChange?.Invoke(); //And then fire the event
        }
    }
    (float current, float getMasked, float cannotMaskFor) cooldown = (0,3, 5); //This is a tuple that holds multiple values for the same variable.
    bool maskIsCoolingDown;
    SpriteRenderer spriteRenderer;

    public event Action onMaskEquipChange;
    public event Action<int, int> onLivesChanged;

    InputAction move;
    InputAction interact;
    InputAction mask;
    House examiningHouse;

    readonly Vector3 defaultPosition = new Vector3(991.174438f,543.144531f,0);

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
        rb = GetComponent<Rigidbody2D>();
        move = InputSystem.actions.FindAction("Move");
        interact = InputSystem.actions.FindAction("Interact");
        mask = InputSystem.actions.FindAction("Mask");
        interact.Disable();
    }

    void LevelStart()
    {
        lives = startingLives; //Reset the lives to starting lives
        isMasked = false;
        transform.position = defaultPosition; //Reset position
        onLivesChanged?.Invoke(lives, startingLives);
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
        if (cooldown.current > 0) 
        {
            cooldown.current -= Time.deltaTime; //Do the cooldown only while it's not 0
            if (isMasked) MaskStatus.Instance.maskBar.fillAmount = (cooldown.current / cooldown.getMasked); //Handle only when masked
        }

        else
        {
            if (isMasked)
            {
                isMasked = false; //Disable the mask. With the property field and checking this is actually set to true, it should only do it once.
                maskIsCoolingDown = true; //The mask is cooling down.
            }
            else if (maskIsCoolingDown)
            {
                MaskStatus.Instance.maskBar.fillAmount = 1; //Snap back to full
                maskIsCoolingDown = false; //And the mask is no longer on cooldown
            }
        }

        if (interact.WasPressedThisFrame() && (examiningHouse != null && !examiningHouse.wasChecked))
        {
            examiningHouse.ExamineHouse();
        }

        if (mask.WasPressedThisFrame() && cooldown.current <= 0)
        {
            Debug.Log("Attempt mask");
            isMasked = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveDir = move.ReadValue<Vector2>();
        rb.linearVelocity = moveDir * 200f;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out House house))
        {
            examiningHouse = house;
            interact.Enable();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (examiningHouse != null) //Assumes it's exiting collision with one of the houses
        {
            interact.Disable();
            examiningHouse = null;
        }
    }

    public void LoseLife()
    {
        lives = Mathf.Clamp(lives-1,0,startingLives);
        onLivesChanged?.Invoke(lives, startingLives);
    }

    public void GetLife()
    {
        lives = Mathf.Clamp(lives+1, 0, startingLives);
        onLivesChanged?.Invoke(lives, startingLives);
    }
}



//stvari za dodati: interaction button (kada si blizu zgrade koju želiš otkriti (i dok si blizu da se prikaže) aka npr e ili f), button za cancelanje
//(isti gumb za aktiviranje, kliknem ga opet za 1 sek i onda imam samo 2 sek cooldown ali ga tretira kao da je prazna), zamijeni mask button (m) sa space