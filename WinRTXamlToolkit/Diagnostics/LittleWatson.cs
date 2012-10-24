//using System;
//using System.Text;
//using Windows.ApplicationModel.DataTransfer;
//using Windows.Foundation;
//using Windows.Storage;
//using Windows.UI.Popups;
//using Windows.UI.Xaml;
//using WinRTXamlToolkit.IO;

//namespace WinRTXamlToolkit.Diagnostics
//{
//    public static class LittleWatson
//    {
//        public static StorageFolder Folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
//        public static string Filename = "LittleWatson.txt";
//        public static string EmailTarget = "user@domain.com";

//        private static TypedEventHandler<DataTransferManager, DataRequestedEventArgs> handler;
//        private static ErrorMessageInfo errormessage;

//        public static async void ReportException(UnhandledExceptionEventArgs ex, string extra)
//        {
//            try
//            {

//                errormessage = new ErrorMessageInfo()
//                {
//                    Usermessage = extra,
//                    Info = ex.Message,
//                    Exception = ex.Exception.Message,
//                    ExceptionDetail = ex.Exception.StackTrace
//                };

//                var file =
//                    await
//                    Folder.CreateFileAsync(
//                        Filename, CreationCollisionOption.ReplaceExisting);
//                FileIO.WriteTextAsync(errormessage);
//                Win8StorageHelper.SaveData(Filename, Folder, errormessage);
//            }
//            catch (Exception)
//            {
//            }
//        }

//        public async static void CheckForPreviousException()
//        {
//            try
//            {
//                errormessage = null;
//                errormessage = (ErrorMessageInfo)await Win8StorageHelper.LoadData(Filename, Folder, typeof(ErrorMessageInfo));
//                if (errormessage != null)
//                {
//                    ShowErrorMessageDialog();
//                }
//            }
//            catch (Exception)
//            {
//            }
//            finally
//            {
//                if (errormessage != null)
//                {
//                    Win8StorageHelper.SafeDeleteFile(Folder, Filename);
//                }
//            }
//        }

//        private static async void ShowErrorMessageDialog()
//        {
//            // Register handler for DataRequested events for sharing
//            if (handler != null)
//                DataTransferManager.GetForCurrentView().DataRequested -= handler;

//            handler = new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
//            DataTransferManager.GetForCurrentView().DataRequested += handler;

//            // Create the message dialog and set its content
//            var messageDialog = new MessageDialog("An error occured last time Flipped was run, do you want to help us out and send the error to us?");

//            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
//            messageDialog.Commands.Add(new UICommand(
//                "Share Error",
//                new UICommandInvokedHandler(LittleWatson.CommandInvokedHandler)));
//            messageDialog.Commands.Add(new UICommand(
//                "Cancel",
//                new UICommandInvokedHandler(LittleWatson.CommandInvokedHandler)));

//            // Set the command that will be invoked by default
//            messageDialog.DefaultCommandIndex = 0;

//            // Set the command to be invoked when escape is pressed
//            messageDialog.CancelCommandIndex = 1;

//            // Show the message dialog
//            await messageDialog.ShowAsync();
//        }

//        private static void CommandInvokedHandler(IUICommand command)
//        {
//            if (command.Label == "Cancel")
//            {
//                DataTransferManager.GetForCurrentView().DataRequested -= handler;
//            }
//            else
//            {
//                DataTransferManager.ShowShareUI();
//            }
//        }

//        private static void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
//        {
//            var request = args.Request;
//            request.Data.Properties.Title = errormessage.Usermessage;
//            request.Data.Properties.Description = errormessage.Info;

//            // Share recipe text
//            StringBuilder builder = new StringBuilder();
//            builder.Append("Please send report to:\r\n");
//            builder.Append(EmailTarget);
//            builder.AppendLine();

//            builder.Append("Error Detail\r\n");
//            builder.Append(errormessage.Exception);

//            builder.AppendLine();
//            builder.Append("\r\nAdditional Info\r\n");
//            builder.Append(errormessage.ExceptionDetail);
//            builder.AppendLine();

//            request.Data.SetText(builder.ToString());

//            DataTransferManager.GetForCurrentView().DataRequested -= handler;
//        }

//    }


//    public class ErrorMessageInfo
//    {
//        public string Usermessage;
//        public string Info;
//        public string Exception;
//        public string ExceptionDetail;
//    }
//}
