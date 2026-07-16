-- SQL Seeds for Enhanced Voucher Numbering with Date Format and Branch ID
-- These INSERT statements add default date format settings to pos_settings table
-- Execute these if you need to add default values for existing installations

-- Sales Invoice (JV)
INSERT INTO pos_settings (key_name, key_value, setting_type, description, category, is_required, created_date)
VALUES ('ACC_VOUCHER_JV_DATE_FORMAT', 'YYYYMMDD', 'STRING', 'Date format for JV voucher (e.g., YYYYMMDD, YYYY-MM-DD)', 'ACCOUNTING_VOUCHER', 0, GETDATE())
ON CONFLICT (key_name) DO UPDATE SET key_value = 'YYYYMMDD', updated_date = GETDATE();

-- Receipt Voucher
INSERT INTO pos_settings (key_name, key_value, setting_type, description, category, is_required, created_date)
VALUES ('ACC_VOUCHER_RECEIPT_DATE_FORMAT', 'YYYYMMDD', 'STRING', 'Date format for Receipt voucher (e.g., YYYYMMDD)', 'ACCOUNTING_VOUCHER', 0, GETDATE())
ON CONFLICT (key_name) DO UPDATE SET key_value = 'YYYYMMDD', updated_date = GETDATE();

-- Payment Voucher
INSERT INTO pos_settings (key_name, key_value, setting_type, description, category, is_required, created_date)
VALUES ('ACC_VOUCHER_PAYMENT_DATE_FORMAT', 'YYYYMMDD', 'STRING', 'Date format for Payment voucher (e.g., YYYYMMDD)', 'ACCOUNTING_VOUCHER', 0, GETDATE())
ON CONFLICT (key_name) DO UPDATE SET key_value = 'YYYYMMDD', updated_date = GETDATE();

-- Inter-Branch Transfer (IBT)
INSERT INTO pos_settings (key_name, key_value, setting_type, description, category, is_required, created_date)
VALUES ('ACC_VOUCHER_IBT_DATE_FORMAT', '', 'STRING', 'Date format for IBT voucher (leave empty to omit date)', 'ACCOUNTING_VOUCHER', 0, GETDATE())
ON CONFLICT (key_name) DO UPDATE SET key_value = '', updated_date = GETDATE();

-- Adjustment Voucher
INSERT INTO pos_settings (key_name, key_value, setting_type, description, category, is_required, created_date)
VALUES ('ACC_VOUCHER_ADJ_DATE_FORMAT', 'YYYYMMDD', 'STRING', 'Date format for Adjustment voucher (e.g., YYYYMMDD)', 'ACCOUNTING_VOUCHER', 0, GETDATE())
ON CONFLICT (key_name) DO UPDATE SET key_value = 'YYYYMMDD', updated_date = GETDATE();

-- Alternative: For SQL Server (without CONFLICT clause)
-- Uncomment this section if using SQL Server without modern MERGE syntax

/*
MERGE INTO pos_settings AS target
USING (VALUES 
  ('ACC_VOUCHER_JV_DATE_FORMAT', 'YYYYMMDD', 'STRING', 'Date format for JV voucher (e.g., YYYYMMDD, YYYY-MM-DD)', 'ACCOUNTING_VOUCHER', 0),
  ('ACC_VOUCHER_RECEIPT_DATE_FORMAT', 'YYYYMMDD', 'STRING', 'Date format for Receipt voucher', 'ACCOUNTING_VOUCHER', 0),
  ('ACC_VOUCHER_PAYMENT_DATE_FORMAT', 'YYYYMMDD', 'STRING', 'Date format for Payment voucher', 'ACCOUNTING_VOUCHER', 0),
  ('ACC_VOUCHER_IBT_DATE_FORMAT', '', 'STRING', 'Date format for IBT voucher (leave empty to omit date)', 'ACCOUNTING_VOUCHER', 0),
  ('ACC_VOUCHER_ADJ_DATE_FORMAT', 'YYYYMMDD', 'STRING', 'Date format for Adjustment voucher', 'ACCOUNTING_VOUCHER', 0)
) AS source (key_name, key_value, setting_type, description, category, is_required)
ON target.key_name = source.key_name
WHEN MATCHED THEN
  UPDATE SET key_value = source.key_value, updated_date = GETDATE()
WHEN NOT MATCHED THEN
  INSERT (key_name, key_value, setting_type, description, category, is_required, created_date)
  VALUES (source.key_name, source.key_value, source.setting_type, source.description, source.category, source.is_required, GETDATE());
*/

-- Verify the settings were created
SELECT key_name, key_value, description 
FROM pos_settings 
WHERE key_name LIKE 'ACC_VOUCHER_%_DATE_FORMAT'
ORDER BY key_name;

-- Expected Output:
-- ACC_VOUCHER_ADJ_DATE_FORMAT       | YYYYMMDD | Date format for Adjustment voucher (e.g., YYYYMMDD)
-- ACC_VOUCHER_IBT_DATE_FORMAT       |          | Date format for IBT voucher (leave empty to omit date)
-- ACC_VOUCHER_JV_DATE_FORMAT        | YYYYMMDD | Date format for JV voucher (e.g., YYYYMMDD, YYYY-MM-DD)
-- ACC_VOUCHER_PAYMENT_DATE_FORMAT   | YYYYMMDD | Date format for Payment voucher
-- ACC_VOUCHER_RECEIPT_DATE_FORMAT   | YYYYMMDD | Date format for Receipt voucher
