using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager : Manager<RandomManager>
{
    private RandomSeed roomGenerationRandom;
    private RandomSeed otherRandom;

    public RandomSeed RoomGenerationRandom { get => roomGenerationRandom; set => roomGenerationRandom = value; }
    public RandomSeed OtherRandom { get => otherRandom; set => otherRandom = value; }

    public void Initialize()
    {
        roomGenerationRandom = new RandomSeed();
        otherRandom = new RandomSeed();
    }
}
