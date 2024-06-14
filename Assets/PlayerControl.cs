//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/PlayerControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControl: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""ddbc45b4-cfc1-41a6-8490-3e1721a87807"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cac4a18d-0943-403b-ad6d-c704bf99838e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""aa7fcd39-154e-4812-a264-a7d381a7ec5d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lock On Left Target"",
                    ""type"": ""Button"",
                    ""id"": ""4fbc70d3-1f7d-40f0-b29b-61ddb3e34edc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lock On Right Target"",
                    ""type"": ""Button"",
                    ""id"": ""3a68a95d-c6fa-4ea2-b7ef-71891cef9fdf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""9c54f2f9-b113-4fbe-930f-e199a2965615"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c9754dc8-4c20-4c03-9dca-8d2e1c8efd56"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""48373e3b-8c2f-4e7d-be23-cf9e4a0b0dd6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""806071d7-2a04-47f3-be96-76c2fb036631"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9ba9a657-0d4f-49c7-a027-700d846f7c8c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c5c27ea3-773d-4fe1-a5e8-de0e98b0ed67"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b407b4e-d8bb-423b-a6ed-2f1b15c37d59"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock On Left Target"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96c45b8f-708c-4f0e-a347-6bfdcb531e38"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock On Right Target"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player Actions"",
            ""id"": ""38a441b1-2116-4be5-ad2c-6be0b689b003"",
            ""actions"": [
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""229fab32-62f0-4f2c-9ae4-a78862b316b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftAttack"",
                    ""type"": ""Button"",
                    ""id"": ""f62cfafb-0639-40b9-8ef8-f8f2fab1034d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Critical Attack"",
                    ""type"": ""Button"",
                    ""id"": ""f3fbd9d9-70d2-4d05-8974-d41683cdc108"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""37c1d928-ac24-43c2-b754-9e3d0b0d598a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""6e5a14d2-2707-4333-8c45-8f67013115b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""d35f2ac8-c9ab-4b22-a369-a0ec8fcb1075"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LockOn"",
                    ""type"": ""Button"",
                    ""id"": ""5cb48eaa-754f-451d-9d03-55ebe41bc67c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Guard"",
                    ""type"": ""Button"",
                    ""id"": ""841c4245-af4b-448f-a0a0-183f902484f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ee187c86-b198-482d-a712-2974538d8010"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34e9ad0d-d1af-45cb-9061-aa6cc4b97b42"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff40ed0b-ab9f-4bea-9244-466fd54d1b40"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a59efe05-78e1-4301-bdd1-e48e6c6c7214"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42f8cfae-db72-4c37-b0cf-adbdcf586564"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62a03187-61e8-4b91-92f5-f4eecc2360b9"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1175d6c0-a621-4375-a3a0-de219a4f469c"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f968ff2f-3559-4632-8b6f-f1f018bd5682"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Critical Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e45b270c-0ae3-49cc-a2e8-9a03714a2082"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cbc0228c-41c9-4110-8149-0e3c15bcb99c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8d4717b-d2d6-4ca9-a5f3-b3ce41045745"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Guard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerQuickSlots"",
            ""id"": ""3605e4cf-bcb9-4107-b22c-3a5953a116cb"",
            ""actions"": [
                {
                    ""name"": ""D-Pad Up"",
                    ""type"": ""Button"",
                    ""id"": ""5325247f-673e-4332-aa7d-6d553253a98a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""D-Pad Down"",
                    ""type"": ""Button"",
                    ""id"": ""95b91e94-780b-45b5-9389-14d118a3be3c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""D-Pad Left"",
                    ""type"": ""Button"",
                    ""id"": ""dce81087-1fa8-46ce-86a7-5a108a82852d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""D-Pad Right"",
                    ""type"": ""Button"",
                    ""id"": ""b199981c-7415-493d-841d-53ec5b073fba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1ff6a7b6-36a5-4c11-9f09-864cb877928a"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fad4ea74-6327-4258-844d-31eae3a2e95f"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd974334-7129-4ba9-ba28-ffdcd29595df"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8ad6774-6e22-418f-bd02-0608ed16802b"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75474261-0e6f-4631-bd10-5cc680f0dc1b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c794b637-813c-4c4d-8420-32b4d2d6b464"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc87bc2a-0291-4e23-83f8-7c50b6ec8c1b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""357f2384-4c43-4cdf-9ccb-ee1f19f6d52d"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-Pad Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        m_PlayerMovement_Camera = m_PlayerMovement.FindAction("Camera", throwIfNotFound: true);
        m_PlayerMovement_LockOnLeftTarget = m_PlayerMovement.FindAction("Lock On Left Target", throwIfNotFound: true);
        m_PlayerMovement_LockOnRightTarget = m_PlayerMovement.FindAction("Lock On Right Target", throwIfNotFound: true);
        // Player Actions
        m_PlayerActions = asset.FindActionMap("Player Actions", throwIfNotFound: true);
        m_PlayerActions_Roll = m_PlayerActions.FindAction("Roll", throwIfNotFound: true);
        m_PlayerActions_LeftAttack = m_PlayerActions.FindAction("LeftAttack", throwIfNotFound: true);
        m_PlayerActions_CriticalAttack = m_PlayerActions.FindAction("Critical Attack", throwIfNotFound: true);
        m_PlayerActions_RightAttack = m_PlayerActions.FindAction("RightAttack", throwIfNotFound: true);
        m_PlayerActions_Jump = m_PlayerActions.FindAction("Jump", throwIfNotFound: true);
        m_PlayerActions_Inventory = m_PlayerActions.FindAction("Inventory", throwIfNotFound: true);
        m_PlayerActions_LockOn = m_PlayerActions.FindAction("LockOn", throwIfNotFound: true);
        m_PlayerActions_Guard = m_PlayerActions.FindAction("Guard", throwIfNotFound: true);
        // PlayerQuickSlots
        m_PlayerQuickSlots = asset.FindActionMap("PlayerQuickSlots", throwIfNotFound: true);
        m_PlayerQuickSlots_DPadUp = m_PlayerQuickSlots.FindAction("D-Pad Up", throwIfNotFound: true);
        m_PlayerQuickSlots_DPadDown = m_PlayerQuickSlots.FindAction("D-Pad Down", throwIfNotFound: true);
        m_PlayerQuickSlots_DPadLeft = m_PlayerQuickSlots.FindAction("D-Pad Left", throwIfNotFound: true);
        m_PlayerQuickSlots_DPadRight = m_PlayerQuickSlots.FindAction("D-Pad Right", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private List<IPlayerMovementActions> m_PlayerMovementActionsCallbackInterfaces = new List<IPlayerMovementActions>();
    private readonly InputAction m_PlayerMovement_Movement;
    private readonly InputAction m_PlayerMovement_Camera;
    private readonly InputAction m_PlayerMovement_LockOnLeftTarget;
    private readonly InputAction m_PlayerMovement_LockOnRightTarget;
    public struct PlayerMovementActions
    {
        private @PlayerControl m_Wrapper;
        public PlayerMovementActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputAction @Camera => m_Wrapper.m_PlayerMovement_Camera;
        public InputAction @LockOnLeftTarget => m_Wrapper.m_PlayerMovement_LockOnLeftTarget;
        public InputAction @LockOnRightTarget => m_Wrapper.m_PlayerMovement_LockOnRightTarget;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Camera.started += instance.OnCamera;
            @Camera.performed += instance.OnCamera;
            @Camera.canceled += instance.OnCamera;
            @LockOnLeftTarget.started += instance.OnLockOnLeftTarget;
            @LockOnLeftTarget.performed += instance.OnLockOnLeftTarget;
            @LockOnLeftTarget.canceled += instance.OnLockOnLeftTarget;
            @LockOnRightTarget.started += instance.OnLockOnRightTarget;
            @LockOnRightTarget.performed += instance.OnLockOnRightTarget;
            @LockOnRightTarget.canceled += instance.OnLockOnRightTarget;
        }

        private void UnregisterCallbacks(IPlayerMovementActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Camera.started -= instance.OnCamera;
            @Camera.performed -= instance.OnCamera;
            @Camera.canceled -= instance.OnCamera;
            @LockOnLeftTarget.started -= instance.OnLockOnLeftTarget;
            @LockOnLeftTarget.performed -= instance.OnLockOnLeftTarget;
            @LockOnLeftTarget.canceled -= instance.OnLockOnLeftTarget;
            @LockOnRightTarget.started -= instance.OnLockOnRightTarget;
            @LockOnRightTarget.performed -= instance.OnLockOnRightTarget;
            @LockOnRightTarget.canceled -= instance.OnLockOnRightTarget;
        }

        public void RemoveCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // Player Actions
    private readonly InputActionMap m_PlayerActions;
    private List<IPlayerActionsActions> m_PlayerActionsActionsCallbackInterfaces = new List<IPlayerActionsActions>();
    private readonly InputAction m_PlayerActions_Roll;
    private readonly InputAction m_PlayerActions_LeftAttack;
    private readonly InputAction m_PlayerActions_CriticalAttack;
    private readonly InputAction m_PlayerActions_RightAttack;
    private readonly InputAction m_PlayerActions_Jump;
    private readonly InputAction m_PlayerActions_Inventory;
    private readonly InputAction m_PlayerActions_LockOn;
    private readonly InputAction m_PlayerActions_Guard;
    public struct PlayerActionsActions
    {
        private @PlayerControl m_Wrapper;
        public PlayerActionsActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Roll => m_Wrapper.m_PlayerActions_Roll;
        public InputAction @LeftAttack => m_Wrapper.m_PlayerActions_LeftAttack;
        public InputAction @CriticalAttack => m_Wrapper.m_PlayerActions_CriticalAttack;
        public InputAction @RightAttack => m_Wrapper.m_PlayerActions_RightAttack;
        public InputAction @Jump => m_Wrapper.m_PlayerActions_Jump;
        public InputAction @Inventory => m_Wrapper.m_PlayerActions_Inventory;
        public InputAction @LockOn => m_Wrapper.m_PlayerActions_LockOn;
        public InputAction @Guard => m_Wrapper.m_PlayerActions_Guard;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Add(instance);
            @Roll.started += instance.OnRoll;
            @Roll.performed += instance.OnRoll;
            @Roll.canceled += instance.OnRoll;
            @LeftAttack.started += instance.OnLeftAttack;
            @LeftAttack.performed += instance.OnLeftAttack;
            @LeftAttack.canceled += instance.OnLeftAttack;
            @CriticalAttack.started += instance.OnCriticalAttack;
            @CriticalAttack.performed += instance.OnCriticalAttack;
            @CriticalAttack.canceled += instance.OnCriticalAttack;
            @RightAttack.started += instance.OnRightAttack;
            @RightAttack.performed += instance.OnRightAttack;
            @RightAttack.canceled += instance.OnRightAttack;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Inventory.started += instance.OnInventory;
            @Inventory.performed += instance.OnInventory;
            @Inventory.canceled += instance.OnInventory;
            @LockOn.started += instance.OnLockOn;
            @LockOn.performed += instance.OnLockOn;
            @LockOn.canceled += instance.OnLockOn;
            @Guard.started += instance.OnGuard;
            @Guard.performed += instance.OnGuard;
            @Guard.canceled += instance.OnGuard;
        }

        private void UnregisterCallbacks(IPlayerActionsActions instance)
        {
            @Roll.started -= instance.OnRoll;
            @Roll.performed -= instance.OnRoll;
            @Roll.canceled -= instance.OnRoll;
            @LeftAttack.started -= instance.OnLeftAttack;
            @LeftAttack.performed -= instance.OnLeftAttack;
            @LeftAttack.canceled -= instance.OnLeftAttack;
            @CriticalAttack.started -= instance.OnCriticalAttack;
            @CriticalAttack.performed -= instance.OnCriticalAttack;
            @CriticalAttack.canceled -= instance.OnCriticalAttack;
            @RightAttack.started -= instance.OnRightAttack;
            @RightAttack.performed -= instance.OnRightAttack;
            @RightAttack.canceled -= instance.OnRightAttack;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Inventory.started -= instance.OnInventory;
            @Inventory.performed -= instance.OnInventory;
            @Inventory.canceled -= instance.OnInventory;
            @LockOn.started -= instance.OnLockOn;
            @LockOn.performed -= instance.OnLockOn;
            @LockOn.canceled -= instance.OnLockOn;
            @Guard.started -= instance.OnGuard;
            @Guard.performed -= instance.OnGuard;
            @Guard.canceled -= instance.OnGuard;
        }

        public void RemoveCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);

    // PlayerQuickSlots
    private readonly InputActionMap m_PlayerQuickSlots;
    private List<IPlayerQuickSlotsActions> m_PlayerQuickSlotsActionsCallbackInterfaces = new List<IPlayerQuickSlotsActions>();
    private readonly InputAction m_PlayerQuickSlots_DPadUp;
    private readonly InputAction m_PlayerQuickSlots_DPadDown;
    private readonly InputAction m_PlayerQuickSlots_DPadLeft;
    private readonly InputAction m_PlayerQuickSlots_DPadRight;
    public struct PlayerQuickSlotsActions
    {
        private @PlayerControl m_Wrapper;
        public PlayerQuickSlotsActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @DPadUp => m_Wrapper.m_PlayerQuickSlots_DPadUp;
        public InputAction @DPadDown => m_Wrapper.m_PlayerQuickSlots_DPadDown;
        public InputAction @DPadLeft => m_Wrapper.m_PlayerQuickSlots_DPadLeft;
        public InputAction @DPadRight => m_Wrapper.m_PlayerQuickSlots_DPadRight;
        public InputActionMap Get() { return m_Wrapper.m_PlayerQuickSlots; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerQuickSlotsActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerQuickSlotsActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterfaces.Add(instance);
            @DPadUp.started += instance.OnDPadUp;
            @DPadUp.performed += instance.OnDPadUp;
            @DPadUp.canceled += instance.OnDPadUp;
            @DPadDown.started += instance.OnDPadDown;
            @DPadDown.performed += instance.OnDPadDown;
            @DPadDown.canceled += instance.OnDPadDown;
            @DPadLeft.started += instance.OnDPadLeft;
            @DPadLeft.performed += instance.OnDPadLeft;
            @DPadLeft.canceled += instance.OnDPadLeft;
            @DPadRight.started += instance.OnDPadRight;
            @DPadRight.performed += instance.OnDPadRight;
            @DPadRight.canceled += instance.OnDPadRight;
        }

        private void UnregisterCallbacks(IPlayerQuickSlotsActions instance)
        {
            @DPadUp.started -= instance.OnDPadUp;
            @DPadUp.performed -= instance.OnDPadUp;
            @DPadUp.canceled -= instance.OnDPadUp;
            @DPadDown.started -= instance.OnDPadDown;
            @DPadDown.performed -= instance.OnDPadDown;
            @DPadDown.canceled -= instance.OnDPadDown;
            @DPadLeft.started -= instance.OnDPadLeft;
            @DPadLeft.performed -= instance.OnDPadLeft;
            @DPadLeft.canceled -= instance.OnDPadLeft;
            @DPadRight.started -= instance.OnDPadRight;
            @DPadRight.performed -= instance.OnDPadRight;
            @DPadRight.canceled -= instance.OnDPadRight;
        }

        public void RemoveCallbacks(IPlayerQuickSlotsActions instance)
        {
            if (m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerQuickSlotsActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerQuickSlotsActions @PlayerQuickSlots => new PlayerQuickSlotsActions(this);
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnLockOnLeftTarget(InputAction.CallbackContext context);
        void OnLockOnRightTarget(InputAction.CallbackContext context);
    }
    public interface IPlayerActionsActions
    {
        void OnRoll(InputAction.CallbackContext context);
        void OnLeftAttack(InputAction.CallbackContext context);
        void OnCriticalAttack(InputAction.CallbackContext context);
        void OnRightAttack(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnLockOn(InputAction.CallbackContext context);
        void OnGuard(InputAction.CallbackContext context);
    }
    public interface IPlayerQuickSlotsActions
    {
        void OnDPadUp(InputAction.CallbackContext context);
        void OnDPadDown(InputAction.CallbackContext context);
        void OnDPadLeft(InputAction.CallbackContext context);
        void OnDPadRight(InputAction.CallbackContext context);
    }
}