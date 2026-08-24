SET IDENTITY_INSERT pos_sales ON;
INSERT pos_sales ([id],[invoice_no], [store_id], [sale_time], [sale_date], [sale_type], [account], [total_amount], [total_tax], [exchange_rate], [paid], [discount_value], [discount_percent], [customer_id], [employee_id], [user_id], [register_mode], [amount_due], [description], [currency_id], [supplier_id], [branch_id], [is_return], [payment_terms_id], [payment_method_id], [customer_name], [customer_vat], [flatDiscountValue], [PONumber], [Zetca_qrcode], [zatca_status], [zatca_ubl_path], [zatca_error_message], [zatca_qrcode_phase2], [zatca_uuid], [zatca_hash], [zatca_message], [zatca_updated_at], [invoice_subtype_code], [zatca_mode], [zatcaInvoiceBase64], [prevInvoiceNo], [prevSaleDate], [returnReason], [returnReasonCode])
Select [id],[invoice_no], [store_id], [sale_time], [sale_date], [sale_type], [account], [total_amount], [total_tax], [exchange_rate], [paid], [discount_value], [discount_percent], [customer_id], [employee_id], [user_id], [register_mode], [amount_due], [description], [currency_id], [supplier_id], [branch_id], [is_return], [payment_terms_id], [payment_method_id], [customer_name], [customer_vat], [flatDiscountValue], [PONumber], [Zetca_qrcode], [zatca_status], [zatca_ubl_path], [zatca_error_message], [zatca_qrcode_phase2], [zatca_uuid], [zatca_hash], [zatca_message], [zatca_updated_at], [invoice_subtype_code], [zatca_mode], [zatcaInvoiceBase64], [prevInvoiceNo], [prevSaleDate], [returnReason], [returnReasonCode] from pos_db.dbo.pos_sales
SET IDENTITY_INSERT pos_sales OFF;

SET IDENTITY_INSERT pos_sales_items ON;
INSERT pos_sales_items ([id], [invoice_no], [sale_id], [item_code], [item_name], [quantity_sold], [cost_price], [unit_price], [discount_percent], [discount_value], [description], [service], [unit_id], [currency_id], [exchange_rate], [branch_id], [tax_id], [tax_rate], [inventory_acc_code], [serialnumber], [loc_code], [packet_qty], [item_number])
Select [id], [invoice_no], [sale_id], [item_code], [item_name], [quantity_sold], [cost_price], [unit_price], [discount_percent], [discount_value], [description], [service], [unit_id], [currency_id], [exchange_rate], [branch_id], [tax_id], [tax_rate], [inventory_acc_code], [serialnumber], [loc_code], [packet_qty], [item_number] from pos_db.dbo.pos_sales_items
SET IDENTITY_INSERT pos_sales_items OFF;

SET IDENTITY_INSERT pos_salesReturn ON;
INSERT pos_salesReturn ([id],[SalesId], [OriginalInvoiceNo], [ItemNumber], [ProductCode], [Description], [QtyReturned], [Amount], [ReturnReason], [User_id], [Created_at], [Updated_at], [Branch_id]) 
select [id],[SalesId], [OriginalInvoiceNo], [ItemNumber], [ProductCode], [Description], [QtyReturned], [Amount], [ReturnReason], [User_id], [Created_at], [Updated_at], [Branch_id] from pos_db.dbo.pos_salesReturn
SET IDENTITY_INSERT pos_salesReturn OFF;

