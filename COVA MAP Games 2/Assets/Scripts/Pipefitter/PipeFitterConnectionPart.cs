using FuzzPhyte.Tools.Connections;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PipeFitterConnectionPart : ConnectionPart
{
    public Dictionary<ConnectionPointUnity, List<ConnectionFixed>> AllWeldData { get => WeldsByPoint; }
}
