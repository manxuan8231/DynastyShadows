using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseCell : MonoBehaviour
{
    [System.Serializable]
    private class ListOfSprites
    {
        public List<Sprite> sprites;
    }

    [SerializeField]
    private List<ListOfSprites> _sprites;
    [SerializeField]
    private int[] _chance;
    [SerializeField]
    private Color[] _colors;
    


    public void SetUp()
    {
        var index = Randomize();
        GetComponent<Image>().sprite = _sprites[index].sprites[Random.Range(0, _sprites[index].sprites.Count)];
        transform.parent.GetComponent<Image>().color = _colors[index];
    }
    private int Randomize()
    {
        int ind = 0;
        for (int i = 0; i < _chance.Length; i++)
        {

            int rand = Random.Range(0, 100);
            if (rand > _chance[i])
            {
                return i;
            }
            ind++;
        }
        return ind;
    }
}
