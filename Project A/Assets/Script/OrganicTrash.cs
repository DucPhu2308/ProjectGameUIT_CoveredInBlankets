using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganicTrash : Trash
{
    TrashType type = TrashType.Organic;
}
public enum TrashType
{
    None, Organic, Plastic
}
