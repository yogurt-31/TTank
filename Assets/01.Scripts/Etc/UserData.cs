using System;
using Unity.Collections;

[Serializable]
public struct UserData
{
    public ulong clientID;
    public FixedString32Bytes playerName;
    public int characterIndex;
}