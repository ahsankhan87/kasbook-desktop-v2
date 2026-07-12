using System;
using System.Runtime.Serialization;

namespace POS.Core
{
    /// <summary>
    /// Thrown when a required accounting configuration value is missing or invalid.
    /// Callers should surface the <see cref="UserMessage"/> in the UI rather than
    /// the raw exception message.
    /// </summary>
    [Serializable]
    public class AccountingConfigException : Exception
    {
        /// <summary>
        /// Human-readable message suitable for display to the end user.
        /// </summary>
        public string UserMessage { get; }

        /// <summary>
        /// The setting key that is missing or invalid, if applicable.
        /// </summary>
        public string SettingKey { get; }

        public AccountingConfigException(string userMessage)
            : base(userMessage)
        {
            UserMessage = userMessage;
        }

        public AccountingConfigException(string userMessage, string settingKey)
            : base(string.Format("[{0}] {1}", settingKey, userMessage))
        {
            UserMessage = userMessage;
            SettingKey  = settingKey;
        }

        public AccountingConfigException(string userMessage, string settingKey, Exception inner)
            : base(string.Format("[{0}] {1}", settingKey, userMessage), inner)
        {
            UserMessage = userMessage;
            SettingKey  = settingKey;
        }

        protected AccountingConfigException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            UserMessage = info.GetString(nameof(UserMessage));
            SettingKey  = info.GetString(nameof(SettingKey));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(UserMessage), UserMessage);
            info.AddValue(nameof(SettingKey),  SettingKey);
        }
    }
}
