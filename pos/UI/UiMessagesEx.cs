using System;

namespace pos.UI
{
    /// <summary>
    /// Helper extensions for UiMessages to support single-language calls
    /// </summary>
    internal static class UiMessagesEx
    {
        public static void Information(string title, string message)
        {
            UiMessages.ShowInfo(message, message, title, title);
        }

        public static void Warning(string title, string message)
        {
            UiMessages.ShowWarning(message, message, title, title);
        }

        public static void Error(string title, string message)
        {
            UiMessages.ShowError(message, message, title, title);
        }

        public static void Success(string title, string message)
        {
            UiMessages.ShowInfo(message, message, title, title);
        }
    }
}
