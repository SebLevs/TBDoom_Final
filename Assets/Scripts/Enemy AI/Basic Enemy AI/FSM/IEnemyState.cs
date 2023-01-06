using UnityEngine;

public interface IEnemyState
{
    // SECTION - Method - State Specific =================================================================== 
    public void WithoutTokenBehaviour(BasicEnemyContext context);

    public void WithTokenBehaviour(BasicEnemyContext context);


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context);
    public void OnStateUpdate(BasicEnemyContext context);
    public IEnemyState OnStateExit(BasicEnemyContext context);
}