SET IDENTITY_INSERT pos_products ON;
INSERT pos_products ([id], [branch_id], [item_number], [code], [name], [name_ar], [category_code], [item_type], [brand_code], [barcode], [status], [qty], [avg_cost], [cost_price], [unit_price], [unit_price_2], [tax_id], [unit_id], [location_code], [re_stock_level], [description], [deleted], [date_created], [date_updated], [user_id], [demand_qty], [purchase_demand_qty], [sale_demand_qty], [origin], [group_code], [alt_no], [picture], [expiry_date], [supplier_id], [packet_qty], [item_number_2], [part_number], [discount_scheme_id], [superseded_from_item_code], [superseded_to_item_code])
select [id], [branch_id], [item_number], [code], [name], [name_ar], [category_code], [item_type], [brand_code], [barcode], [status], [qty], [avg_cost], [cost_price], [unit_price], [unit_price_2], [tax_id], [unit_id], [location_code], [re_stock_level], [description], [deleted], [date_created], [date_updated], [user_id], [demand_qty], [purchase_demand_qty], [sale_demand_qty], [origin], [group_code], [alt_no], [picture], [expiry_date], [supplier_id], [packet_qty], [item_number_2], [part_number], [discount_scheme_id], [superseded_from_item_code], [superseded_to_item_code] from pos_db.dbo.pos_products
SET IDENTITY_INSERT pos_products OFF;

SET IDENTITY_INSERT pos_inventory ON;
INSERT pos_inventory ([id], [item_code], [qty], [cost_price], [unit_price], [branch_id], [user_id], [description], [invoice_no], [date_created], [date_updated], [customer_id], [supplier_id], [total_qty], [trans_date], [loc_code], [packet_qty], [item_number])
select [id], [item_code], [qty], [cost_price], [unit_price], [branch_id], [user_id], [description], [invoice_no], [date_created], [date_updated], [customer_id], [supplier_id], [total_qty], [trans_date], [loc_code], [packet_qty], [item_number] from pos_db.dbo.pos_inventory
SET IDENTITY_INSERT pos_inventory OFF;

SET IDENTITY_INSERT pos_product_stocks ON;
INSERT pos_product_stocks ([id],[branch_id],[user_id],[loc_code],[item_id],[item_code],[qty],[reorder_level],[date_created],[date_updated],[item_number])
select [id],[branch_id],[user_id],[loc_code],[item_id],[item_code],[qty],[reorder_level],[date_created],[date_updated],[item_number] from pos_db.dbo.pos_product_stocks
SET IDENTITY_INSERT pos_product_stocks OFF;

SET IDENTITY_INSERT pos_purchases ON;
INSERT pos_purchases ([id], [invoice_no], [purchase_time], [purchase_date], [purchase_type], [total_amount], [total_tax], [discount_value], [discount_percent], [supplier_id], [employee_id], [user_id], [register_mode], [account], [amount_due], [description], [currency_id], [branch_id], [supplier_invoice_no], [shipping_cost], [due_date], [payment_terms_id], [payment_method_id], [exchange_rate], [foreign_total_amount], [foreign_total_tax], [foreign_total_discount])
Select [id], [invoice_no], [purchase_time], [purchase_date], [purchase_type], [total_amount], [total_tax], [discount_value], [discount_percent], [supplier_id], [employee_id], [user_id], [register_mode], [account], [amount_due], [description], [currency_id], [branch_id], [supplier_invoice_no], [shipping_cost], [due_date], [payment_terms_id], [payment_method_id], [exchange_rate], [foreign_total_amount], [foreign_total_tax], [foreign_total_discount] from pos_db.dbo.pos_purchases
SET IDENTITY_INSERT pos_purchases OFF;

SET IDENTITY_INSERT pos_purchases_items ON;
INSERT pos_purchases_items ([id], [invoice_no], [purchase_id], [item_code], [quantity], [cost_price], [unit_price], [discount_percent], [discount_value], [description], [branch_id], [tax_id], [tax_rate], [inventory_acc_code], [serialnumber], [line], [loc_code], [packet_qty], [item_number], [currency_id], [exchange_rate], [foreign_unit_price], [foreign_cost_price], [foreign_discount_value])
Select [id], [invoice_no], [purchase_id], [item_code], [quantity], [cost_price], [unit_price], [discount_percent], [discount_value], [description], [branch_id], [tax_id], [tax_rate], [inventory_acc_code], [serialnumber], [line], [loc_code], [packet_qty], [item_number], [currency_id], [exchange_rate], [foreign_unit_price], [foreign_cost_price], [foreign_discount_value] from pos_db.dbo.pos_purchases_items
SET IDENTITY_INSERT pos_purchases_items OFF;

