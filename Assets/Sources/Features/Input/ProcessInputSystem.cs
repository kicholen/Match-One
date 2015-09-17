using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ProcessInputSystem : IReactiveSystem, ISetPool {
    public TriggerOnEvent trigger { get { return Matcher.Input.OnEntityAdded(); } }

    Pool _pool;

    public void SetPool(Pool pool) {
        _pool = pool;
    }

    public void Execute(List<Entity> entities) {
        Debug.Log("Process Input");

        var inputEntity = entities.SingleEntity();
        var input = inputEntity.input;
        destroyGroup(input.x, input.y, "", true);
        _pool.DestroyEntity(inputEntity);
    }

    bool destroyGroup(int x, int y, string resourceName = "", bool isFirstEnter = false) {
        bool wasDestroyed = false;
        if (x < 8 && x >= 0 && y < 9 && y >= 0) {
            Entity e = _pool.gameBoardCache.grid[x, y];
            if (e != null) {
                string resName = e.resource.name;

                if ((resName.Equals(resourceName) || resourceName.Equals("")) && destroyBrickIfCan(e, isFirstEnter)) {
                    //e.isDestroy = true;
                    int count = 0;
                    count += destroyGroup(x + 1, y, resName) ? 1 : 0;
                    count += destroyGroup(x - 1, y, resName) ? 1 : 0;
                    count += destroyGroup(x, y + 1, resName) ? 1 : 0;
                    count += destroyGroup(x, y - 1, resName) ? 1 : 0;
                    if (isFirstEnter && count > 0) {
                        e.isDestroy = true;
                    }
                    wasDestroyed = true;
                }
            }
        }
        return wasDestroyed;
    }

    bool destroyBrickIfCan(Entity e, bool isFirstEnter) {
        if (e != null && e.isInteractive && !e.isDestroy) {
            if (!isFirstEnter) {
                e.isDestroy = true;
            }
            return true;
        }
        return false;
    }
}

