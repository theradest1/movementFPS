//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""movement"",
            ""id"": ""8d7db207-3da2-41f1-a012-4dfe4f275d43"",
            ""actions"": [
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""933aa543-d467-4c44-bb52-592cb11ffe1f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""edc17366-df53-472e-98f3-67056113ae18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""4be22819-e75c-42d1-9375-6bb6c4a00fc1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""2c1fa066-6395-4df8-95d6-7761d74368a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""8a0639bd-8cfd-435e-a5e9-334a20b6c67a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""950c3a9c-009d-4734-a17f-a6d3c33f65d7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f11f654a-10ac-4cc6-9969-c2d3012c2f60"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ecfe07c1-ba7b-47bb-92c9-0dd2ad443304"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""18b59872-0ddb-4877-859d-6c8eeb23c737"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Martin"",
                    ""id"": ""fbee3af8-a298-4369-917f-0baac1134966"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f3b7dc36-da41-418f-bce1-3e026376958a"",
                    ""path"": ""<Mouse>/forwardButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dcd13a3f-43af-4cf0-b727-1ac23cdc20c0"",
                    ""path"": ""<Mouse>/backButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d1c604c3-63a1-454c-92ff-745825563564"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""baa6b6de-9f1c-4cd3-b022-ab972fb7aa90"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""397c1e0e-c2dc-4226-b033-61d6a2cf4343"",
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
                    ""id"": ""bb7b1ad4-4467-42b9-98d0-fec254361706"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a24b07c-ca80-421d-8dc4-d0b2acf216d3"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a7afc51-546c-4b08-a394-e8aa287473c8"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""camera"",
            ""id"": ""d115a201-2f22-4710-9cd7-0913619711e3"",
            ""actions"": [
                {
                    ""name"": ""mouseDelta"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b41a7387-8950-4159-aa3f-2f0433b84bfe"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c85c58bd-a0b2-4fe4-b61d-58e3a166d934"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mouseDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""interactions"",
            ""id"": ""ea087e4d-b92f-4497-8491-bfd6978eb7be"",
            ""actions"": [
                {
                    ""name"": ""shoot"",
                    ""type"": ""Button"",
                    ""id"": ""7d268154-92fe-4518-b535-2ffd7cd61241"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ADS"",
                    ""type"": ""Button"",
                    ""id"": ""826f8676-ab80-4b42-aba6-373a602180f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""reload"",
                    ""type"": ""Button"",
                    ""id"": ""738707ae-8c54-41e4-a027-26d1238abe3a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""escape"",
                    ""type"": ""Button"",
                    ""id"": ""fe59f4c2-fd0f-4556-9b28-ccbc3baf1af4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""tab"",
                    ""type"": ""Button"",
                    ""id"": ""55e4f593-e2c1-4ca3-9da6-2a87a83ffe29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""enter"",
                    ""type"": ""Button"",
                    ""id"": ""f828e8cd-f655-45bc-9176-b3a1ba5d1679"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d89babb7-f1aa-47ff-81a9-7b389749b967"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f839d167-3fd1-48a0-90d1-9b9860b28838"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ADS"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b40ebd78-614b-4b3a-947a-440b469f3b98"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""483043b9-6cbb-4223-b6a4-e46b238ac423"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0afbed0-f617-4d2c-b246-33721de0b6d8"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7304381-9474-4d3d-824c-234510843fa4"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""weaponSelects"",
            ""id"": ""95589fb3-ba7e-4637-b0b8-febfd8636406"",
            ""actions"": [
                {
                    ""name"": ""one"",
                    ""type"": ""Button"",
                    ""id"": ""3b25a810-c6da-4f3c-bc06-6d8acee9d170"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""two"",
                    ""type"": ""Button"",
                    ""id"": ""56dfd741-f8f3-4d4f-afe0-f796e02c8f86"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""three"",
                    ""type"": ""Button"",
                    ""id"": ""d4f4f2d4-9e6a-4fda-94ca-accc51552ec1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""scroll"",
                    ""type"": ""Value"",
                    ""id"": ""47aa3d47-020c-4d04-a756-4cf3934bbc96"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4ab72d2f-c535-46e9-81a6-4312488ef604"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b49f8531-7cb2-4bf1-bbb1-670dee293777"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37e83d31-cd17-41cb-8916-b7c9760b7b05"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""one"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae9df130-cef7-4cca-9252-507f1133b421"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // movement
        m_movement = asset.FindActionMap("movement", throwIfNotFound: true);
        m_movement_Walk = m_movement.FindAction("Walk", throwIfNotFound: true);
        m_movement_Jump = m_movement.FindAction("Jump", throwIfNotFound: true);
        m_movement_Crouch = m_movement.FindAction("Crouch", throwIfNotFound: true);
        m_movement_Sprint = m_movement.FindAction("Sprint", throwIfNotFound: true);
        // camera
        m_camera = asset.FindActionMap("camera", throwIfNotFound: true);
        m_camera_mouseDelta = m_camera.FindAction("mouseDelta", throwIfNotFound: true);
        // interactions
        m_interactions = asset.FindActionMap("interactions", throwIfNotFound: true);
        m_interactions_shoot = m_interactions.FindAction("shoot", throwIfNotFound: true);
        m_interactions_ADS = m_interactions.FindAction("ADS", throwIfNotFound: true);
        m_interactions_reload = m_interactions.FindAction("reload", throwIfNotFound: true);
        m_interactions_escape = m_interactions.FindAction("escape", throwIfNotFound: true);
        m_interactions_tab = m_interactions.FindAction("tab", throwIfNotFound: true);
        m_interactions_enter = m_interactions.FindAction("enter", throwIfNotFound: true);
        // weaponSelects
        m_weaponSelects = asset.FindActionMap("weaponSelects", throwIfNotFound: true);
        m_weaponSelects_one = m_weaponSelects.FindAction("one", throwIfNotFound: true);
        m_weaponSelects_two = m_weaponSelects.FindAction("two", throwIfNotFound: true);
        m_weaponSelects_three = m_weaponSelects.FindAction("three", throwIfNotFound: true);
        m_weaponSelects_scroll = m_weaponSelects.FindAction("scroll", throwIfNotFound: true);
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

    // movement
    private readonly InputActionMap m_movement;
    private List<IMovementActions> m_MovementActionsCallbackInterfaces = new List<IMovementActions>();
    private readonly InputAction m_movement_Walk;
    private readonly InputAction m_movement_Jump;
    private readonly InputAction m_movement_Crouch;
    private readonly InputAction m_movement_Sprint;
    public struct MovementActions
    {
        private @PlayerControls m_Wrapper;
        public MovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Walk => m_Wrapper.m_movement_Walk;
        public InputAction @Jump => m_Wrapper.m_movement_Jump;
        public InputAction @Crouch => m_Wrapper.m_movement_Crouch;
        public InputAction @Sprint => m_Wrapper.m_movement_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void AddCallbacks(IMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
            @Walk.started += instance.OnWalk;
            @Walk.performed += instance.OnWalk;
            @Walk.canceled += instance.OnWalk;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Crouch.started += instance.OnCrouch;
            @Crouch.performed += instance.OnCrouch;
            @Crouch.canceled += instance.OnCrouch;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
        }

        private void UnregisterCallbacks(IMovementActions instance)
        {
            @Walk.started -= instance.OnWalk;
            @Walk.performed -= instance.OnWalk;
            @Walk.canceled -= instance.OnWalk;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Crouch.started -= instance.OnCrouch;
            @Crouch.performed -= instance.OnCrouch;
            @Crouch.canceled -= instance.OnCrouch;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
        }

        public void RemoveCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_MovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MovementActions @movement => new MovementActions(this);

    // camera
    private readonly InputActionMap m_camera;
    private List<ICameraActions> m_CameraActionsCallbackInterfaces = new List<ICameraActions>();
    private readonly InputAction m_camera_mouseDelta;
    public struct CameraActions
    {
        private @PlayerControls m_Wrapper;
        public CameraActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @mouseDelta => m_Wrapper.m_camera_mouseDelta;
        public InputActionMap Get() { return m_Wrapper.m_camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void AddCallbacks(ICameraActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraActionsCallbackInterfaces.Add(instance);
            @mouseDelta.started += instance.OnMouseDelta;
            @mouseDelta.performed += instance.OnMouseDelta;
            @mouseDelta.canceled += instance.OnMouseDelta;
        }

        private void UnregisterCallbacks(ICameraActions instance)
        {
            @mouseDelta.started -= instance.OnMouseDelta;
            @mouseDelta.performed -= instance.OnMouseDelta;
            @mouseDelta.canceled -= instance.OnMouseDelta;
        }

        public void RemoveCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraActions @camera => new CameraActions(this);

    // interactions
    private readonly InputActionMap m_interactions;
    private List<IInteractionsActions> m_InteractionsActionsCallbackInterfaces = new List<IInteractionsActions>();
    private readonly InputAction m_interactions_shoot;
    private readonly InputAction m_interactions_ADS;
    private readonly InputAction m_interactions_reload;
    private readonly InputAction m_interactions_escape;
    private readonly InputAction m_interactions_tab;
    private readonly InputAction m_interactions_enter;
    public struct InteractionsActions
    {
        private @PlayerControls m_Wrapper;
        public InteractionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @shoot => m_Wrapper.m_interactions_shoot;
        public InputAction @ADS => m_Wrapper.m_interactions_ADS;
        public InputAction @reload => m_Wrapper.m_interactions_reload;
        public InputAction @escape => m_Wrapper.m_interactions_escape;
        public InputAction @tab => m_Wrapper.m_interactions_tab;
        public InputAction @enter => m_Wrapper.m_interactions_enter;
        public InputActionMap Get() { return m_Wrapper.m_interactions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InteractionsActions set) { return set.Get(); }
        public void AddCallbacks(IInteractionsActions instance)
        {
            if (instance == null || m_Wrapper.m_InteractionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InteractionsActionsCallbackInterfaces.Add(instance);
            @shoot.started += instance.OnShoot;
            @shoot.performed += instance.OnShoot;
            @shoot.canceled += instance.OnShoot;
            @ADS.started += instance.OnADS;
            @ADS.performed += instance.OnADS;
            @ADS.canceled += instance.OnADS;
            @reload.started += instance.OnReload;
            @reload.performed += instance.OnReload;
            @reload.canceled += instance.OnReload;
            @escape.started += instance.OnEscape;
            @escape.performed += instance.OnEscape;
            @escape.canceled += instance.OnEscape;
            @tab.started += instance.OnTab;
            @tab.performed += instance.OnTab;
            @tab.canceled += instance.OnTab;
            @enter.started += instance.OnEnter;
            @enter.performed += instance.OnEnter;
            @enter.canceled += instance.OnEnter;
        }

        private void UnregisterCallbacks(IInteractionsActions instance)
        {
            @shoot.started -= instance.OnShoot;
            @shoot.performed -= instance.OnShoot;
            @shoot.canceled -= instance.OnShoot;
            @ADS.started -= instance.OnADS;
            @ADS.performed -= instance.OnADS;
            @ADS.canceled -= instance.OnADS;
            @reload.started -= instance.OnReload;
            @reload.performed -= instance.OnReload;
            @reload.canceled -= instance.OnReload;
            @escape.started -= instance.OnEscape;
            @escape.performed -= instance.OnEscape;
            @escape.canceled -= instance.OnEscape;
            @tab.started -= instance.OnTab;
            @tab.performed -= instance.OnTab;
            @tab.canceled -= instance.OnTab;
            @enter.started -= instance.OnEnter;
            @enter.performed -= instance.OnEnter;
            @enter.canceled -= instance.OnEnter;
        }

        public void RemoveCallbacks(IInteractionsActions instance)
        {
            if (m_Wrapper.m_InteractionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInteractionsActions instance)
        {
            foreach (var item in m_Wrapper.m_InteractionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InteractionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InteractionsActions @interactions => new InteractionsActions(this);

    // weaponSelects
    private readonly InputActionMap m_weaponSelects;
    private List<IWeaponSelectsActions> m_WeaponSelectsActionsCallbackInterfaces = new List<IWeaponSelectsActions>();
    private readonly InputAction m_weaponSelects_one;
    private readonly InputAction m_weaponSelects_two;
    private readonly InputAction m_weaponSelects_three;
    private readonly InputAction m_weaponSelects_scroll;
    public struct WeaponSelectsActions
    {
        private @PlayerControls m_Wrapper;
        public WeaponSelectsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @one => m_Wrapper.m_weaponSelects_one;
        public InputAction @two => m_Wrapper.m_weaponSelects_two;
        public InputAction @three => m_Wrapper.m_weaponSelects_three;
        public InputAction @scroll => m_Wrapper.m_weaponSelects_scroll;
        public InputActionMap Get() { return m_Wrapper.m_weaponSelects; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WeaponSelectsActions set) { return set.Get(); }
        public void AddCallbacks(IWeaponSelectsActions instance)
        {
            if (instance == null || m_Wrapper.m_WeaponSelectsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_WeaponSelectsActionsCallbackInterfaces.Add(instance);
            @one.started += instance.OnOne;
            @one.performed += instance.OnOne;
            @one.canceled += instance.OnOne;
            @two.started += instance.OnTwo;
            @two.performed += instance.OnTwo;
            @two.canceled += instance.OnTwo;
            @three.started += instance.OnThree;
            @three.performed += instance.OnThree;
            @three.canceled += instance.OnThree;
            @scroll.started += instance.OnScroll;
            @scroll.performed += instance.OnScroll;
            @scroll.canceled += instance.OnScroll;
        }

        private void UnregisterCallbacks(IWeaponSelectsActions instance)
        {
            @one.started -= instance.OnOne;
            @one.performed -= instance.OnOne;
            @one.canceled -= instance.OnOne;
            @two.started -= instance.OnTwo;
            @two.performed -= instance.OnTwo;
            @two.canceled -= instance.OnTwo;
            @three.started -= instance.OnThree;
            @three.performed -= instance.OnThree;
            @three.canceled -= instance.OnThree;
            @scroll.started -= instance.OnScroll;
            @scroll.performed -= instance.OnScroll;
            @scroll.canceled -= instance.OnScroll;
        }

        public void RemoveCallbacks(IWeaponSelectsActions instance)
        {
            if (m_Wrapper.m_WeaponSelectsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IWeaponSelectsActions instance)
        {
            foreach (var item in m_Wrapper.m_WeaponSelectsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_WeaponSelectsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public WeaponSelectsActions @weaponSelects => new WeaponSelectsActions(this);
    public interface IMovementActions
    {
        void OnWalk(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnMouseDelta(InputAction.CallbackContext context);
    }
    public interface IInteractionsActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnADS(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnTab(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
    }
    public interface IWeaponSelectsActions
    {
        void OnOne(InputAction.CallbackContext context);
        void OnTwo(InputAction.CallbackContext context);
        void OnThree(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
    }
}