SET IDENTITY_INSERT pos_purchasesReturn ON;
INSERT pos_purchasesReturn ([id], [PurchaseId], [OriginalInvoiceNo], [ItemNumber], [ProductCode], [Description], [QtyReturned], [Amount], [ReturnReason], [User_id], [Created_at], [Updated_at], [Branch_id])
select [id], [PurchaseId], [OriginalInvoiceNo], [ItemNumber], [ProductCode], [Description], [QtyReturned], [Amount], [ReturnReason], [User_id], [Created_at], [Updated_at], [Branch_id] from pos_db.dbo.pos_purchasesReturn
SET IDENTITY_INSERT pos_purchasesReturn OFF;

SET IDENTITY_INSERT pos_customers ON;
INSERT pos_customers ([id],[branch_id],[first_name],[last_name],[address],[email],[contact_no],[status],[date_created],[date_updated],[vat_no],[user_id],[credit_limit],[vin_no],[car_name],[StreetName],[BuildingNumber],[CitySubdivisionName],[CityName],[PostalCode],[CountryName],[RegistrationName],[GLAccountID],[cr_number],[customer_code])
select [id],[branch_id],[first_name],[last_name],[address],[email],[contact_no],[status],[date_created],[date_updated],[vat_no],[user_id],[credit_limit],[vin_no],[car_name],[StreetName],[BuildingNumber],[CitySubdivisionName],[CityName],[PostalCode],[CountryName],[RegistrationName],[GLAccountID],[cr_number],[customer_code] from pos_db.dbo.pos_customers
SET IDENTITY_INSERT pos_customers OFF;

SET IDENTITY_INSERT pos_suppliers ON;
INSERT pos_suppliers ([id], [branch_id], [first_name], [last_name], [address], [email], [contact_no], [status], [date_created], [date_updated], [vat_no], [user_id], [vat_status], [StreetName], [BuildingNumber], [CitySubdivisionName], [CityName], [PostalCode], [CountryName], [GLAccountID], [supplier_code])
select [id], [branch_id], [first_name], [last_name], [address], [email], [contact_no], [status], [date_created], [date_updated], [vat_no], [user_id], [vat_status], [StreetName], [BuildingNumber], [CitySubdivisionName], [CityName], [PostalCode], [CountryName], [GLAccountID], [supplier_code] from pos_db.dbo.pos_suppliers
SET IDENTITY_INSERT pos_suppliers OFF;

SET IDENTITY_INSERT pos_brands ON;
INSERT pos_brands ([id], [branch_id], [user_id], [code], [name], [date_created], [date_updated], [category_code], [group_code])
select [id], [branch_id], [user_id], [code], [name], [date_created], [date_updated], [category_code], [group_code] from pos_db.dbo.pos_brands
SET IDENTITY_INSERT pos_brands OFF;

SET IDENTITY_INSERT pos_categories ON;
INSERT pos_categories ([id], [branch_id], [user_id], [code], [name], [date_created], [date_updated])
select [id], [branch_id], [user_id], [code], [name], [date_created], [date_updated] from pos_db.dbo.pos_categories
SET IDENTITY_INSERT pos_categories OFF;

SET IDENTITY_INSERT pos_locations ON;
INSERT pos_locations ([id], [branch_id], [user_id], [code], [name], [date_created], [date_updated])
select [id], [branch_id], [user_id], [code], [name], [date_created], [date_updated] from pos_db.dbo.pos_locations
SET IDENTITY_INSERT pos_locations OFF;

