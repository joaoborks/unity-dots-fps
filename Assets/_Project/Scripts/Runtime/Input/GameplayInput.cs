//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/_Project/Data/Input/GameplayInput.inputactions
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

namespace MyFps.Input
{
    public partial class @GameplayInput: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameplayInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameplayInput"",
    ""maps"": [
        {
            ""name"": ""Navigation"",
            ""id"": ""554b0aa5-08a1-4586-9546-68cf63ee7c5f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c6429992-15b7-4887-ad27-1039fc869c21"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e29442e2-acff-421f-9867-11f5002090f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b436e981-d2ae-4c74-bc03-5e06a54967e4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""fec4fedf-ad16-464f-9e20-f55b4a8ae38a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""d6be34d5-96a0-4411-8a3d-ecd4868134b0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""e377cbec-3ab3-4996-9ae8-b4bcca0b8889"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""b721a184-6454-4b1d-8fc6-e82c7794822d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""134c0b60-5349-433d-a6cf-63ff019bd31a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""ec368de8-a586-4dd0-9d1a-dfe2d545973c"",
            ""actions"": [
                {
                    ""name"": ""FreeLook"",
                    ""type"": ""Value"",
                    ""id"": ""561e386a-65b9-42c2-b5c0-d2f871c79eb3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a457ec1e-bdc0-4abc-9019-e0f699ed93b4"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false,invertY=false)"",
                    ""groups"": ""Desktop"",
                    ""action"": ""FreeLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Combat"",
            ""id"": ""63085394-d1f6-4770-8e02-864b0de32df1"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""d7a6eee7-45c7-4f45-b7d4-5756a5e8ac89"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""038cddcd-655b-4e57-a50c-7531dfb3b6ab"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UserInterface"",
            ""id"": ""9f10a8ef-c528-4489-9e15-4c34d110d645"",
            ""actions"": [
                {
                    ""name"": ""ToggleMenu"",
                    ""type"": ""Button"",
                    ""id"": ""efd2453e-208c-4081-8e3d-62214f73958d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5d52ff31-02d8-4793-92f8-4138c95b6b2c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""ToggleMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Desktop"",
            ""bindingGroup"": ""Desktop"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Pointer>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Navigation
            m_Navigation = asset.FindActionMap("Navigation", throwIfNotFound: true);
            m_Navigation_Move = m_Navigation.FindAction("Move", throwIfNotFound: true);
            m_Navigation_Jump = m_Navigation.FindAction("Jump", throwIfNotFound: true);
            // Camera
            m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
            m_Camera_FreeLook = m_Camera.FindAction("FreeLook", throwIfNotFound: true);
            // Combat
            m_Combat = asset.FindActionMap("Combat", throwIfNotFound: true);
            m_Combat_Fire = m_Combat.FindAction("Fire", throwIfNotFound: true);
            // UserInterface
            m_UserInterface = asset.FindActionMap("UserInterface", throwIfNotFound: true);
            m_UserInterface_ToggleMenu = m_UserInterface.FindAction("ToggleMenu", throwIfNotFound: true);
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

        // Navigation
        private readonly InputActionMap m_Navigation;
        private List<INavigationActions> m_NavigationActionsCallbackInterfaces = new List<INavigationActions>();
        private readonly InputAction m_Navigation_Move;
        private readonly InputAction m_Navigation_Jump;
        public struct NavigationActions
        {
            private @GameplayInput m_Wrapper;
            public NavigationActions(@GameplayInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Navigation_Move;
            public InputAction @Jump => m_Wrapper.m_Navigation_Jump;
            public InputActionMap Get() { return m_Wrapper.m_Navigation; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(NavigationActions set) { return set.Get(); }
            public void AddCallbacks(INavigationActions instance)
            {
                if (instance == null || m_Wrapper.m_NavigationActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_NavigationActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }

            private void UnregisterCallbacks(INavigationActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Jump.started -= instance.OnJump;
                @Jump.performed -= instance.OnJump;
                @Jump.canceled -= instance.OnJump;
            }

            public void RemoveCallbacks(INavigationActions instance)
            {
                if (m_Wrapper.m_NavigationActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(INavigationActions instance)
            {
                foreach (var item in m_Wrapper.m_NavigationActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_NavigationActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public NavigationActions @Navigation => new NavigationActions(this);

        // Camera
        private readonly InputActionMap m_Camera;
        private List<ICameraActions> m_CameraActionsCallbackInterfaces = new List<ICameraActions>();
        private readonly InputAction m_Camera_FreeLook;
        public struct CameraActions
        {
            private @GameplayInput m_Wrapper;
            public CameraActions(@GameplayInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @FreeLook => m_Wrapper.m_Camera_FreeLook;
            public InputActionMap Get() { return m_Wrapper.m_Camera; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
            public void AddCallbacks(ICameraActions instance)
            {
                if (instance == null || m_Wrapper.m_CameraActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CameraActionsCallbackInterfaces.Add(instance);
                @FreeLook.started += instance.OnFreeLook;
                @FreeLook.performed += instance.OnFreeLook;
                @FreeLook.canceled += instance.OnFreeLook;
            }

            private void UnregisterCallbacks(ICameraActions instance)
            {
                @FreeLook.started -= instance.OnFreeLook;
                @FreeLook.performed -= instance.OnFreeLook;
                @FreeLook.canceled -= instance.OnFreeLook;
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
        public CameraActions @Camera => new CameraActions(this);

        // Combat
        private readonly InputActionMap m_Combat;
        private List<ICombatActions> m_CombatActionsCallbackInterfaces = new List<ICombatActions>();
        private readonly InputAction m_Combat_Fire;
        public struct CombatActions
        {
            private @GameplayInput m_Wrapper;
            public CombatActions(@GameplayInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Fire => m_Wrapper.m_Combat_Fire;
            public InputActionMap Get() { return m_Wrapper.m_Combat; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CombatActions set) { return set.Get(); }
            public void AddCallbacks(ICombatActions instance)
            {
                if (instance == null || m_Wrapper.m_CombatActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CombatActionsCallbackInterfaces.Add(instance);
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
            }

            private void UnregisterCallbacks(ICombatActions instance)
            {
                @Fire.started -= instance.OnFire;
                @Fire.performed -= instance.OnFire;
                @Fire.canceled -= instance.OnFire;
            }

            public void RemoveCallbacks(ICombatActions instance)
            {
                if (m_Wrapper.m_CombatActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ICombatActions instance)
            {
                foreach (var item in m_Wrapper.m_CombatActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_CombatActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public CombatActions @Combat => new CombatActions(this);

        // UserInterface
        private readonly InputActionMap m_UserInterface;
        private List<IUserInterfaceActions> m_UserInterfaceActionsCallbackInterfaces = new List<IUserInterfaceActions>();
        private readonly InputAction m_UserInterface_ToggleMenu;
        public struct UserInterfaceActions
        {
            private @GameplayInput m_Wrapper;
            public UserInterfaceActions(@GameplayInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @ToggleMenu => m_Wrapper.m_UserInterface_ToggleMenu;
            public InputActionMap Get() { return m_Wrapper.m_UserInterface; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UserInterfaceActions set) { return set.Get(); }
            public void AddCallbacks(IUserInterfaceActions instance)
            {
                if (instance == null || m_Wrapper.m_UserInterfaceActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_UserInterfaceActionsCallbackInterfaces.Add(instance);
                @ToggleMenu.started += instance.OnToggleMenu;
                @ToggleMenu.performed += instance.OnToggleMenu;
                @ToggleMenu.canceled += instance.OnToggleMenu;
            }

            private void UnregisterCallbacks(IUserInterfaceActions instance)
            {
                @ToggleMenu.started -= instance.OnToggleMenu;
                @ToggleMenu.performed -= instance.OnToggleMenu;
                @ToggleMenu.canceled -= instance.OnToggleMenu;
            }

            public void RemoveCallbacks(IUserInterfaceActions instance)
            {
                if (m_Wrapper.m_UserInterfaceActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IUserInterfaceActions instance)
            {
                foreach (var item in m_Wrapper.m_UserInterfaceActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_UserInterfaceActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public UserInterfaceActions @UserInterface => new UserInterfaceActions(this);
        private int m_DesktopSchemeIndex = -1;
        public InputControlScheme DesktopScheme
        {
            get
            {
                if (m_DesktopSchemeIndex == -1) m_DesktopSchemeIndex = asset.FindControlSchemeIndex("Desktop");
                return asset.controlSchemes[m_DesktopSchemeIndex];
            }
        }
        public interface INavigationActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
        }
        public interface ICameraActions
        {
            void OnFreeLook(InputAction.CallbackContext context);
        }
        public interface ICombatActions
        {
            void OnFire(InputAction.CallbackContext context);
        }
        public interface IUserInterfaceActions
        {
            void OnToggleMenu(InputAction.CallbackContext context);
        }
    }
}
