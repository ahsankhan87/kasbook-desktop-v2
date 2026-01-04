using System;
using System.Data;

namespace POS.BLL
{
    public sealed class SettingsBLL
    {
        private const string SmallSaleThresholdKey = "SmallSaleThreshold";

        public double GetSmallSaleThreshold(double defaultValue = 200.0)
        {
            try
            {
                var generalBLL = new GeneralBLL();
                DataTable dt = generalBLL.GetRecord("TOP 1 setting_value", "pos_settings WHERE setting_key='" + SmallSaleThresholdKey + "'");

                if (dt != null && dt.Rows.Count > 0)
                {
                    double v;
                    return double.TryParse(Convert.ToString(dt.Rows[0]["setting_value"]), out v) ? v : defaultValue;
                }

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public void SetSmallSaleThreshold(double value)
        {
            var generalBLL = new GeneralBLL();

            // If row exists -> update, else -> insert
            DataTable exists = generalBLL.GetRecord("TOP 1 setting_key", "pos_settings WHERE setting_key='" + SmallSaleThresholdKey + "'");
            string v = value.ToString("0.##");

            if (exists != null && exists.Rows.Count > 0)
            {
                generalBLL.UpdateOrDeleteRecord("pos_settings", "setting_value='" + v + "'", "setting_key='" + SmallSaleThresholdKey + "'");
            }
            else
            {
                generalBLL.InsertRecord("pos_settings", "setting_key,setting_value", "'" + SmallSaleThresholdKey + "','" + v + "'");
            }
        }
    }
}
