using System.Collections.Generic;
using Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TilemapDamageSystem))]
public class TilemapDamageSystem : UpdateSystem {
    [SerializeField] 
    private Tilemap Tilemap;

    [SerializeField] 
    private TilemapHealth TilemapHealth;
    
    private Filter filter;

    private Dictionary<string, int> healthDictionary;

    private int[][] cellHealths;

    public override void OnAwake()
    {
        filter = Filter.All.With<DamagedComponent>().With<HealthComponent>().With<TilemapComponent>();

        InstantiateHealth();
    }

    private void InstantiateHealth()
    {
        healthDictionary = new Dictionary<string, int>();
        
        List<string> tiles = TilemapHealth.Tiles;
        List<int> healths = TilemapHealth.Health;

        for (int i = 0; i < tiles.Count && i < healths.Count; i++) {
            healthDictionary.Add(tiles[i], healths[i]);
        }
        
        BoundsInt bounds = Tilemap.cellBounds;
        TileBase[] allTiles = Tilemap.GetTilesBlock(bounds);

        cellHealths = new int[bounds.size.x][];
        
        for (int x = 0; x < bounds.size.x; x++) {

            cellHealths[x] = new int[bounds.size.y];
            
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                    cellHealths[x][y] = healthDictionary[tile.name];
                else
                    cellHealths[x][y] = 0;
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (cellHealths == null)
            return;
        
        var bounds = Tilemap.cellBounds;
        
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