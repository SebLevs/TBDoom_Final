using UnityEngine;

[HelpURL("ww.google.com")]
public class RoomEnemySubscriber : MonoBehaviour
{
    // SECTION - Field =========================================================
    private RoomEnemyManager myRoomEnemyManager;


    // SECTION - Method =========================================================
    #region Unity
    private void Start()
    {
        myRoomEnemyManager = GetComponentInParent<RoomEnemyManager>();
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
    #endregion

    #region Subscriber
    private void Subscribe()
    {
        if (myRoomEnemyManager)
            myRoomEnemyManager.Subscribe(gameObject);
    }

    private void Unsubscribe()
    {
        if (myRoomEnemyManager)
            myRoomEnemyManager.Unsubscribe(gameObject);
    }
    #endregion
}
