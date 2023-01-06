using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/AI/AI Possible Positions Agglomerate", fileName = "SO _ AI Possible Positions Agglomerate")]
public class AIPossiblePositionsAgglomerateSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    public AIPossiblePositionsSO frontClosePositions;
    public AIPossiblePositionsSO frontMidPositions;
    public AIPossiblePositionsSO backPositions;
}
