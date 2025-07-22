using UnityEngine;

[CreateAssetMenu(fileName = "CaseSO", menuName = "ScriptableObjects/CaseSO", order = 1)]
public class CaseSO : ScriptableObject
{
    public Sprite caseImage;
    public string itemName;
    public int id;
}
