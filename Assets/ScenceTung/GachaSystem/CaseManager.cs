using System.Collections.Generic;
using UnityEngine;

public class CaseManager : MonoBehaviour
{
    [SerializeField] private List<CaseItem> caseItems;
    [SerializeField] private CaseSlot[] caseSlots;
    private void Start()
    {
        PopulateShopItems();
    }

    public void PopulateShopItems()
    {
        for (int i = 0; i < caseItems.Count && i < caseSlots.Length; i++)
        {
            CaseItem caseItem = caseItems[i];
            caseSlots[i].Initallize( caseItem.caseSO,caseItem.price);
            caseSlots[i].gameObject.SetActive(true);

        }
        for (int i = caseItems.Count; i < caseSlots.Length; i++)
        {
            caseSlots[i].gameObject.SetActive(false);
        }
    }
}


[System.Serializable]
public class CaseItem
{
    public CaseSO caseSO;
    public int price;
    
}

