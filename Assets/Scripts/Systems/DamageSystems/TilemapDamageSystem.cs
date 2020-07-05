using System.Collections.Generic;
using Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TilemapDamageSystem))]
public class TilemapDamageSystem : UpdateSystem {
    [SerializeField] 
    private TilemapHealth TilemapHealth;
    
    private Tilemap tilemap;

    private Filter filter;
    private Filter tilemapFilter;

    private Dictionary<string, int> healthDictionary;

    private int[][] cellHealths;

    public override void OnAwake()
    {
        filter = Filter.All.With<DamagedComponent>().With<HealthComponent>().With<TilemapComponent>();
        
        tilemapFilter = Filter.All.With<TilemapComponent>();
    }

    private void InstantiateHealth()
    {
        healthDictionary = new Dictionary<string, int>();
        
        List<string> tiles = TilemapHealth.Tiles;
        List<int> healths = TilemapHealth.Health;

        for (int i = 0; i < tiles.Count && i < healths.Count; i++) {
            healthDictionary.Add(tiles[i], healths[i]);
        }
        
        BoundsInt bounds = tilemap.cellBounds;
        cellHealths = new int[bounds.size.x][];

        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            
            cellHealths[x + tilemap.size.x / 2] = new int[bounds.size.y];
            
            for (int y = bounds.min.y; y < bounds.max.y; y++) {
                for (int z = bounds.min.z; z < bounds.max.z; z++) {
                    Vector3Int p = new Vector3Int(x, y, z);
                    Vector3Int p1 = TilemapMovementSystem.ToPathSystem(tilemap.size, p);
                    
                    TileBase tile = tilemap.GetTile(p);

                    if (tile != null)
                        cellHealths[p1.x][p1.y] = healthDictionary[tile.name];
                    else
                        cellHealths[p1.x][p1.y] = 0;
                }
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in tilemapFilter) {
            tilemap = entity.GetComponent<TilemapComponent>().Tilemap;
            
            if (healthDictionary == null)
                InstantiateHealth();
            
            break;
        }

        if (cellHealths == null)
            return;
        
        var bounds = tilemap.cellBounds;
        
        foreach (var entity in filter) {
            ref var tilemap = ref entity.GetComponent<TilemapComponent>().Tilemap;
            var damaged = entity.GetComponent<DamagedComponent>();
            var bullet = damaged.Bullet;
            var bulletCollider = damaged.BulletCollider;

            var dir = bulletCollider.Collsion2D.relativeVelocity;
            
            var cell = tilemap.WorldToCell(bulletCollider.Collsion2D.contacts[0].point + deltaTime * 5 * dir);

            var cellIndex = TilemapMovementSystem.ToPathSystem(bounds.size, cell);
            
            cellHealths[cellIndex.x][cellIndex.y] -= bullet.Damage;
            
            if (cellHealths[cellIndex.x][cellIndex.y] <= 0)
                tilemap.SetTile(cell, null);

            entity.RemoveComponent<DamagedComponent>();
        }
    }
}