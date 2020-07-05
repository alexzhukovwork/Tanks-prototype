using System;
using System.Collections.Generic;
using Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TilemapMovementSystem))]
public class TilemapMovementSystem : UpdateSystem
{
    [SerializeField] 
    private Tilemap Tilemap;

    private float speed = 0.01f;
    
    private Filter filter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<PathFinderComponent>().With<TransformComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (Tilemap == null) {
            Filter tilemapFilter = Filter.All.With<TilemapHealthComponent>().With<TilemapComponent>();

            if (tilemapFilter.Length > 0)
                Tilemap = tilemapFilter.GetEntity(0).GetComponent<TilemapComponent>().Tilemap;
        }
        
        var size = Tilemap.size;
        PathFind.Grid grid = GetPathFindGrid();
        
        foreach (var entity in filter) {
            var pathFinder = entity.GetComponent<PathFinderComponent>();
            var transform = entity.GetComponent<TransformComponent>();

            var start = Tilemap.WorldToCell(transform.Transform.position);
            var target = Tilemap.WorldToCell(pathFinder.Target.position);

            start = ToPathSystem(size, start);
            target = ToPathSystem(size, target);
            
            PathFind.Point _from = new PathFind.Point(start.x, start.y);
            PathFind.Point _to = new PathFind.Point(target.x, target.y);

            List<PathFind.Point> path = PathFind.Pathfinding.FindPath(grid, _from, _to);

            ref var pathComponent = ref entity.GetComponent<PathComponent>();
            pathComponent.CurrentPoint = (Vector2Int) start;
            pathComponent.Distance = path.Count;

            if (path.Count > 0) {
                pathComponent.NextPoint = new Vector2Int(path[0].x, path[0].y);
            }
            else {
                pathComponent.NextPoint = (Vector2Int) target;
            }
        }
    }

    private PathFind.Grid GetPathFindGrid()
    {
        var size = Tilemap.size;
            
        bool[,] tilesmap = new bool[size.x, size.y];
        
        BoundsInt bounds = Tilemap.cellBounds;
        TileBase[] allTiles = Tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) {

            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];

                tilesmap[x, y] = tile == null;
            }
        }
        
        PathFind.Grid grid = new PathFind.Grid(size.x, size.y, tilesmap);

        return grid;
    }
    
    public static Vector3Int ToPathSystem(Vector3Int size, Vector3Int data)
    {
        data.x += size.x / 2;
        data.y += size.y / 2;

        data.x = Math.Min(data.x, size.x - 1);
        data.x = Math.Max(0, data.x);
        
        data.y = Math.Min(data.y, size.y - 1);
        data.y = Math.Max(0, data.y);
        
        return data;
    }

    private Vector3Int ToTilemapSystem(Vector3Int size, Vector3Int data)
    {
        data.x -= size.x / 2;
        data.y -= size.y / 2;

        return data;
    }
}