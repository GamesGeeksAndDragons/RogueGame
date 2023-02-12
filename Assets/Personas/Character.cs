#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Personas
{
    public interface ICharacter : IDispatched, IObservable<PositionObservation>, IDisposable
    {
        Parameters CurrentState();
        int ArmourClass { get; set; }
        int HitPoints { get; set; }
        Coordinate Coordinates { get; set; }
    }

    internal abstract class Character<T> : Dispatched<T>, ICharacter
        where T : class, IDispatched
    {
        private readonly List<IObserver<PositionObservation>> _observers = new List<IObserver<PositionObservation>>();

        protected Character(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state) 
            : base(dispatchRegistry, actionRegistry, actor)
        {
            var extracted = state.ToParameters();

            // ReSharper disable once VirtualMemberCallInConstructor
            UpdateState(extracted);
        }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(Coordinates))) Coordinates = state.ToValue<Coordinate>(nameof(Coordinates));
            if (state.HasValue(nameof(HitPoints))) HitPoints = state.ToValue<int>(nameof(HitPoints));
            if (state.HasValue(nameof(ArmourClass))) ArmourClass = state.ToValue<int>(nameof(ArmourClass));

            base.UpdateState(state);
        }

        public override Parameters CurrentState()
        {
            var state = base.CurrentState();

            state.AppendParameter(nameof(Coordinates), Coordinates);
            if (!IsZero(HitPoints)) state.AppendParameter(nameof(HitPoints), HitPoints);
            if (!IsZero(ArmourClass)) state.AppendParameter(nameof(ArmourClass), ArmourClass);

            return state;
        }

        public int ArmourClass { get; set; }
        public int HitPoints { get; set; }

        private Coordinate _coordinates = Coordinate.NotSet;
        public Coordinate Coordinates
        {
            get => _coordinates;
            set
            {
                var before = _coordinates;
                _coordinates = value;

                foreach (var observer in _observers)
                {
                    observer.OnNext(new PositionObservation(UniqueId, (before, Coordinates)) );
                }
            }
        }

        protected internal override void RegisterActions()
        {
            ActionRegistry.RegisterAction(this, Deed.Teleport);
            ActionRegistry.RegisterAction(this, Deed.Move);
            ActionRegistry.RegisterAction(this, Deed.Strike);

            base.RegisterActions();
        }

        internal class UnsubscribePositionObservation<TPO> : IDisposable
        {
            private readonly List<IObserver<TPO>> _observers;
            private readonly IObserver<TPO> _observer;

            internal UnsubscribePositionObservation(List<IObserver<TPO>> observers, IObserver<TPO> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public IDisposable Subscribe(IObserver<PositionObservation> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            observer.OnNext(new PositionObservation(UniqueId, (Coordinate.NotSet, Coordinates)) );

            return new UnsubscribePositionObservation<PositionObservation>(_observers, observer);
        }

        public void Dispose()
        {
            if (_observers.Count != 0)
            {
                foreach (var observer in _observers)
                {
                    observer.OnCompleted();
                }
            }

            _observers.Clear();
        }
    }
}
