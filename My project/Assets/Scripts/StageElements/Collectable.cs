using UnityEngine;

public class Collectable : MonoBehaviour
{
    bool _collected;

    public bool Collected { get { return _collected; } set { _collected = value; } }
}
