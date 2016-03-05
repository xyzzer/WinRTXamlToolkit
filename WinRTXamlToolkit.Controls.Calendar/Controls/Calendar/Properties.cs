namespace WinRTXamlToolkit.Controls
{
    internal class Properties
    {
        internal class Resources
        {
            public static object Calendar_PreviousButtonName
            {
                get { return "previous button"; }
            }

            public static object Calendar_NextButtonName
            {
                get { return "next button"; }
            }

            public static string CalendarAutomationPeer_CalendarButtonLocalizedControlType
            {
                get { return "calendar button"; }
            }

            public static string CalendarAutomationPeer_DayButtonLocalizedControlType
            {
                get { return "day button"; }
            }

            public static string CalendarAutomationPeer_BlackoutDayHelpText
            {
                get { return "Blackout Day - {0}"; }
            }

            public static string Calendar_OnDisplayModePropertyChanged_InvalidValue
            {
                get { return "DisplayMode value is not valid."; }
            }

            public static string Calendar_OnFirstDayOfWeekChanged_InvalidValue
            {
                get { return "FirstDayOfWeek value is not valid."; }
            }

            public static string Calendar_OnSelectionModeChanged_InvalidValue
            {
                get { return "SelectionMode value is not valid."; }
            }

            public static string Calendar_OnSelectedDateChanged_InvalidValue
            {
                get { return "SelectedDate value is not valid."; }
            }

            public static string Calendar_OnSelectedDateChanged_InvalidOperation
            {
                get { return "The SelectedDate property cannot be set when the selection mode is None."; }
            }

            public static string CalendarAutomationPeer_MonthMode
            {
                get { return "Month"; }
            }

            public static string CalendarAutomationPeer_YearMode
            {
                get { return "Year"; }
            }

            public static string CalendarAutomationPeer_DecadeMode
            {
                get { return "Decade"; }
            }

            public static string CalendarCollection_MultiThreadedCollectionChangeNotSupported
            {
                get { return "This type of Collection does not support changes to its SourceCollection from a thread different from the Dispatcher thread."; }
            }

            public static string Calendar_CheckSelectionMode_InvalidOperation
            {
                get { return "The SelectedDates collection can be changed only in a multiple selection mode. Use the SelectedDate in a single selection mode."; }
            }

            public static string Calendar_UnSelectableDates
            {
                get { return "Value is not valid."; }
            }
        }
    }
}