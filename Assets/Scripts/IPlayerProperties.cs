using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerProperties 
{
    public int FuelCellCount { get; }
    public int ShieldCount { get; }
    public bool IsShielded { get; }
    public bool IsGameOver { get; }

}
