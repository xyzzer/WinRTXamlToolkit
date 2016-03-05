namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Animation mode.
    /// </summary>
    public enum AnimationMode
    {
        /// <summary>
        /// Animation used to show target.
        /// </summary>
        In,
        /// <summary>
        /// Animation used to hide target.
        /// </summary>
        Out,
        /// <summary>
        /// Animation used to show target during forward navigation.
        /// </summary>
        ForwardIn,
        /// <summary>
        /// Animation used to hide target during forward navigation.
        /// </summary>
        ForwardOut,
        /// <summary>
        /// Animation used to show target during backward navigation.
        /// </summary>
        BackwardIn,
        /// <summary>
        /// Animation used to hide target during backward navigation.
        /// </summary>
        BackwardOut,
    }

    /// <summary>
    /// Axis position for flip transition.
    /// </summary>
    public enum AxisOfFlip
    {
        /// <summary>
        /// Left or top edge axis.
        /// </summary>
        LeftOrTop,
        /// <summary>
        /// Axis that goes through the center of the animation target.
        /// </summary>
        Center,
        /// <summary>
        /// Right or bottom axis.
        /// </summary>
        RightOrBottom,
        /// <summary>
        /// Randomly selected axis.
        /// </summary>
        Random,
    }
    
    /// <summary>
    /// Direction of motion for animation.
    /// </summary>
    public enum DirectionOfMotion
    {
        /// <summary>
        /// Right to left transition animation.
        /// </summary>
        RightToLeft,
        /// <summary>
        /// left to right transition animation.
        /// </summary>
        LeftToRight,
        /// <summary>
        /// Top to bottom transition animation.
        /// </summary>
        TopToBottom,
        /// <summary>
        /// Bottom to top transition animation.
        /// </summary>
        BottomToTop,
        /// <summary>
        /// Randomly selected transition animation direction of motion.
        /// </summary>
        Random
    }

    /// <summary>
    /// State of the state machine of AlternativeFrame/AlternativePage.
    /// </summary>
    public enum NavigationState
    {
        /// <summary>
        /// The page is initializing.
        /// </summary>
        Initializing = 0,
        /// <summary>
        /// The page is preloading content
        /// </summary>
        Preloading,
        /// <summary>
        /// The page has preloaded content
        /// </summary>
        Preloaded,
        /// <summary>
        /// The page is unloading preloaded content
        /// </summary>
        UnloadingPreloaded,
        /// <summary>
        /// The page has unloaded preloaded content
        /// </summary>
        UnloadedPreloaded,
        /// <summary>
        /// The page is being navigated to
        /// </summary>
        NavigatingTo,
        /// <summary>
        /// The page is being transitioned to
        /// </summary>
        TransitioningTo,
        /// <summary>
        /// The page has been navigated to
        /// </summary>
        NavigatedTo,
        /// <summary>
        /// The page has been transitioned to
        /// </summary>
        TransitionedTo,
        /// <summary>
        /// The page is being navigating from
        /// </summary>
        NavigatingFrom,
        /// <summary>
        /// The page has been navigated from
        /// </summary>
        NavigatedFrom,
        /// <summary>
        /// The page is being transitioned from
        /// </summary>
        TransitioningFrom,
        /// <summary>
        /// The page has been transitioned from
        /// </summary>
        TransitionedFrom,
    }

    /// <summary>
    /// Determines whether page transition animations should occur sequentially or parallel/concurrently on both pages.
    /// </summary>
    public enum PageTransitionMode
    {
        /// <summary>
        /// Out animation runs first, then in animation runs.
        /// </summary>
        Sequential,
        /// <summary>
        /// Both in and out animations start at the same time.
        /// </summary>
        Parallel
    }
}