#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;

public class StateKey : ScriptableObject, IEquatable<StateKey>
{
    [SerializeField, HideInInspector]
    private string uniqueID;

    public string UniqueID => uniqueID;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = GUID.Generate().ToString();
            EditorUtility.SetDirty(this);
            Debug.Log($"[StateKey] Generated new uniqueID for '{name}': {uniqueID}");
        }
#endif
    }

    // Implement IEquatable<StateKey>
    public bool Equals(StateKey other)
    {
        if (other == null) return false;
        return string.Equals(this.uniqueID, other.uniqueID, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
        if (obj is StateKey otherKey)
            return Equals(otherKey);
        return false;
    }

    public override int GetHashCode()
    {
        // use uniqueID's hash code
        return (uniqueID == null) ? 0 : uniqueID.GetHashCode();
    }

    public static bool operator ==(StateKey left, StateKey right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(StateKey left, StateKey right)
    {
        return !(left == right);
    }
}
