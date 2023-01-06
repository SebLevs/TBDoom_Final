using UnityEngine;

// SECTION - Enumeration ============================================================

// Basic Ennemy
#region Basic Enemy
public enum BasicEnemy_Types { HOVERING, GROUNDED }
public enum BasicEnemy_States { ONE, TWO, NULL }
public enum BasicEnemy_AnimationStates { IDDLE, MOVEMENT, ONAWAKE, DEAD, STATE_01_TRANSITION, STATE_01_NOTOKEN, STATE_01_TOKEN, STATE_02_TRANSITION, STATE_02_NOTOKEN, STATE_02_TOKEN }
public enum BasicEnemy_AnimTriggers { DEATH, EXITDEATH, STATE_01_TRANSITION, STATE_01_NOTOKEN, STATE_01_TOKEN, STATE_02_TRANSITION, STATE_02_NOTOKEN, STATE_02_TOKEN }
#endregion


// Base Enemy AI
#region Base Enemy AI
public enum ValidationCheckTypes { CHILDSPECIFIC, ALWAYSVALID, RAYCASTSINGLE, RAYCASTARRAY, OVERLAPSPHERE }
public enum AdditionalWeaponEvent { NONE, ALL, HASCHANGED, FINISHEDRELOADING, HASSHOT, STARTEDRELOADING }
#endregion


// SpawnerEnemy
#region Spawner
public enum SpawnerOrientation { ANYSURFACE, CENTER, FLOOR, CEILING, LEFT, RIGHT, FORWARD, BACKWARD };
#endregion
