using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//can only write enum
public enum GroundMapType { Wall, Ground,Fragile,Null=128 };
public enum PathPointType { Obstacle, MidAir, GroundBelow, LeftEdge, RightEdge, SoleGround };

//playerBrain
public enum CommandType { Jump,Attack};
//AIBrain
public enum AICondition { Patrol,Attack};
//
public enum TileMapKind { Map,Wall,Decorate};

//
public enum FindPathType { Jump,Fly};

