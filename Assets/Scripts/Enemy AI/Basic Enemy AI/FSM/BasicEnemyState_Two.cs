using UnityEngine;

public class BasicEnemyState_Two : IEnemyState
{
    // SECTION - Field ===================================================================
    private bool launchAnimation = false;


    // SECTION - Method - State Specific ===================================================================
    public void WithoutTokenBehaviour(BasicEnemyContext context)
    {
        context.OnDefault_NoToken_Behaviour();

        if (!context.HasToken && context.Behaviour_NoToken_2)
        {
            if (context.CanUseBehaviour)
            {
                if (context.NoTokenHasAnim_2)
                    context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATE_02_NOTOKEN);
                else
                    context.Behaviour_NoToken_2.Execute();
            }
        }
    }

    public void WithTokenBehaviour(BasicEnemyContext context)
    {
        context.OnDefault_Token_Behaviour();

        if (context.HasToken &&
            context.CanUseBehaviour &&
           !context.IsWeaponReloading() &&
            context.IsTargetNear() &&
            context.IsIddleOrMoving() )
        {
            if ((context.Behaviour_Token_2 != null && context.Behaviour_Token_2.IsExecutionValid()) || context.IsTargetNear())
                context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATE_02_TOKEN);
        }
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context)
    {
        if (context.WeaponManager_2 != null)
            context.SetEndReachedDistance(context.WeaponManager_2.Weapon.Range);
    }

    public void OnStateUpdate(BasicEnemyContext context)
    {
        WithoutTokenBehaviour(context);
        WithTokenBehaviour(context);
    }

    public IEnemyState OnStateExit(BasicEnemyContext context)
    {
        // Dead
        if (context.MyLivingEntity.IsDead)
            return new EnemyStateDead();

        return this;
    }
}
