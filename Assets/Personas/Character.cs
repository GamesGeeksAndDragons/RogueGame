#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Personas;

// https://beej.us/moria/mmspoilers/character.html#attributes

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

    protected Character(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, Coordinate position, int armourClass, int hitPoints)
        : base(dispatchRegistry, actionRegistry, actor)
    {
        Coordinates = position;
        HitPoints = hitPoints;
    }

    public override void UpdateState(Parameters state)
    {
        Coordinates = state.GetCoordinates();
        HitPoints = state.GetHitPoints();
        ArmourClass = state.GetArmourClass();

        base.UpdateState(state);
    }

    public override Parameters CurrentState()
    {
        var state = base.CurrentState();

        state
            .AddCoordinates(Coordinates)
            .AddHitPoints(HitPoints)
            .AddArmourClass(ArmourClass);

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
                observer.OnNext(new PositionObservation(this, (before, Coordinates)));
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
            _observers = observers;
            _observer = observer;
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

        observer.OnNext(new PositionObservation(this, (Coordinate.NotSet, Coordinates)));

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
