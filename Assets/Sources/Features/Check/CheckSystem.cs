using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class CheckSystem : IReactiveSystem, ISetPool {
    public TriggerOnEvent trigger { get { return Matcher.GameBoardElement.OnEntityRemoved(); } }

    Pool _pool;

    public void SetPool(Pool pool) {
        _pool = pool;
    }

    public void Execute(List<Entity> entities) {
        Debug.Log("Check");

        var gameBoard = _pool.gameBoard;
        var grid = _pool.gameBoardCache.grid;
        for (int column = 0; column < gameBoard.columns; column++) {
            for (int row = 0; row < gameBoard.rows; row++) {
                Entity e = _pool.gameBoardCache.grid[column, row];

                if (e != null && !e.isDestroy) {
                    string resName = e.resource.name;
                    if (column != gameBoard.columns - 1) {
                        Entity entityRight = _pool.gameBoardCache.grid[column + 1, row];
                        if (entityRight != null && entityRight.isInteractive && !entityRight.isDestroy && entityRight.resource.name.Equals(e.resource.name)) {
                            return;
                        }
                    }
                    if (column != 0) {
                        Entity entityLeft = _pool.gameBoardCache.grid[column - 1, row];
                        if (entityLeft != null && entityLeft.isInteractive && !entityLeft.isDestroy && entityLeft.resource.name.Equals(e.resource.name)) {
                            return;
                        }
                    }
                    if (row != 0) {
                        Entity entityTop = _pool.gameBoardCache.grid[column, row - 1];
                        if (entityTop != null && entityTop.isInteractive && !entityTop.isDestroy && entityTop.resource.name.Equals(e.resource.name)) {
                            return;
                        }
                    }
                    if (row != gameBoard.rows - 1) {
                        Entity entityBottom = _pool.gameBoardCache.grid[column, row + 1];
                        if (entityBottom != null && entityBottom.isInteractive && !entityBottom.isDestroy && entityBottom.resource.name.Equals(e.resource.name)) {
                            return;
                        }
                    }
                }
            }
        }
        
        bool isBroken = true;
        do {
            int x = Random.Range(0, gameBoard.rows - 1);
            int y = Random.Range(0, gameBoard.columns - 1);

            Entity e = _pool.gameBoardCache.grid[x, y];
            if (e != null && e.isInteractive && !e.isDestroy) {
                if (x != gameBoard.columns - 1) {
                    Entity entityRight = _pool.gameBoardCache.grid[x + 1, y];
                    if (entityRight != null && !entityRight.isDestroy && entityRight.isInteractive) {
                        entityRight.ReplaceResource(e.resource.name);
                        isBroken = false;
                    }
                }
                if (x != 0) {
                    Entity entityLeft = _pool.gameBoardCache.grid[x - 1, y];
                    if (entityLeft != null && !entityLeft.isDestroy && entityLeft.isInteractive) {
                        entityLeft.ReplaceResource(e.resource.name);
                        isBroken = false;
                    }
                }
                if (y != 0) {
                    Entity entityTop = _pool.gameBoardCache.grid[x, y - 1];
                    if (entityTop != null && !entityTop.isDestroy && entityTop.isInteractive) {
                        entityTop.ReplaceResource(e.resource.name);
                        isBroken = false;
                    }
                }
                if (y != gameBoard.rows - 1) {
                    Entity entityBottom = _pool.gameBoardCache.grid[x, y + 1];
                    if (entityBottom != null && !entityBottom.isDestroy && entityBottom.isInteractive) {
                        entityBottom.ReplaceResource(e.resource.name);
                        isBroken = false;
                    }
                }
            }
        } while (isBroken);
    }
}
