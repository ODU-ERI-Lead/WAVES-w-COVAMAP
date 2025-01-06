using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWireGroup 
{
    List<WireBundleManager.Wire> GetWires(); // Retrieve the wires in the group
    int GetWireCount(); // Get the number of wires in the group
    int wireType();

    public WireBundleManager.WireMaterialType GetWireMaterialType();

}

