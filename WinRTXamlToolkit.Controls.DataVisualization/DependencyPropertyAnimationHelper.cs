﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls.DataVisualization.Charting
{
    /// <summary>
    /// Represents a control that can animate the transitions between its specified
    /// dependency property.
    /// </summary>
    internal static class DependencyPropertyAnimationHelper
    {
        /// <summary>
        /// Number of key frames per second to generate the date time animations.
        /// </summary>
        public const int KeyFramesPerSecond = 20;

        /// <summary>
        /// The pattern used to ensure unique keys for the storyboards stored in
        /// a framework element's resource dictionary.
        /// </summary>
        private const string StoryboardKeyPattern = "__{0}__";

        /// <summary>
        /// Returns a unique key for a storyboard.
        /// </summary>
        /// <param name="propertyPath">The property path of the property that 
        /// the storyboard animates.</param>
        /// <returns>A unique key for a storyboard.</returns>
        private static string GetStoryboardKey(string propertyPath)
        {
            return string.Format(CultureInfo.InvariantCulture, StoryboardKeyPattern, propertyPath);
        }

        /// <summary>
        /// Starts animating a dependency property of a framework element to a 
        /// target value.
        /// </summary>
        /// <param name="target">The element to animate.</param>
        /// <param name="animatingDependencyProperty">The dependency property to
        /// animate.</param>
        /// <param name="propertyPath">The path of the dependency property to 
        /// animate.</param>
        /// <param name="targetValue">The value to animate the dependency
        /// property to.</param>
        /// <param name="timeSpan">The duration of the animation.</param>
        /// <param name="easingFunction">The easing function to uses to
        /// transition the data points.</param>
        public static void BeginAnimation(
            this FrameworkElement target,
            DependencyProperty animatingDependencyProperty,
            string propertyPath,
            object targetValue,
            TimeSpan timeSpan,
            EasingFunctionBase easingFunction)
        {
            string key = GetStoryboardKey(propertyPath);
            object resource;
            target.Resources.TryGetValue(key, out resource);
            Storyboard storyBoard = resource as Storyboard;

            if (storyBoard != null)
            {
                // Save current value
                object currentValue = target.GetValue(animatingDependencyProperty);
                storyBoard.Stop();
                // RestoreAsync that value so it doesn't snap back to its starting value
                target.SetValue(animatingDependencyProperty, currentValue);
                target.Resources.Remove(key);
            }

            storyBoard = CreateStoryboard(target, animatingDependencyProperty, propertyPath, ref targetValue, timeSpan, easingFunction);

            storyBoard.Completed +=
                (source, args) =>
                {
                    storyBoard.Stop();
                    target.SetValue(animatingDependencyProperty, targetValue);
                    target.Resources.Remove(key);
                };

            target.Resources.Add(key, storyBoard);
            storyBoard.Begin();
        }

        /// <summary>
        /// Creates a story board that animates a dependency property to a 
        /// value.
        /// </summary>
        /// <param name="target">The element that is the target of the 
        /// storyboard.</param>
        /// <param name="animatingDependencyProperty">The dependency property
        /// to animate.</param>
        /// <param name="propertyPath">The property path of the dependency
        /// property to animate.</param>
        /// <param name="toValue">The value to animate the dependency property
        /// to.</param>
        /// <param name="durationTimeSpan">The duration of the animation.
        /// </param>
        /// <param name="easingFunction">The easing function to use to
        /// transition the data points.</param>
        /// <returns>The story board that animates the property.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "easingFunction", Justification = "This parameter is used in Silverlight.")]
        private static Storyboard CreateStoryboard(
            FrameworkElement target,
            DependencyProperty animatingDependencyProperty,
            string propertyPath,
            ref object toValue,
            TimeSpan durationTimeSpan,
            EasingFunctionBase easingFunction)
        {
            object fromValue = target.GetValue(animatingDependencyProperty);

            double fromDoubleValue;
            double toDoubleValue;

            DateTime fromDateTime;
            DateTime toDateTime;

            Storyboard storyBoard = new Storyboard();
            Storyboard.SetTarget(storyBoard, target);

            Storyboard.SetTargetProperty(storyBoard, propertyPath);

            if ((fromValue != null && toValue != null))
            {
                if (ValueHelper.TryConvert(fromValue, out fromDoubleValue) && ValueHelper.TryConvert(toValue, out toDoubleValue))
                {
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.EnableDependentAnimation = true;
#if !NO_EASING_FUNCTIONS
                    doubleAnimation.EasingFunction = easingFunction;
#endif
                    doubleAnimation.Duration = durationTimeSpan;
                    doubleAnimation.To = ValueHelper.ToDouble(toValue);
                    toValue = doubleAnimation.To;

                    storyBoard.Children.Add(doubleAnimation);
                }
                else if (ValueHelper.TryConvert(fromValue, out fromDateTime) && ValueHelper.TryConvert(toValue, out toDateTime))
                {
                    ObjectAnimationUsingKeyFrames keyFrameAnimation = new ObjectAnimationUsingKeyFrames();
                    keyFrameAnimation.EnableDependentAnimation = true;
                    keyFrameAnimation.Duration = durationTimeSpan;

                    long intervals = (long)(durationTimeSpan.TotalSeconds * KeyFramesPerSecond);
                    if (intervals < 2L)
                    {
                        intervals = 2L;
                    }

                    IEnumerable<TimeSpan> timeSpanIntervals =
                        ValueHelper.GetTimeSpanIntervalsInclusive(durationTimeSpan, intervals);

                    IEnumerable<DateTime> dateTimeIntervals =
                        ValueHelper.GetDateTimesBetweenInclusive(fromDateTime, toDateTime, intervals);

                    IEnumerable<DiscreteObjectKeyFrame> keyFrames =
                        EnumerableFunctions.Zip(
                            dateTimeIntervals,
                            timeSpanIntervals,
                            (dateTime, timeSpan) => new DiscreteObjectKeyFrame() { Value = dateTime, KeyTime = timeSpan });

                    foreach (DiscreteObjectKeyFrame keyFrame in keyFrames)
                    {
                        keyFrameAnimation.KeyFrames.Add(keyFrame);
                        toValue = keyFrame.Value;
                    }

                    storyBoard.Children.Add(keyFrameAnimation);
                }
            }

            if (storyBoard.Children.Count == 0)
            {
                ObjectAnimationUsingKeyFrames keyFrameAnimation = new ObjectAnimationUsingKeyFrames();
                keyFrameAnimation.EnableDependentAnimation = true;
                DiscreteObjectKeyFrame endFrame = new DiscreteObjectKeyFrame() { Value = toValue, KeyTime = new TimeSpan(0, 0, 0) };
                keyFrameAnimation.KeyFrames.Add(endFrame);

                storyBoard.Children.Add(keyFrameAnimation);
            }

            return storyBoard;
        }
    }
}