SET IDENTITY_INSERT pos_suppliers_payments ON;
INSERT pos_suppliers_payments ([id], [branch_id], [account_id], [account_name], [supplier_id], [invoice_no], [debit], [credit], [entry_date], [description], [entry_id], [date_created], [date_updated], [user_id], [payment_ref_invoice_no]) 
select [id], [branch_id], [account_id], [account_name], [supplier_id], [invoice_no], [debit], [credit], [entry_date], [description], [entry_id], [date_created], [date_updated], [user_id], [payment_ref_invoice_no] from pos_db.dbo.pos_suppliers_payments
SET IDENTITY_INSERT pos_suppliers_payments OFF;

SET IDENTITY_INSERT pos_customers_payments ON;
INSERT pos_customers_payments ([id], [branch_id], [account_id], [account_name], [customer_id], [invoice_no], [debit], [credit], [entry_date], [description], [entry_id], [date_created], [date_updated], [user_id], [payment_ref_invoice_no]) 
select [id], [branch_id], [account_id], [account_name], [customer_id], [invoice_no], [debit], [credit], [entry_date], [description], [entry_id], [date_created], [date_updated], [user_id], [payment_ref_invoice_no] from pos_db.dbo.pos_customers_payments
SET IDENTITY_INSERT pos_customers_payments OFF;

SET IDENTITY_INSERT pos_estimates ON;
INSERT pos_estimates ([id], [invoice_no], [store_id], [sale_time], [sale_date], [sale_type], [account], [total_amount], [total_tax], [exchange_rate], [paid], [discount_value], [discount_percent], [customer_id], [employee_id], [user_id], [register_mode], [amount_due], [description], [currency_id], [supplier_id], [branch_id], [is_return], [status], [customer_name], [customer_vat], [flatDiscountValue])
select [id], [invoice_no], [store_id], [sale_time], [sale_date], [sale_type], [account], [total_amount], [total_tax], [exchange_rate], [paid], [discount_value], [discount_percent], [customer_id], [employee_id], [user_id], [register_mode], [amount_due], [description], [currency_id], [supplier_id], [branch_id], [is_return], [status], [customer_name], [customer_vat], [flatDiscountValue] from pos_db.dbo.pos_estimates
SET IDENTITY_INSERT pos_estimates OFF;

SET IDENTITY_INSERT pos_estimates_items ON;
INSERT pos_estimates_items ([id], [invoice_no], [sale_id], [item_code], [item_name], [quantity_sold], [cost_price], [unit_price], [discount_percent], [discount_value], [description], [service], [unit_id], [currency_id], [exchange_rate], [branch_id], [tax_id], [tax_rate], [inventory_acc_code], [serialnumber], [loc_code], [packet_qty], [item_number]) 
select [id], [invoice_no], [sale_id], [item_code], [item_name], [quantity_sold], [cost_price], [unit_price], [discount_percent], [discount_value], [description], [service], [unit_id], [currency_id], [exchange_rate], [branch_id], [tax_id], [tax_rate], [inventory_acc_code], [serialnumber], [loc_code], [packet_qty], [item_number] from pos_db.dbo.pos_estimates_items
SET IDENTITY_INSERT pos_estimates_items OFF;

SET IDENTITY_INSERT pos_hold_purchases ON;
INSERT pos_hold_purchases ([id], [invoice_no], [purchase_time], [purchase_date], [purchase_type], [total_amount], [total_tax], [discount_value], [discount_percent], [supplier_id], [employee_id], [user_id], [register_mode], [account], [amount_due], [description], [currency_id], [branch_id], [supplier_invoice_no], [shipping_cost], [due_date], [payment_terms_id], [payment_method_id])
select [id], [invoice_no], [purchase_time], [purchase_date], [purchase_type], [total_amount], [total_tax], [discount_value], [discount_percent], [supplier_id], [employee_id], [user_id], [register_mode], [account], [amount_due], [description], [currency_id], [branch_id], [supplier_invoice_no], [shipping_cost], [due_date], [payment_terms_id], [payment_method_id] from pos_db.dbo.pos_hold_purchases
SET IDENTITY_INSERT pos_hold_purchases OFF;

