using System.Collections.Generic;
using UnityEngine;

public class CaseScroll : MonoBehaviour
{
    [Header("ScrollView")]
    [SerializeField]
    private GameObject _prefabs;
    private float speed;
    private bool _isScrolling;
    public int scrollSpeed;
    public int countCell;
   public bool hasKey = false;

    private List<CaseCell> _cells = new List<CaseCell>();
    public void Scroll()
    {
        if (!hasKey) return;
        if (_isScrolling)
        {
            return;
        }
        hasKey = false; // Xài xong key thì reset

        GetComponent<RectTransform>().localPosition = new Vector2(1010,0);

        speed = Random.Range(4, 5);
        _isScrolling = true;
        if (_cells.Count == 0)
        {
            for (int i = 0; i < countCell; i++)
            {
                _cells.Add(Instantiate(_prefabs, transform).GetComponentInChildren<CaseCell>());
            }
        }
        foreach (var cell in _cells)
        {
            cell.SetUp();
        }
    }

    private void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left * 100, speed * Time.deltaTime * scrollSpeed);
        if (speed > 0)
        {
            speed -= Time.deltaTime;

        }
        else
        {
            speed = 0;
            _isScrolling = false;
        }

    }
}
