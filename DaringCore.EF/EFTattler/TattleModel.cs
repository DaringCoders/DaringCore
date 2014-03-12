using System;

namespace DaringCore.EF.EFTattler
{
    public class TattleModel
    {
        public string ObjectName { get; set; }
        public DateTime ChangeDate { get; set; }
        public string NewObject { get; set; }
        public string OriginalObject { get; set; }
        public string UserId { get; set; }
        public TattleReason Reason { get; set; }
    }

    public enum TattleReason
    {
        Added,
        Modified,
        Deleted,
        ToProduction
    }
}
