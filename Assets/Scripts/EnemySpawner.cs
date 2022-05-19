using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Transform _spawnPointsSet;
    [SerializeField] private Transform _enemyLookObject;

    [Header("Enemy head randomizer")]
    [SerializeField] private float _colorHueMin;
    [SerializeField] private float _colorHueMax = 1f;
    [SerializeField] private float _colorSaturationMin = 0.7f;
    [SerializeField] private float _colorSaturationMax = 0.7f;
    [SerializeField] private float _colorValueMin = 0.7f;
    [SerializeField] private float _colorValueMax = 0.7f;

    private List<Enemy> _enemies = new List<Enemy>();
    private int _enemiesNumber;

    public void DestroySpawnEnemies()
    {
        if (_enemies.Count > 0)
        {
            foreach (Enemy enemy in _enemies.Where(enemy => enemy != null))
            {
                enemy.Died -= EnemyOnDied;
                Destroy(enemy.gameObject);
            }
        }
    }

    public void Spawn()
    {
        DestroySpawnEnemies();
        _enemies = new List<Enemy>();

        for (var i = 0; i < _spawnPointsSet.childCount; i++)
        {
            Transform spawnPoint = _spawnPointsSet.GetChild(i);
            Enemy enemy = Instantiate(_enemy, spawnPoint.position, Quaternion.identity);

            var headComponent = enemy.GetComponentInChildren<Head>();
            var rendererComponent = headComponent.GetComponent<Renderer>();
            rendererComponent.material.color = Random.ColorHSV(
                _colorHueMin,
                _colorHueMax,
                _colorSaturationMin,
                _colorSaturationMax,
                _colorValueMin,
                _colorValueMax);
            enemy.transform.LookAt(_enemyLookObject);

            enemy.Died += EnemyOnDied;
            _enemies.Add(enemy);
        }

        _enemiesNumber = _enemies.Count;
        _player.SetInitialRank(_enemiesNumber);
    }

    private void EnemyOnDied(Character character, GameObject lastGameObjectCollision)
    {
        Enemy enemy = _enemies.FirstOrDefault(enemy => enemy == character);

        if (enemy != null)
        {
            _enemies.Remove(enemy);
            enemy.Died -= EnemyOnDied;
            Destroy(enemy.gameObject);
        }

        _enemiesNumber = _enemies.Count;

        if (lastGameObjectCollision != null && lastGameObjectCollision.TryGetComponent(out Enemy enemyKiller))
        {
            enemyKiller.IncreaseSize();
            enemyKiller.DecreaseImpactForce();
        }

        if (_player.IsDied == false)
        {
            _player.DecreaseRank();

            if (lastGameObjectCollision != null && lastGameObjectCollision.TryGetComponent(out Player _))
            {
                _player.IncreaseKill();
                _player.IncreaseSize();
                _player.DecreaseImpactForce();
            }

            if (_enemiesNumber == 0)
            {
                _player.Win();
            }
        }
    }
}