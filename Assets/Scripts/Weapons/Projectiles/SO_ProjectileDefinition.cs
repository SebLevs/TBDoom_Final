using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType { PHYSICAL, RAYCAST, MELEE }

[CreateAssetMenu(menuName = "Scriptable/Projectiles/ProjectileDefinition", fileName = "SO_ProjectileDefinition")]
public class SO_ProjectileDefinition : ScriptableObject
{
    [SerializeField] private ProjectileType m_projectileType;

    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private float m_colliderRadius;
    [SerializeField] private float m_colliderHeight;
    [SerializeField] private bool m_affectedByGravity;
    [SerializeField] private Sprite[] m_physicalProjectileSprites;

    [SerializeField] private bool m_showRaycastTrajectory;

    [SerializeField] private Sprite m_meleeProjectileSprite;
    [SerializeField] private float m_meleeProjectileWidth;
    [SerializeField] private float m_meleeProjectileThickness;

    public ProjectileType ProjectileType { get => m_projectileType; set => m_projectileType = value; }

    public float ProjectileSpeed { get => m_projectileSpeed; set => m_projectileSpeed = value; }
    public float ColliderRadius { get => m_colliderRadius; set => m_colliderRadius = value; }
    public float ColliderHeight { get => m_colliderHeight; set => m_colliderHeight = value; }
    public bool AffectedByGravity { get => m_affectedByGravity; set => m_affectedByGravity = value; }
    public Sprite[] PhysicalProjectileSprites { get => m_physicalProjectileSprites; set => m_physicalProjectileSprites = value; }
    
    public bool ShowRaycastTrajectory { get => m_showRaycastTrajectory; set => m_showRaycastTrajectory = value; }
    
    public Sprite MeleeProjectileSprite { get => m_meleeProjectileSprite; set => m_meleeProjectileSprite = value; }
    public float MeleeProjectileWidth { get => m_meleeProjectileWidth; set => m_meleeProjectileWidth = value; }
    public float MeleeProjectileThickness { get => m_meleeProjectileThickness; set => m_meleeProjectileThickness = value; }
}
