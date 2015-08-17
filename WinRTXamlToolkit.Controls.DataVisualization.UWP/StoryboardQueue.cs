// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;


namespace Controls.DataVisualization.Toolkit
{
    /// <summary>
    /// Represents a storyboard queue that plays storyboards in sequence.
    /// </summary>
    internal class StoryboardQueue
    {
        /// <summary>
        /// A queue of the storyboards.
        /// </summary>
        private readonly Queue<Storyboard> _storyBoards = new Queue<Storyboard>();

        /// <summary>
        /// Accepts a new storyboard to play in sequence.
        /// </summary>
        /// <param name="storyBoard">The storyboard to play.</param>
        /// <param name="completedAction">An action to execute when the 
        /// storyboard completes.</param>
        public void Enqueue(Storyboard storyBoard, EventHandler completedAction)
        {
            storyBoard.Completed +=
                (sender, args) =>
                {
                    if (completedAction != null)
                    {
                        completedAction(sender, (EventArgs)args);
                    }

                    _storyBoards.Dequeue();
                    Dequeue();
                };

            _storyBoards.Enqueue(storyBoard);

            if (_storyBoards.Count == 1)
            {
                Dequeue();
            }
        }

        /// <summary>
        /// Removes the next storyboard in the queue and plays it.
        /// </summary>
        private void Dequeue()
        {
            if (_storyBoards.Count > 0)
            {
                Storyboard storyboard = _storyBoards.Peek();
#pragma warning disable 4014
                storyboard.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => storyboard.Begin());
#pragma warning restore 4014
                //storyboard.Begin();
            }
        }
    }
}