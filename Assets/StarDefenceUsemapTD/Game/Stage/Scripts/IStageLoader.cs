using UnityEngine.Tilemaps;

namespace STARTD.Core.Stage
{
    public interface IStageLoader
    {
        Stage LoadStage(Tilemap tiles);
    }
}