#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A ScriptableObject-based state that includes an embedded StateKey sub-asset.
    /// It auto-generates a sub-asset (hidden) if none is assigned.
    /// </summary>
    public abstract class ScriptableState : ScriptableObject, IState<BaseContext>
    {
        [SerializeField]
        private string displayName;

        [Header("Embedded StateKey (Auto-Created)")]
        [SerializeField, HideInInspector]
        protected StateKey key;
        // We'll hide it in the Inspector, so it doesn't appear as a separate field.

        // The public "Key" property that IState<BaseContext> requires.
        // We return `embeddedKey`.
        public StateKey Key => key;

        public string DisplayName => displayName;

        // Called whenever the ScriptableState is recompiled or changed in the editor.
        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!key)
            {
                // 1) Create a new StateKey in memory.
                key = ScriptableObject.CreateInstance<StateKey>();
                key.name = $"_Key_{this.name}";

                // 2) Make this StateKey a sub-asset of the state file.
                string path = AssetDatabase.GetAssetPath(this);
                if (!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.AddObjectToAsset(key, path);
                    // Optionally hide it in the Project view, so it doesn't clutter:
                    key.hideFlags = HideFlags.HideInHierarchy;

                    // 3) Force an OnValidate in StateKey, so it can generate a GUID.
                    EditorUtility.SetDirty(key);
                    EditorUtility.SetDirty(this);
                    AssetDatabase.ImportAsset(path);
                    Debug.Log($"[{name}] Created embedded StateKey sub-asset: {key.name}");
                }
                else
                {
                    Debug.LogWarning($"[{name}] Could not create sub-asset because '{this}' is not saved yet.");
                }
            }
#endif
        }

        // The three IState<BaseContext> methods:
        public StateKey EnterState(BaseContext context, StateKey fromKey) => OnEnter(context, fromKey);
        public StateKey UpdateState(BaseContext context) => OnUpdate(context);
        public StateKey ExitState(BaseContext context, StateKey toKey) => OnExit(context, toKey);

        // Overridable hooks:
        public virtual StateKey OnEnter(BaseContext context, StateKey fromKey) => key;
        public virtual StateKey OnUpdate(BaseContext context) => key;
        public virtual StateKey OnExit(BaseContext context, StateKey toKey) => key;

        // If you want a “ChangeState()” method to unify transitions:
        public virtual StateKey ChangeState(StateKey newKey) => newKey;
    }
}
