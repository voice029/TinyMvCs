namespace TinyMvCs.MvLib
{
    public interface IMvBinderEntry
    {
        void Bind();
        void UnBind();
    }

    public interface IMvBinderStateBase
    {
        void Bind();
        void Unbind();
    
        T AddEntry<T>(T entry, bool autoBind = true) where T : IMvBinderEntry;
    
        bool RemoveEntry(IMvBinderEntry entry);
    }

    public class MvBinderState : IMvBinderStateBase
    {
        private class MvBinderEntry : IMvBinderEntry
        {
            private Action OnActivate;
            private Action OnDeactivate;

            public MvBinderEntry(Action activation, Action deactivation)
            {
                SetOnActivate(activation);
                SetOnDeactivate(deactivation);
            }
        
            public void SetOnActivate(Action activation)
            {
                OnActivate = activation;
            }
        
            public void SetOnDeactivate(Action deactivation)
            {
                OnDeactivate = deactivation;
            }

            public void Bind()
            {
                OnActivate?.Invoke();
            }

            public void UnBind()
            {
                OnDeactivate?.Invoke();
            }
        }
    
        private readonly List<IMvBinderEntry> _entries = new();
    
        public IReadOnlyList<IMvBinderEntry> Entries => _entries.AsReadOnly();
  
        public T AddEntry<T>(T entry, bool autoBind = true) where T : IMvBinderEntry
        {
            _entries.Add(entry);
        
            if (autoBind)
            {
                entry.Bind();
            }
        
            return entry;
        }

        public bool RemoveEntry(IMvBinderEntry mvBinderEntry)
        {
            return _entries.Remove(mvBinderEntry);
        }
    
        public IMvBinderEntry AddEntry(Action onActivate, Action onDeactivate)
        {
            MvBinderEntry entry = new(onActivate, onDeactivate);
            AddEntry(entry);
            return entry;
        }
    
        public void Bind()
        {
            foreach (var entry in Entries)
            {
                entry.Bind();
            }
        }

        public void Unbind()
        {
            foreach (var entry in Entries)
            {
                entry.UnBind();
            }
        }
    }

    public class MvBinderStateChangePayload<TState>
    {
        public MvBinderStateChangePayload(
            TState previousState, TState newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }
    
        public TState PreviousState { get; }
        public TState NewState { get;  }
    }


    public interface IMvBinderBase
    {
        IMvBinderStateBase GetBinderState();
    }

    public class MvBinder<TState> : IMvBinderBase where TState : notnull
    {
        public delegate void OnStateChangeHandler(MvBinderStateChangePayload<TState> newState);
        private Dictionary<TState, IMvBinderStateBase> _stateToBinderState = new();
        private TState _currentState;
        private IMvBinderStateBase _currentBinderState;
        private OnStateChangeHandler _stateChangeHandler;

    
        public MvBinder(TState state)
        {
            _stateChangeHandler = _ => { };
            _currentState = state;
            _currentBinderState = new MvBinderState();
            _stateToBinderState[_currentState] = _currentBinderState;
        }
    
        public IMvBinderStateBase this[TState state]
        {
            get => GetBinder(state);
        }
    
        OnStateChangeHandler SetStateChangeHandler(OnStateChangeHandler handler)
        {
            _stateChangeHandler = handler;
            return _stateChangeHandler;
        }
    
        public TState GetState()
        {
            return _currentState;
        }

        public IMvBinderStateBase GetBinderState() => _currentBinderState;

        public IMvBinderStateBase GetBinder(TState state)
        {
            IMvBinderStateBase binderState;
            if (!_stateToBinderState.TryGetValue(state, out binderState))
            {
                MvBinderState mvBinderState = new MvBinderState();
                _stateToBinderState.Add(state, mvBinderState);
                binderState = mvBinderState;
            }

            return binderState;
        }
        public IMvBinderStateBase ActivateState(TState state)
        {
            _stateChangeHandler?.Invoke(new MvBinderStateChangePayload<TState>(_currentState, state){});
            GetBinderState().Unbind();
            _currentState = state;
            _currentBinderState = GetBinder(_currentState);
        
            return _currentBinderState;
        }
    }
}