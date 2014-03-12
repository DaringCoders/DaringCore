using System.Collections.Generic;

namespace DaringCore.EF.EFTattler
{
    public interface ITattleStorage
    {
        bool AddTattle(TattleModel model);

        TattleModel GetLastTattle(string objectName);

        IEnumerable<TattleModel> GetTattles(string objectName, int skip = 0, int take = 20);
    }
}
