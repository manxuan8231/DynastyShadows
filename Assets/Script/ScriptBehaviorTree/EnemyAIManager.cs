using System;
using System.Collections.Generic;
using UnityEngine;
/**
 * 1. Behavior Tree Manager for Enemy AI
 * 2. Game đối kháng (đánh cờ) đánh với máy
 * 3. Tìm đường A* (A* Pathfinding) cho Enemy AI
 * 4. AR Game (AR Game) - Pokemon Go
 */

public class EnemyAIManager : MonoBehaviour
{
    // Singleton
    public static EnemyAIManager Instance { get; private set; }
    
    // danh sách các enemy đang hoạt động
    private List<EnemyAI> _activeEnemies = new List<EnemyAI>();
    
    // biến phân phối Tick qua các frame
    private int _currentEnemyIndexToTick = 0;
    
    // số lượng enemy được tick trong mỗi frame
    public int enemiesToTickPerFrame = 5;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject); // Nếu đã có instance khác, hủy đối tượng này
        }
        else
        {
            Instance = this; // Gán instance hiện tại
            // DontDestroyOnLoad(gameObject); // Giữ instance này không bị hủy khi chuyển cảnh
        }
    }

    private void Start()
    {
        // enemiesToTickPerFrame = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None).Length;
        // // Lấy số lượng enemy hiện tại trong scene
    }

    // hàm để enemyAI tự động đăng ký khi được tạo
    public void RegisterEnemy(EnemyAI enemy)
    {
        if (enemy != null && !_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Add(enemy);
        }
    }
    
    // hàm để enemyAI tự động hủy đăng ký khi enemy die
    public void UnregisterEnemy(EnemyAI enemy)
    {
        if (enemy != null && _activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
            // điều chỉnh lại chỉ số tick nếu cần
            if(_currentEnemyIndexToTick >= _activeEnemies.Count && _activeEnemies.Count > 0)
            {
                _currentEnemyIndexToTick = 0; // reset về đầu danh sách
            }
        }
    }
    // 100 Enemy cùng chạy Evaluate() mỗi frame 60fps
    // 5 enemy tick mỗi frame --> 20 frame mới tick hết 100 enemy
    // mỗi enemy tick 3 lần mỗi giây

    // Update is called once per frame
    void Update()
    {
        if(_activeEnemies.Count == 0)
        {
            return; // không có enemy nào để tick
        }
        // Tick một số lượng enemy nhất định trong mỗi frame
        for (int i = 0; i < enemiesToTickPerFrame; i++)
        {
            if(_activeEnemies.Count == 0)
            {
                break; // không còn enemy nào để tick
            }
            _currentEnemyIndexToTick = (_currentEnemyIndexToTick + 1) % _activeEnemies.Count; // vòng qua danh sách
            var enemy = _activeEnemies[_currentEnemyIndexToTick];
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                enemy.Tick(); // gọi hàm Tick của enemy
            }
            else
            {
                _activeEnemies.RemoveAt(_currentEnemyIndexToTick); // nếu enemy đã bị hủy, xóa khỏi danh sách
                if(_currentEnemyIndexToTick >= _activeEnemies.Count
                   && _activeEnemies.Count > 0)
                {
                    _currentEnemyIndexToTick = 0; // reset về đầu danh sách nếu đã hết
                }

                i--; // giảm i để không bỏ qua tick của enemy tiếp theo
            }
        }
    }
}
