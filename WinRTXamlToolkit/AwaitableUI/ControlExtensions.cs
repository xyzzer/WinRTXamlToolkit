using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Extension methods for Control class.
    /// </summary>
    public static class ControlExtensions
    {
        #region GoToVisualStateAsync()
        /// <summary>
        /// Goes to specified visual state, waiting for the transition to complete.
        /// </summary>
        /// <param name="control">
        /// Control to transition.
        /// </param>
        /// <param name="visualStatesHost">
        /// FrameworkElement that defines the visual states
        /// (usually the root of the control's template).
        /// </param>
        /// <param name="stateGroupName">
        /// Name of the state group
        /// (speeds up the search for the state transition storyboard).
        /// </param>
        /// <param name="stateName">
        /// State to transition to.
        /// </param>
        /// <returns>
        /// Awaitable task that completes when the transition storyboard completes.
        /// </returns>
        /// <remarks>
        /// If a state transition storyboard is not found - the task
        /// completes immediately.
        /// </remarks>
        public static async Task GoToVisualStateAsync(
            this Control control,
            FrameworkElement visualStatesHost,
            string stateGroupName,
            string stateName)
        {
            var tcs = new TaskCompletionSource<Storyboard>();

            Storyboard storyboard =
                GetStoryboardForVisualState(visualStatesHost, stateGroupName, stateName);

            if (storyboard != null)
            {
                EventHandler<object> eh = null;

                eh = (s, e) =>
                {
                    storyboard.Completed -= eh;
                    tcs.SetResult(storyboard);
                };

                storyboard.Completed += eh;
            }

            VisualStateManager.GoToState(control, stateName, true);

            if (storyboard == null)
            {
                return;
            }

            await tcs.Task;
        } 
        #endregion

        #region GetStoryboardForVisualState()
        /// <summary>
        /// Gets the state transition storyboard for the specified state.
        /// </summary>
        /// <param name="visualStatesHost">
        /// FrameworkElement that defines the visual states
        /// (usually the root of the control's template).
        /// </param>
        /// <param name="stateGroupName">
        /// Name of the state group
        /// (speeds up the search for the state transition storyboard).
        /// </param>
        /// <param name="stateName">
        /// State to transition to.
        /// </param>
        /// <returns>The state transition storyboard.</returns>
        private static Storyboard GetStoryboardForVisualState(
            FrameworkElement visualStatesHost,
            string stateGroupName,
            string stateName)
        {
            Storyboard storyboard = null;

            var stateGroups = VisualStateManager.GetVisualStateGroups(visualStatesHost);
            VisualStateGroup stateGroup = null;

            if (!string.IsNullOrEmpty(stateGroupName))
            {
                stateGroup = stateGroups.FirstOrDefault(g => g.Name == stateGroupName);
            }

            VisualState state = null;

            if (stateGroup != null)
            {
                state = stateGroup.States.FirstOrDefault(s => s.Name == stateName);
            }

            if (state == null)
            {
                foreach (var group in stateGroups)
                {
                    state = group.States.FirstOrDefault(s => s.Name == stateName);

                    if (state != null)
                    {
                        break;
                    }
                }
            }

            if (state != null)
            {
                storyboard = state.Storyboard;
            }

            return storyboard;
        } 
        #endregion
    }
}