SET IDENTITY_INSERT pos_hold_purchases_items ON;
INSERT pos_hold_purchases_items ([id], [invoice_no], [purchase_id], [item_code], [quantity], [cost_price], [unit_price], [discount_percent], [discount_value], [description], [branch_id], [tax_id], [tax_rate], [inventory_acc_code], [serialnumber], [line], [loc_code], [packet_qty], [item_number]) 
select [id], [invoice_no], [purchase_id], [item_code], [quantity], [cost_price], [unit_price], [discount_percent], [discount_value], [description], [branch_id], [tax_id], [tax_rate], [inventory_acc_code], [serialnumber], [line], [loc_code], [packet_qty], [item_number] from pos_db.dbo.pos_hold_purchases_items
SET IDENTITY_INSERT pos_hold_purchases_items OFF;

SET IDENTITY_INSERT acc_payments ON;
INSERT acc_payments ([id], [invoice_no], [payment_time], [branch_id], [user_id], [employee_id], [payment_date], [description], [name], [amount], [account_code], [tax_id], [tax_rate], [tax_amount], [supplier_invoice_no], [entry_id], [vat_no], [paymentType]) 
select [id], [invoice_no], [payment_time], [branch_id], [user_id], [employee_id], [payment_date], [description], [name], [amount], [account_code], [tax_id], [tax_rate], [tax_amount], [supplier_invoice_no], [entry_id], [vat_no], [paymentType] from pos_db.dbo.acc_payments
SET IDENTITY_INSERT acc_payments OFF;

SET IDENTITY_INSERT pos_banks ON;
INSERT pos_banks ([id], [GLAccountID], [code], [name], [accountNo], [holderName], [bankBranch], [branch_id], [user_id], [date_created], [date_updated])
select [id], [GLAccountID], [code], [name], [accountNo], [holderName], [bankBranch], [branch_id], [user_id], [date_created], [date_updated] from pos_db.dbo.pos_banks
SET IDENTITY_INSERT pos_banks OFF;

SET IDENTITY_INSERT pos_banks_payments ON;
INSERT pos_banks_payments ([id], [branch_id], [account_id], [account_name], [bank_id], [invoice_no], [debit], [credit], [entry_date], [description], [entry_id], [date_created], [date_updated], [user_id]) 
select [id], [branch_id], [account_id], [account_name], [bank_id], [invoice_no], [debit], [credit], [entry_date], [description], [entry_id], [date_created], [date_updated], [user_id] from pos_db.dbo.pos_banks_payments
SET IDENTITY_INSERT pos_banks_payments OFF;

SET IDENTITY_INSERT pos_product_adjustment ON;
INSERT pos_product_adjustment ([id], [invoice_no], [item_code], [qty], [cost_price], [unit_price], [branch_id], [user_id], [description], [date_created], [date_updated], [trans_date], [loc_code], [item_number])
select [id], [invoice_no], [item_code], [qty], [cost_price], [unit_price], [branch_id], [user_id], [description], [date_created], [date_updated], [trans_date], [loc_code], [item_number] from pos_db.dbo.pos_product_adjustment
SET IDENTITY_INSERT pos_product_adjustment OFF;

SET IDENTITY_INSERT Logs ON;
INSERT Logs ([Id], [BranchId], [UserId], [Action], [Timestamp], [Details], [PcName], [AdditionalInfo])
select [Id], [BranchId], [UserId], [Action], [Timestamp], [Details], [PcName], [AdditionalInfo] from pos_db.dbo.Logs
SET IDENTITY_INSERT Logs OFF;

update pos_customers set GLAccountID=6
update pos_suppliers set GLAccountID=21
update pos_banks set GLAccountID=3