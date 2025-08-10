using pos.Sales;
using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using static java.security.cert.CertPathValidatorException;

namespace pos.Master.Companies.zatca
{

    public class ZatcaInvoiceGenerator
    {
        public XmlDocument GenerateZatcaInvoiceXmlDocument(DataSet ds, string invoiceNo)
        {
            // Get invoice header and items from dataset
            DataTable invoiceHeader = ds.Tables["Sale"];
            DataTable invoiceItems = ds.Tables["SalesItems"];

            // Create XML document with proper namespaces
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            // Create root Invoice element with namespaces
            XmlElement invoiceElement = xmlDoc.CreateElement("Invoice", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            invoiceElement.SetAttribute("xmlns:cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            invoiceElement.SetAttribute("xmlns:cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            invoiceElement.SetAttribute("xmlns:ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            xmlDoc.AppendChild(invoiceElement);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");


            // Add all invoice components
            AddUBLExtensions(xmlDoc, invoiceElement);
            AddBasicInvoiceInfo(xmlDoc, invoiceElement, invoiceHeader.Rows[0], invoiceNo);
            AddDocumentReferences(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddSupplierParty(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddCustomerParty(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddDeliveryInfo(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddPaymentMeans(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddAllowanceCharge(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddTaxTotals(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddLegalMonetaryTotals(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddInvoiceLines(xmlDoc, invoiceElement, invoiceItems);

            // Generate file name as per ZATCA: VAT + _ + Date + "T" + Time + _ + IRN.xml
            //string vatNumber = "1010010000"; // invoiceHeader.Rows[0]["customer_vat_no"].ToString();
            //string date = Convert.ToDateTime(invoiceHeader.Rows[0]["sale_date"]).ToString("yyyyMMdd");
            //string time = Convert.ToDateTime(invoiceHeader.Rows[0]["sale_time"]).ToString("HHmmss");
            //string sanitizedInvoiceNo = System.Text.RegularExpressions.Regex.Replace(invoiceNo, "[^a-zA-Z0-9]", "-");
            //string filename = $"{vatNumber}_{date}T{time}_{sanitizedInvoiceNo}.xml";
            //xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null), invoiceElement);
            //xmlDoc.DocumentElement.SetAttribute("filename", filename);

            //// Save directly with correct file name
            //string ublFolder = Path.Combine(Application.StartupPath, "UBL");
            //if (!Directory.Exists(ublFolder))
            //    Directory.CreateDirectory(ublFolder);

            //string filePath = Path.Combine(ublFolder, filename);
            //xmlDoc.Save(filePath);

            return xmlDoc;
        }

        private void AddBasicInvoiceInfo(XmlDocument xmlDoc, XmlElement parent, DataRow invoice, string invoiceNo)
        {
            // Profile ID (reporting:1.0 for standard invoices)
            AddElement(xmlDoc, parent, "cbc:ProfileID", "reporting:1.0");

            // Invoice ID (your invoice number)
            AddElement(xmlDoc, parent, "cbc:ID", invoiceNo);

            // UUID (should be unique for each invoice)
            AddElement(xmlDoc, parent, "cbc:UUID", Guid.NewGuid().ToString());

            // Issue date and time
            DateTime issueDate = Convert.ToDateTime(invoice["sale_time"]);
            AddElement(xmlDoc, parent, "cbc:IssueDate", issueDate.ToString("yyyy-MM-dd"));
            AddElement(xmlDoc, parent, "cbc:IssueTime", issueDate.ToString("HH:mm:ss"));

            // Auto determine invoice type based on "account"
            string accountType = invoice.Table.Columns.Contains("account") ? invoice["account"].ToString().ToLower() : "sale";
            string subtype = invoice.Table.Columns.Contains("invoice_subtype_code") ? invoice["invoice_subtype_code"].ToString() : "01"; // 01: Standard, 02: Simplified

            string invoiceTypeCode = "388"; // Default: Tax Invoice
            if (accountType == "return")
                invoiceTypeCode = "381"; // Credit Note
            else if (accountType == "sale")
                invoiceTypeCode = "388"; // Standard Sale
            else
                invoiceTypeCode = "388"; // 

            string invoiceTypeName = (subtype == "02" ? "0200000" : "0100000");

            // Invoice type code (388 for standard invoice)
            XmlElement invoiceTypeCodeXML = AddElement(xmlDoc, parent, "cbc:InvoiceTypeCode", invoiceTypeCode);
            invoiceTypeCodeXML.SetAttribute("name", invoiceTypeName);

            // Currency codes
            AddElement(xmlDoc, parent, "cbc:DocumentCurrencyCode", "SAR");
            AddElement(xmlDoc, parent, "cbc:TaxCurrencyCode", "SAR");
        }

        private void AddDocumentReferences(XmlDocument xmlDoc, XmlElement parent, DataRow invoice)
        {
            // ICV reference
            XmlElement icvRef = xmlDoc.CreateElement("cac", "AdditionalDocumentReference", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, icvRef, "cbc:ID", "ICV");
            AddElement(xmlDoc, icvRef, "cbc:UUID", invoice["id"].ToString()); // Should be calculated
            parent.AppendChild(icvRef);

            // PIH reference
            XmlElement pihRef = xmlDoc.CreateElement("cac", "AdditionalDocumentReference", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, pihRef, "cbc:ID", "PIH");
            XmlElement attachment = xmlDoc.CreateElement("cac", "Attachment", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            XmlElement binaryObject = AddElement(xmlDoc, attachment, "cbc:EmbeddedDocumentBinaryObject", "");
            binaryObject.SetAttribute("mimeCode", "text/plain");
            pihRef.AppendChild(attachment);
            parent.AppendChild(pihRef);

            // QR code reference (placeholder)
            XmlElement qrRef = xmlDoc.CreateElement("cac", "AdditionalDocumentReference", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, qrRef, "cbc:ID", "QR");
            XmlElement qrAttachment = xmlDoc.CreateElement("cac", "Attachment", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            XmlElement qrBinaryObject = AddElement(xmlDoc, qrAttachment, "cbc:EmbeddedDocumentBinaryObject", "");
            qrBinaryObject.SetAttribute("mimeCode", "text/plain");
            qrRef.AppendChild(qrAttachment);
            parent.AppendChild(qrRef);
        }

        private void AddSupplierParty(XmlDocument xmlDoc, XmlElement parent, DataRow invoice)
        {
            //seller
            string organizationName = "";
            string organizationUnitName = UsersModal.logged_in_branch_name;
            string countryName = "";
            string organizationIdentifier = "";
            string location = "";
            string StreetName = "";
            string BuildingNumber = "";
            string CitySubdivisionName = "";
            string CityName = "";
            string PostalCode = "";

            GeneralBLL objBLL = new GeneralBLL();
            string keyword = "TOP 1 *";
            string table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {
                organizationName = dr["name"].ToString();
                countryName = dr["CountryName"].ToString();
                organizationIdentifier = dr["vat_no"].ToString();
                location = dr["address"].ToString();
                StreetName = dr["StreetName"].ToString();
                BuildingNumber = dr["BuildingNumber"].ToString();
                CitySubdivisionName = dr["CitySubdivisionName"].ToString();
                CityName = dr["CityName"].ToString();
                PostalCode = dr["PostalCode"].ToString();

            }

            XmlElement supplierParty = xmlDoc.CreateElement("cac", "AccountingSupplierParty", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            XmlElement party = xmlDoc.CreateElement("cac", "Party", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            // Party identification (CRN - Commercial Registration Number)
            XmlElement partyIdentification = xmlDoc.CreateElement("cac", "PartyIdentification", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            XmlElement id = AddElement(xmlDoc, partyIdentification, "cbc:ID", organizationIdentifier);
            id.SetAttribute("schemeID", "CRN");
            party.AppendChild(partyIdentification);

            // Postal address
            XmlElement postalAddress = xmlDoc.CreateElement("cac", "PostalAddress", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, postalAddress, "cbc:StreetName", StreetName);
            AddElement(xmlDoc, postalAddress, "cbc:BuildingNumber", BuildingNumber);
            AddElement(xmlDoc, postalAddress, "cbc:CitySubdivisionName", CitySubdivisionName);
            AddElement(xmlDoc, postalAddress, "cbc:CityName", CityName);
            AddElement(xmlDoc, postalAddress, "cbc:PostalZone", PostalCode);
            XmlElement country = xmlDoc.CreateElement("cac", "Country", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, country, "cbc:IdentificationCode", "SA");
            postalAddress.AppendChild(country);
            party.AppendChild(postalAddress);

            // Tax scheme
            XmlElement partyTaxScheme = xmlDoc.CreateElement("cac", "PartyTaxScheme", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, partyTaxScheme, "cbc:CompanyID", organizationIdentifier);
            XmlElement taxScheme = xmlDoc.CreateElement("cac", "TaxScheme", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, taxScheme, "cbc:ID", "VAT");
            partyTaxScheme.AppendChild(taxScheme);
            party.AppendChild(partyTaxScheme);

            // Legal entity
            XmlElement partyLegalEntity = xmlDoc.CreateElement("cac", "PartyLegalEntity", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, partyLegalEntity, "cbc:RegistrationName", organizationName);
            party.AppendChild(partyLegalEntity);

            supplierParty.AppendChild(party);
            parent.AppendChild(supplierParty);
        }

        private void AddCustomerParty(XmlDocument xmlDoc, XmlElement parent, DataRow invoice)
        {
            //buyer
            XmlElement customerParty = xmlDoc.CreateElement("cac", "AccountingCustomerParty", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            XmlElement party = xmlDoc.CreateElement("cac", "Party", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            // Postal address
            XmlElement postalAddress = xmlDoc.CreateElement("cac", "PostalAddress", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, postalAddress, "cbc:StreetName", "street name");
            AddElement(xmlDoc, postalAddress, "cbc:BuildingNumber", "3724");
            AddElement(xmlDoc, postalAddress, "cbc:CitySubdivisionName", "Alfalah");
            AddElement(xmlDoc, postalAddress, "cbc:CityName", "Alfalah");
            AddElement(xmlDoc, postalAddress, "cbc:PostalZone", "15385");
            XmlElement country = xmlDoc.CreateElement("cac", "Country", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, country, "cbc:IdentificationCode", "SA");
            postalAddress.AppendChild(country);
            party.AppendChild(postalAddress);

            // Tax scheme
            XmlElement partyTaxScheme = xmlDoc.CreateElement("cac", "PartyTaxScheme", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, partyTaxScheme, "cbc:CompanyID", "310424415000003");
            XmlElement taxScheme = xmlDoc.CreateElement("cac", "TaxScheme", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, taxScheme, "cbc:ID", "VAT");
            partyTaxScheme.AppendChild(taxScheme);
            party.AppendChild(partyTaxScheme);

            // Legal entity
            XmlElement partyLegalEntity = xmlDoc.CreateElement("cac", "PartyLegalEntity", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, partyLegalEntity, "cbc:RegistrationName", "مؤسسة المشتري");
            party.AppendChild(partyLegalEntity);

            customerParty.AppendChild(party);
            parent.AppendChild(customerParty);
        }

        private void AddDeliveryInfo(XmlDocument xmlDoc, XmlElement parent, DataRow invoice)
        {
            XmlElement delivery = xmlDoc.CreateElement("cac", "Delivery", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            DateTime deliveryDate = Convert.ToDateTime(invoice["sale_date"]);
            AddElement(xmlDoc, delivery, "cbc:ActualDeliveryDate", deliveryDate.ToString("yyyy-MM-dd"));
            parent.AppendChild(delivery);
        }
        private void AddPaymentMeans(XmlDocument xmlDoc, XmlElement parent, DataRow invoice, string invoiceTypeCode="")
        {
            XmlElement paymentMeans = xmlDoc.CreateElement("cac", "PaymentMeans", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            string paymentMethod = invoice.Table.Columns.Contains("sale_type") ? invoice["sale_type"].ToString() : "Cash";
            string paymentMeansCode = paymentMethod == "Cash" ? "10" : "30"; // 10=Cash, 30=Credit

            // For credit/debit notes, use code 42 and add InstructionNote for reason
            //string invoiceTypeCode = invoice.Table.Columns.Contains("invoice_type_code") ? invoice["invoice_type_code"].ToString() : "388";
            if (invoiceTypeCode == "381" || invoiceTypeCode == "383") // 381=Credit Note, 383=Debit Note
            {
                paymentMeansCode = "42"; // ZATCA code for credit/debit note
                                         // Get the reason from the DataRow, or set a default if not present
                
                string reason = invoice.Table.Columns.Contains("returnReason") ? invoice["returnReason"].ToString() : "Reason for issuing credit/debit note";
                //string reason = "Reason for issuing credit/debit note"; // Default reason, can be customized
                AddElement(xmlDoc, paymentMeans, "cbc:PaymentMeansCode", paymentMeansCode);
                AddElement(xmlDoc, paymentMeans, "cbc:InstructionNote", reason);
            }
            else
            {
                AddElement(xmlDoc, paymentMeans, "cbc:PaymentMeansCode", paymentMeansCode);

                if (paymentMeansCode == "30") // Credit
                {
                    XmlElement paymentTerms = xmlDoc.CreateElement("cac", "PaymentTerms", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                    AddElement(xmlDoc, paymentTerms, "cbc:Note", "Payment due within 30 days");
                    AddElement(xmlDoc, paymentTerms, "cbc:PaymentDueDate", DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"));
                    paymentMeans.AppendChild(paymentTerms);
                }
            }

            parent.AppendChild(paymentMeans);
        }
        
        private void AddAllowanceCharge(XmlDocument xmlDoc, XmlElement parent, DataRow invoice)
        {
            decimal discount = Math.Abs(Convert.ToDecimal(invoice["discount_value"]));

            if (discount > 0)
            {
                XmlElement allowanceCharge = xmlDoc.CreateElement("cac", "AllowanceCharge", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                AddElement(xmlDoc, allowanceCharge, "cbc:ChargeIndicator", "false");
                AddElement(xmlDoc, allowanceCharge, "cbc:AllowanceChargeReason", "discount");
                AddElement(xmlDoc, allowanceCharge, "cbc:Amount", discount.ToString("F2"), "SAR");

                XmlElement taxCategory = xmlDoc.CreateElement("cac", "TaxCategory", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                XmlElement categoryId = AddElement(xmlDoc, taxCategory, "cbc:ID", "S");
                categoryId.SetAttribute("schemeID", "UN/ECE 5305");
                categoryId.SetAttribute("schemeAgencyID", "6");
                AddElement(xmlDoc, taxCategory, "cbc:Percent", "15.00");

                XmlElement taxScheme = xmlDoc.CreateElement("cac", "TaxScheme", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                AddElement(xmlDoc, taxScheme, "cbc:ID", "VAT");
                taxCategory.AppendChild(taxScheme);
                allowanceCharge.AppendChild(taxCategory);

                parent.AppendChild(allowanceCharge);
            }
        }

        private void AddTaxTotals(XmlDocument xmlDoc, XmlElement parent, DataRow invoice)
        {
            decimal taxableAmount = Convert.ToDecimal(invoice["total_amount"]) - Convert.ToDecimal(invoice["discount_value"]);
            decimal taxPercent = 15m; // Standard VAT rate in KSA
            decimal taxAmount = Math.Round(taxableAmount * taxPercent / 100, 2);

            // First TaxTotal (summary)
            XmlElement taxTotal1 = xmlDoc.CreateElement("cac", "TaxTotal", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, taxTotal1, "cbc:TaxAmount", Math.Abs(taxAmount).ToString("F2"), "SAR");
            parent.AppendChild(taxTotal1);

            // Second TaxTotal (detailed)
            XmlElement taxTotal2 = xmlDoc.CreateElement("cac", "TaxTotal", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, taxTotal2, "cbc:TaxAmount", Math.Abs(taxAmount).ToString("F2"), "SAR");

            XmlElement taxSubtotal = xmlDoc.CreateElement("cac", "TaxSubtotal", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, taxSubtotal, "cbc:TaxableAmount", Math.Abs(taxableAmount).ToString("F2"), "SAR");
            AddElement(xmlDoc, taxSubtotal, "cbc:TaxAmount", Math.Abs(taxAmount).ToString("F2"), "SAR");

            XmlElement taxCategory = xmlDoc.CreateElement("cac", "TaxCategory", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            XmlElement categoryId = AddElement(xmlDoc, taxCategory, "cbc:ID", "S");
            categoryId.SetAttribute("schemeID", "UN/ECE 5305");
            categoryId.SetAttribute("schemeAgencyID", "6");
            AddElement(xmlDoc, taxCategory, "cbc:Percent", taxPercent.ToString("F2"));

            XmlElement taxScheme = xmlDoc.CreateElement("cac", "TaxScheme", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            XmlElement schemeId = AddElement(xmlDoc, taxScheme, "cbc:ID", "VAT");
            schemeId.SetAttribute("schemeID", "UN/ECE 5153");
            schemeId.SetAttribute("schemeAgencyID", "6");
            taxCategory.AppendChild(taxScheme);
            taxSubtotal.AppendChild(taxCategory);
            taxTotal2.AppendChild(taxSubtotal);
            parent.AppendChild(taxTotal2);
        }

        public void AddLegalMonetaryTotals(XmlDocument xmlDoc, XmlElement parent, DataRow invoice)
        {
            decimal discount = Convert.ToDecimal(invoice["discount_value"]);
            decimal lineTotal = Convert.ToDecimal(invoice["total_amount"])- discount;
            decimal taxAmount = Convert.ToDecimal(invoice["total_tax"]);
            decimal payableAmount = lineTotal + taxAmount;

            XmlElement monetaryTotal = xmlDoc.CreateElement("cac", "LegalMonetaryTotal", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            AddElement(xmlDoc, monetaryTotal, "cbc:LineExtensionAmount", Math.Abs(lineTotal).ToString("F2"), "SAR");
            AddElement(xmlDoc, monetaryTotal, "cbc:TaxExclusiveAmount", Math.Abs(lineTotal).ToString("F2"), "SAR");
            AddElement(xmlDoc, monetaryTotal, "cbc:TaxInclusiveAmount", Math.Abs(payableAmount).ToString("F2"), "SAR");
            AddElement(xmlDoc, monetaryTotal, "cbc:AllowanceTotalAmount", Math.Abs(discount).ToString("F2"), "SAR");
            AddElement(xmlDoc, monetaryTotal, "cbc:PrepaidAmount", "0.00", "SAR");
            AddElement(xmlDoc, monetaryTotal, "cbc:PayableAmount", Math.Abs(payableAmount).ToString("F2"), "SAR");
            parent.AppendChild(monetaryTotal);
        }

        private void AddInvoiceLines(XmlDocument xmlDoc, XmlElement parent, DataTable invoiceItems)
        {
            int lineNumber = 1;
            foreach (DataRow item in invoiceItems.Rows)
            {
                decimal quantity = Convert.ToDecimal(item["quantity_sold"]);
                decimal discount = Convert.ToDecimal(item["discount_value"]);
                decimal price = Convert.ToDecimal(item["unit_price"]) - discount;
                decimal lineTotal = quantity * price;
                decimal taxPercent = Convert.ToDecimal(item["tax_rate"]);
                decimal taxAmount = Convert.ToDecimal(item["vat"]);

                XmlElement invoiceLine = xmlDoc.CreateElement("cac", "InvoiceLine", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                AddElement(xmlDoc, invoiceLine, "cbc:ID", Math.Abs(lineNumber).ToString());

                // Invoiced quantity
                XmlElement invoicedQuantity = AddElement(xmlDoc, invoiceLine, "cbc:InvoicedQuantity", Math.Abs(quantity).ToString("F6"));
                invoicedQuantity.SetAttribute("unitCode", "PCE"); // PCE = pieces

                // Line extension amount
                AddElement(xmlDoc, invoiceLine, "cbc:LineExtensionAmount", Math.Abs(lineTotal).ToString("F2"), "SAR");

                // Tax total for line
                XmlElement lineTaxTotal = xmlDoc.CreateElement("cac", "TaxTotal", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                AddElement(xmlDoc, lineTaxTotal, "cbc:TaxAmount", Math.Abs(taxAmount).ToString("F2"), "SAR");
                AddElement(xmlDoc, lineTaxTotal, "cbc:RoundingAmount", Math.Abs(lineTotal + taxAmount).ToString("F2"), "SAR");
                invoiceLine.AppendChild(lineTaxTotal);

                // Item information
                XmlElement itemElement = xmlDoc.CreateElement("cac", "Item", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                AddElement(xmlDoc, itemElement, "cbc:Name", item["name"].ToString());

                // Tax category for item
                XmlElement classifiedTaxCategory = xmlDoc.CreateElement("cac", "ClassifiedTaxCategory", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                AddElement(xmlDoc, classifiedTaxCategory, "cbc:ID", "S"); // S = Standard rate
                AddElement(xmlDoc, classifiedTaxCategory, "cbc:Percent", taxPercent.ToString("F2"));

                XmlElement itemTaxScheme = xmlDoc.CreateElement("cac", "TaxScheme", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                AddElement(xmlDoc, itemTaxScheme, "cbc:ID", "VAT");
                classifiedTaxCategory.AppendChild(itemTaxScheme);
                itemElement.AppendChild(classifiedTaxCategory);
                invoiceLine.AppendChild(itemElement);

                // Price information
                XmlElement priceElement = xmlDoc.CreateElement("cac", "Price", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                AddElement(xmlDoc, priceElement, "cbc:PriceAmount", Math.Abs(price).ToString("F2"), "SAR");
                invoiceLine.AppendChild(priceElement);

                parent.AppendChild(invoiceLine);
                lineNumber++;
            }
        }

        private XmlElement AddElement(XmlDocument xmlDoc, XmlElement parent, string qualifiedName, string value, string currencyID = null)
        {
            string[] parts = qualifiedName.Split(':');
            string prefix = parts[0];
            string localName = parts[1];
            string namespaceUri = prefix == "cbc" ?
                "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" :
                "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

            XmlElement element = xmlDoc.CreateElement(prefix, localName, namespaceUri);
            element.InnerText = value;

            if (currencyID != null)
            {
                element.SetAttribute("currencyID", currencyID);
            }

            if (parent != null)
            {
                parent.AppendChild(element);
            }

            return element;
        }
        private void AddUBLExtensions(XmlDocument xmlDoc, XmlElement parent)
        {
            // Create UBLExtensions element
            XmlElement ublExtensions = xmlDoc.CreateElement("ext", "UBLExtensions", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

            parent.AppendChild(ublExtensions);
        }

        private void AddTransform(XmlDocument xmlDoc, XmlElement parent, string algorithm, string xpath)
        {
            XmlElement transform = xmlDoc.CreateElement("ds", "Transform", "http://www.w3.org/2000/09/xmldsig#");
            transform.SetAttribute("Algorithm", algorithm);

            if (xpath != null)
            {
                XmlElement xpathElement = xmlDoc.CreateElement("ds", "XPath", "http://www.w3.org/2000/09/xmldsig#");
                xpathElement.InnerText = xpath;
                transform.AppendChild(xpathElement);
            }

            parent.AppendChild(transform);
        }

        public XmlDocument GenerateZatcaCreditNoteXmlDocument(DataSet ds, string invoiceNo, string prevInvoiceNo = null, DateTime? prevInvoiceDate = null)
        {
            DataTable invoiceHeader = ds.Tables["Sale"];
            DataTable invoiceItems = ds.Tables["SalesItems"];

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XmlElement invoiceElement = xmlDoc.CreateElement("Invoice", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            invoiceElement.SetAttribute("xmlns:cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            invoiceElement.SetAttribute("xmlns:cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            invoiceElement.SetAttribute("xmlns:ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            xmlDoc.AppendChild(invoiceElement);

            AddUBLExtensions(xmlDoc, invoiceElement);

            // Set InvoiceTypeCode for Credit Note (381)
            AddElement(xmlDoc, invoiceElement, "cbc:ProfileID", "reporting:1.0");
            AddElement(xmlDoc, invoiceElement, "cbc:ID", invoiceNo);
            AddElement(xmlDoc, invoiceElement, "cbc:UUID", Guid.NewGuid().ToString());
            DateTime issueDate = Convert.ToDateTime(invoiceHeader.Rows[0]["sale_time"]);
            AddElement(xmlDoc, invoiceElement, "cbc:IssueDate", issueDate.ToString("yyyy-MM-dd"));
            AddElement(xmlDoc, invoiceElement, "cbc:IssueTime", issueDate.ToString("HH:mm:ss"));

            string accountType = invoiceHeader.Rows[0].Table.Columns.Contains("account") ? invoiceHeader.Rows[0]["account"].ToString().ToLower() : "sale";
            string subtype = invoiceHeader.Rows[0].Table.Columns.Contains("invoice_subtype_code") ? invoiceHeader.Rows[0]["invoice_subtype_code"].ToString() : "01"; // 01: Standard, 02: Simplified

            //string invoiceTypeCode = "388"; // Default: Tax Invoice
            //if (accountType == "return")
            //    invoiceTypeCode = "381"; // Credit Note
            //else if (accountType == "sale")
            //    invoiceTypeCode = "388"; // Standard Sale
            //else
            //    invoiceTypeCode = "388"; // 

            string invoiceTypeName = (subtype == "02" ? "0200000" : "0100000");

            XmlElement invoiceTypeCodeXML = AddElement(xmlDoc, invoiceElement, "cbc:InvoiceTypeCode", "381");
            invoiceTypeCodeXML.SetAttribute("name", invoiceTypeName); // Credit Note
                                                                      // Assuming 'reason' is a string variable containing the reason for the note
            
            AddElement(xmlDoc, invoiceElement, "cbc:DocumentCurrencyCode", "SAR");
            AddElement(xmlDoc, invoiceElement, "cbc:TaxCurrencyCode", "SAR");

            // Add BillingReference for previous invoice
            if (!string.IsNullOrEmpty(prevInvoiceNo) && prevInvoiceDate.HasValue)
            {
                XmlElement billingReference = xmlDoc.CreateElement("cac", "BillingReference", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                XmlElement invoiceDocRef = xmlDoc.CreateElement("cac", "InvoiceDocumentReference", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                string idValue = $"Invoice Number: {prevInvoiceNo}; Invoice Issue Date: {prevInvoiceDate.Value:yyyy-MM-dd}";
                AddElement(xmlDoc, invoiceDocRef, "cbc:ID", idValue);
                billingReference.AppendChild(invoiceDocRef);
                invoiceElement.AppendChild(billingReference);
            }

            AddDocumentReferences(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddSupplierParty(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddCustomerParty(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddDeliveryInfo(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddPaymentMeans(xmlDoc, invoiceElement, invoiceHeader.Rows[0], "381");// 381=Credit Note, 383=Debit Note
            AddAllowanceCharge(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddTaxTotals(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddLegalMonetaryTotals(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddInvoiceLines(xmlDoc, invoiceElement, invoiceItems);

            return xmlDoc;
        }

        public XmlDocument GenerateZatcaDebitNoteXmlDocument(DataSet ds, string invoiceNo, string prevInvoiceNo = null, DateTime? prevInvoiceDate = null)
        {
            DataTable invoiceHeader = ds.Tables["Sale"];
            DataTable invoiceItems = ds.Tables["SalesItems"];

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XmlElement invoiceElement = xmlDoc.CreateElement("Invoice", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            invoiceElement.SetAttribute("xmlns:cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            invoiceElement.SetAttribute("xmlns:cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            invoiceElement.SetAttribute("xmlns:ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            xmlDoc.AppendChild(invoiceElement);

            AddUBLExtensions(xmlDoc, invoiceElement);

            // Set InvoiceTypeCode for Debit Note (383)
            AddElement(xmlDoc, invoiceElement, "cbc:ProfileID", "reporting:1.0");
            AddElement(xmlDoc, invoiceElement, "cbc:ID", invoiceNo);
            AddElement(xmlDoc, invoiceElement, "cbc:UUID", Guid.NewGuid().ToString());
            DateTime issueDate = Convert.ToDateTime(invoiceHeader.Rows[0]["sale_time"]);
            AddElement(xmlDoc, invoiceElement, "cbc:IssueDate", issueDate.ToString("yyyy-MM-dd"));
            AddElement(xmlDoc, invoiceElement, "cbc:IssueTime", issueDate.ToString("HH:mm:ss"));

            string accountType = invoiceHeader.Rows[0].Table.Columns.Contains("account") ? invoiceHeader.Rows[0]["account"].ToString().ToLower() : "sale";
            string subtype = invoiceHeader.Rows[0].Table.Columns.Contains("invoice_subtype_code") ? invoiceHeader.Rows[0]["invoice_subtype_code"].ToString() : "01"; // 01: Standard, 02: Simplified

            //string invoiceTypeCode = "388"; // Default: Tax Invoice
            //if (accountType == "return")
            //    invoiceTypeCode = "381"; // Credit Note
            //else if (accountType == "sale")
            //    invoiceTypeCode = "388"; // Standard Sale
            //else
            //    invoiceTypeCode = "388"; // 

            string invoiceTypeName = (subtype == "02" ? "0200000" : "0100000");

            XmlElement invoiceTypeCodeXML = AddElement(xmlDoc, invoiceElement, "cbc:InvoiceTypeCode", "383");
            invoiceTypeCodeXML.SetAttribute("name", invoiceTypeName); // Debit Note

            AddElement(xmlDoc, invoiceElement, "cbc:DocumentCurrencyCode", "SAR");
            AddElement(xmlDoc, invoiceElement, "cbc:TaxCurrencyCode", "SAR");

            // Add BillingReference for previous invoice
            if (!string.IsNullOrEmpty(prevInvoiceNo) && prevInvoiceDate.HasValue)
            {
                XmlElement billingReference = xmlDoc.CreateElement("cac", "BillingReference", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                XmlElement invoiceDocRef = xmlDoc.CreateElement("cac", "InvoiceDocumentReference", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                string idValue = $"Invoice Number: {prevInvoiceNo}; Invoice Issue Date: {prevInvoiceDate.Value:yyyy-MM-dd}";
                AddElement(xmlDoc, invoiceDocRef, "cbc:ID", idValue);
                billingReference.AppendChild(invoiceDocRef);
                invoiceElement.AppendChild(billingReference);
            }

            AddDocumentReferences(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddSupplierParty(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddCustomerParty(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddDeliveryInfo(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddPaymentMeans(xmlDoc, invoiceElement, invoiceHeader.Rows[0], "383");// 381=Credit Note, 383=Debit Note
            AddAllowanceCharge(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddTaxTotals(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddLegalMonetaryTotals(xmlDoc, invoiceElement, invoiceHeader.Rows[0]);
            AddInvoiceLines(xmlDoc, invoiceElement, invoiceItems);

            return xmlDoc;
        }

        // Step 1: Save UUID, Hash, and Status to DB
        public static void SaveZatcaStatusToDatabase(string invoiceNo, string uuid, string invoiceHash, string invoiceBase64,
            string status, string env, string message = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE pos_sales SET zatca_uuid = @uuid, zatca_hash = @hash, zatca_mode = @mode, " +
                    "zatca_status = @status, zatcaInvoiceBase64 = @invoiceBase64, zatca_message = @message, zatca_updated_at = GETDATE() WHERE invoice_no = @invoice_no", cn);
                cmd.Parameters.AddWithValue("@uuid", (object)uuid ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hash", (object)invoiceHash ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object)status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@mode", (object)env?? DBNull.Value);
                cmd.Parameters.AddWithValue("@invoiceBase64", (object)invoiceBase64 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@message", (object)message ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@invoice_no", invoiceNo);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Step 2: Update UI - Add a column in DataGridView on frm_all_sales
        // Columns: ZATCA Status, UUID, QR View button (done in your UI, no code needed here)

        

        // Step 6: Add log entry (optional)
        public void LogZatcaSubmission(string invoiceNo, string logMessage)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO zatca_logs (invoice_no, log_message, log_date) VALUES (@invoice_no, @msg, GETDATE())", cn);
                cmd.Parameters.AddWithValue("@invoice_no", invoiceNo);
                cmd.Parameters.AddWithValue("@msg", logMessage);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Step 7: Credential management for Simulation and Production
        public static string GetCertFromDb(int branchId, string mode)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT cert_base64 FROM zatca_credentials WHERE branch_id = @branchId AND mode = @mode", cn);
                cmd.Parameters.AddWithValue("@branchId", branchId);
                cmd.Parameters.AddWithValue("@mode", mode);
                cn.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }
        public static string GetComplainceRequestIDFromDb(int branchId, string mode)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT compliance_request_id FROM zatca_credentials WHERE branch_id = @branchId AND mode = @mode", cn);
                cmd.Parameters.AddWithValue("@branchId", branchId);
                cmd.Parameters.AddWithValue("@mode", mode);
                cn.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }
        public static string GetPrivateKeyFromDb(int branchId, string mode)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT private_key FROM zatca_credentials WHERE branch_id = @branchId AND mode = @mode", cn);
                cmd.Parameters.AddWithValue("@branchId", branchId);
                cmd.Parameters.AddWithValue("@mode", mode);
                cn.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        public static string GetSecretFromDb(int branchId, string mode)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT secret_key FROM zatca_credentials WHERE branch_id = @branchId AND mode = @mode", cn);
                cmd.Parameters.AddWithValue("@branchId", branchId);
                cmd.Parameters.AddWithValue("@mode", mode);
                cn.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        public static int UpsertZatcaCredentials(string cert_type, string mode, string cert, string privateKey, string secret,string csr,string otp,string compliance_request_id)
        {
            int newID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(@"
            IF EXISTS (SELECT 1 FROM zatca_credentials WHERE branch_id = @branchId AND mode = @mode AND cert_type=@cert_type)
                UPDATE zatca_credentials 
                SET cert_base64 = @cert, private_key = @privateKey, secret_key = @secret, updated_at = GETDATE(), csr_text=@csr_text, 
                otp=@otp, compliance_request_id=@compliance_request_id, cert_type=@cert_type
                WHERE branch_id = @branchId AND mode = @mode AND cert_type=@cert_type
            ELSE
                INSERT INTO zatca_credentials (company_id, user_id, branch_id, mode, cert_base64, private_key, secret_key, updated_at,csr_text,created_at,otp,compliance_request_id, cert_type)
                VALUES (@companyID,@userID,@branchId, @mode, @cert, @privateKey, @secret, GETDATE(),@csr_text,GETDATE(),@otp,@compliance_request_id, @cert_type)", cn);

                cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id); 
                cmd.Parameters.AddWithValue("@companyID", UsersModal.loggedIncompanyID);
                cmd.Parameters.AddWithValue("@userID", UsersModal.logged_in_userid);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@cert", cert);
                cmd.Parameters.AddWithValue("@privateKey", privateKey);
                cmd.Parameters.AddWithValue("@secret", secret);
                cmd.Parameters.AddWithValue("@csr_text", csr);
                cmd.Parameters.AddWithValue("@otp", otp);
                cmd.Parameters.AddWithValue("@compliance_request_id", compliance_request_id);
                cmd.Parameters.AddWithValue("@cert_type", cert_type);

                newID = cmd.ExecuteNonQuery();
                newID += Convert.ToInt32(cmd.ExecuteScalar());

                return newID;
            }
        }
        public static int UpsertZatcaPCSIDCredentials(string cert_type, string mode, string cert, string secret,int parent_id)
        {
            int newID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(@"
            IF EXISTS (SELECT 1 FROM zatca_credentials WHERE branch_id = @branchId AND mode = @mode AND cert_type=@cert_type)
                UPDATE zatca_credentials 
                SET cert_base64 = @cert, secret_key = @secret, updated_at = GETDATE(), parent_id = @parent_id
                WHERE branch_id = @branchId AND mode = @mode AND cert_type=@cert_type
            ELSE
                INSERT INTO zatca_credentials (company_id, user_id, branch_id, mode, cert_base64, secret_key, updated_at, cert_type, parent_id)
                VALUES (@companyID,@userID,@branchId, @mode, @cert, @secret, GETDATE(),@cert_type, @parent_id)", cn);

                cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@companyID", UsersModal.loggedIncompanyID);
                cmd.Parameters.AddWithValue("@userID", UsersModal.logged_in_userid);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@cert", cert);
                cmd.Parameters.AddWithValue("@secret", secret);
                cmd.Parameters.AddWithValue("@cert_type", cert_type);
                cmd.Parameters.AddWithValue("@parent_id", parent_id);

                newID = cmd.ExecuteNonQuery();
                newID += Convert.ToInt32(cmd.ExecuteScalar());

                return newID;
            }
        }

        public static int UpdateZatcaStatus(int id)
        {
            int affectedRows = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                // 1. Set all credentials for this branch to inactive (status = 0)
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE zatca_credentials 
                    SET status = 0 
                    WHERE branch_id = @branchId", cn))
                {
                    cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
                    affectedRows += cmd.ExecuteNonQuery();
                }

                // 2. Set the selected credential to active (status = 1)
                using (SqlCommand cmd_1 = new SqlCommand(@"
                    UPDATE zatca_credentials 
                    SET status = 1 
                    WHERE branch_id = @branchId AND id = @ID", cn))
                {
                    cmd_1.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
                    cmd_1.Parameters.AddWithValue("@ID", id);
                    affectedRows += cmd_1.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }
        public static DataRow GetActiveZatcaCSID()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                string query = @"
                    SELECT TOP 1 *
                    FROM zatca_credentials
                    WHERE branch_id = @branchId AND status = 1";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
        }
        public static DataRow GetZatcaCredentialByParentID(int parentID)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                string query = @"
                    SELECT TOP 1 *
                    FROM zatca_credentials
                    WHERE branch_id = @branchId AND parent_id = @parent_id";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@parent_id", parentID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
        }

        public static DataRow GetZatcaCredentialByID(int id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                string query = @"
                    SELECT TOP 1 *
                    FROM zatca_credentials
                    WHERE branch_id = @branchId AND status = 1 AND id = @ID";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
        }
    }
    
}
