namespace STARTD.Game.Economy
{
    public interface IEconomy
    {
        int Cur { get; }
        int Max { get; }
        bool Add(int amount);
        bool Remove(int amount, out bool IsZero);
    }
}
