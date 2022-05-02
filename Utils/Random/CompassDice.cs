#nullable enable
namespace Utils.Random
{
    internal class CompassDice<T> : ICompassDice<T> where T : struct
    {
        private readonly Die _die;

        public string Name => _die.Name;
        public T Random
        {
            get
            {
                var num = _die.Random - 1;
                num = (int) Math.Pow(2, num);
                return (T)(object)num;
            }
        }

        internal void Populate() => _die.Populate();

        public CompassDice(int max, string loadFolder, Die.RandomiserReset reset=Die.RandomiserReset.None)
        {
            _die = new Die(1, max, loadFolder, reset);
        }

        public void NextTurn()
        {
            _die.NextTurn();
        }
    }
}