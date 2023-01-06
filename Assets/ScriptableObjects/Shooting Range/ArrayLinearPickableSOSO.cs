using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Scriptable/Data Structure/Array Linear PickableSOs", fileName = "SO_myArrayLinearPickableSOs")]
public class ArrayLinearPickableSOSO : ScriptableObject, IArrayLinear
{
    // SECTION - Field ===================================================================
    private int count = 0;
    private int currentIndex = 0;
    [SerializeField] private PickableSO[] myArray;


    // SECTION - Property ===================================================================

    public PickableSO[] GetArray { get => myArray; }


    // SECTION - Method - Interface Implementation ===================================================================
    public int Count { get => count; }
    public int CurrentIndex { get => currentIndex; }

    public int Length => myArray.Length;

    public bool IsNull => myArray == null;
    public bool IsEmpty => count == 0;
    public bool IsFull => count == myArray.Length;


    // SECTION - Method - Unity Specific ===================================================================
    private void OnEnable()
    {
        //currentIndex = 0;
        CalculateCount();
    }


    // SECTION - Method - Data Structure Specific ===================================================================
    public void AddLength(int length = 1)
    {
        PickableSO[] temp = new PickableSO[myArray.Length + length];

        if (Length != 0)
            for (int i = 0; i < myArray.Length; i++)
                temp[i] = myArray[i];

        myArray = temp;
    }

    public PickableSO GetPrevious()
    {
        if (currentIndex - 1 >= 0 && myArray[currentIndex - 1] != null)
        {
            currentIndex--;
            return myArray[currentIndex];
        }

        return myArray[currentIndex];
    }

    public PickableSO GetNext()
    {
        if (currentIndex + 1 <= count - 1 && myArray[currentIndex + 1] != null)
        {
            currentIndex++;
            return myArray[currentIndex];
        }

        return myArray[currentIndex];
    }

    public void Copy(PickableSO[] copyFrom)
    {
        if (copyFrom == null)
            return;

        count = 0;

        myArray = copyFrom;

        foreach (PickableSO item in myArray)
            if (item != null)
                count++;
    }

    public void CalculateCount()
    {
        count = 0;

        if (myArray != null)
        {
            for (int i = 0; i < myArray.Length; i++)
                if (myArray[i] != null)
                    count++;
        }
    }

    public void Sort_BackToBack(ref int sortFrom)
    {
        if (myArray == null || sortFrom >= myArray.Length)
            return;

        if (myArray[sortFrom] != null)
        {
            for (int i = 0; i < count; i++)
            {
                if (myArray[i] == null)
                {
                    myArray[i] = myArray[sortFrom];
                    myArray[sortFrom] = null;
                }
            }
        }

        sortFrom++;
        Sort_BackToBack(ref sortFrom);
    }

    public PickableSO GetElement(int index)
    {
        return myArray[index];
    }

    public void Add(PickableSO item)
    {
        if (count < myArray.Length)
            for (int i = 0; i < myArray.Length; i++)
                if (myArray[i] == null)
                {
                    myArray[i] = item;
                    count++;
                    return;
                }
    }

    public void AddUnique(PickableSO item)
    {
        if (Length == 0)
        {
            AddLength();
            myArray[0] = item;
        }

        bool canAdd = true;
        bool thereWasNull = false;

        foreach (PickableSO weaponSO in myArray)
        {
            if (weaponSO == null)
            {
                thereWasNull = true;
                continue;
            }

            if (item.GetInstanceID() == weaponSO.GetInstanceID())
            {
                canAdd = false;
                break;
            }
        }

        if (canAdd)
        {
            if (thereWasNull)
                Add(item);
            else if (count != 0)
            {
                AddLength();

                myArray[count] = item;
                count++;
            }
        }
    }

    public void ChangeAt(PickableSO item, int index)
    {
        if (index < myArray.Length)
            myArray[index] = item;
    }

    public void Remove()
    {
        if (myArray != null)
            myArray[count--] = null;
    }

    public void RemoveAt(int removeAt, bool alsoSort = false)
    {
        if (removeAt < count)
        {
            if (!alsoSort)
            {
                myArray[removeAt] = null;
                count--;
                return;
            }

            // Recursive not used: Bellow is faster
            PickableSO[] temp = new PickableSO[myArray.Length];

            for (int i = 0; i < removeAt; i++)
                temp[i] = myArray[i];

            for (int i = removeAt + 1; i < myArray.Length; i++)
                temp[i] = myArray[i];

            myArray = temp;
            count--;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < myArray.Length; i++)
            if (myArray[i] != null)
                myArray[i] = null;
    }

    public void Reset(bool keepItemZero = false)
    {
        int resetWithQty = keepItemZero ? 1 : 0;

        PickableSO itemZero = myArray[0];

        myArray = new PickableSO[resetWithQty];

        if (keepItemZero)
            myArray[0] = itemZero;

        CalculateCount();
    }

    public void Debugger()
    {
        foreach (PickableSO item in myArray)
        {
            Debug.Log($"myArray.item.name = {item}");
        }
    }
}
