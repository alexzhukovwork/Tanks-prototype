using System;
using System.Collections.Generic;
using Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TilemapMovementSystem))]
public class TilemapMovementSystem : UpdateSystem
{
    private Tilemap tilemap;

    private float speed = 0.01f;
    
    private Filter filter;
    private Filter tilemapFilter;
    
    public override void OnAwake()
    {
        filter = Filter.All.With<PathFinderComponent>().With<TransformComponent>();

        tilemapFilter = Filter.All.With<TilemapComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in tilemapFilter) {
            tilemap = entity.GetComponent<TilemapComponent>().Tilemap;
            break;
        }
        
        var size = tilemap.size;
        PathFind.Grid grid = GetPathFindGrid();
        
        foreach (var entity in filter) {
            var pathFinder = entity.GetComponent<PathFinderComponent>();
            var transform = entity.GetComponent<TransformComponent>();

            var start = tilemap.WorldToCell(transform.Transform.position);
            var target = tilemap.WorldToCell(pathFinder.Target.position);

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
        var size = tilemap.size;
            
        bool[,] tilesmap = new bool[size.x, size.y];
        
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {
                for (int z = bounds.min.z; z < bounds.max.z; z++) {
                    Vector3Int p = new Vector3Int(x, y, z);
                    Vector3Int p1 = ToPathSystem(tilemap.size, p);
                    
                    tilesmap[p1.x, p1.y] = tilemap.GetTile(p) == null;
                }
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
}