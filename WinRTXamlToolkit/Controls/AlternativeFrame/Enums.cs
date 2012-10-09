namespace WinRTXamlToolkit.Controls
{
    public enum AnimationMode
    {
        In,
        Out,
        ForwardIn,
        ForwardOut,
        BackwardIn,
        BackwardOut,
    }

    public enum AxisOfFlip
    {
        LeftOrTop,
        Center,
        RightOrBottom,
        Random,
    }
    
    public enum DirectionOfMotion
    {
        RightToLeft,
        LeftToRight,
        TopToBottom,
        BottomToTop,
        Random
    }

    public enum NavigationState
    {
        Initializing = 0,
        Preloading,
        Preloaded,
        UnloadingPreloaded,
        UnloadedPreloaded,
        NavigatingTo,
        TransitioningTo,
        NavigatedTo,
        TransitionedTo,
        NavigatingFrom,
        NavigatedFrom,
        TransitioningFrom,
        TransitionedFrom,
    }

    public enum PageTransitionMode
    {
        Sequential,
        Parallel
    }
}