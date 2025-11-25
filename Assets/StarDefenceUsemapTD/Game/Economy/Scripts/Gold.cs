namespace STARTD.Game.Economy
{
    public class Gold : IEconomy
    {
        public int Cur { get; private set; }
        public int Max { get; private set; }

        public Gold(int firstAmount, int MaxAmount)
        {
            Cur = firstAmount;
            Max = MaxAmount;
        }

        public bool Add(int amount)
        {
            if (amount < 0)
                return false;

            Cur += amount;
            return true;
        }

        public bool Remove(int amount, out bool IsZero)
        {
            IsZero = false;

            if (amount < 0)
                return false;

            if (Cur >= amount)
            {
                Cur -= amount;
                if (Cur <= 0)
                    IsZero = true;
                return true;
            }

            return false;
        }
    }
}
