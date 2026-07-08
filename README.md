# Kasbook Desktop POS And Accounting App

## Product Introduction

**Kasbook Desktop POS And Accounting App** is an all-in-one business management solution for retail shops, trading businesses, wholesalers, and multi-branch operations. It combines **fast billing**, **inventory control**, **purchasing**, **customer and supplier management**, **accounting**, and **business reporting** in one easy-to-use desktop application.

Built on **.NET Framework 4.8 WinForms** with **SQL Server**, Kasbook is designed for businesses that want the speed and reliability of a desktop system while keeping daily operations organized and under control.

Whether the business needs to issue invoices quickly, track stock accurately, manage purchases, record journal entries, or review financial reports, Kasbook provides a single platform to handle the full workflow.

### Why Businesses Use Kasbook

- Faster sales billing and counter operations
- Better stock and purchase control
- Clear customer and supplier records
- Built-in accounting and journal management
- Branch-wise operational visibility
- Secure user access with permissions
- English and Arabic support
- Practical reports for daily decision-making

## Customer-Facing Features

Kasbook is designed to support the daily needs of growing businesses with practical, easy-to-access modules.

### Sales and Billing

- Fast point-of-sale billing
- New sales transaction entry
- Sales return handling
- Sales history and transaction listing
- Invoice generation and printing
- Customer-linked billing
- Tax-aware sales workflow

### Purchases and Supplier Operations

- Purchase entry and management
- Purchase return handling
- Supplier-linked purchasing workflow
- Purchase invoice support
- Purchase history and reporting

### Products and Inventory

- Product and service management
- Item master detail maintenance
- Inventory tracking support
- Branch-wise product handling
- Low stock monitoring
- Inventory reporting

### Customer and Supplier Management

- Customer records management
- Supplier records management
- Transaction linkage with customers and suppliers
- Better account tracking for business partners

### Accounting Features

- Journal entry management
- Journal daybook
- Chart of accounts support
- Account groups
- Trial balance
- Profit and loss report
- Balance sheet report
- Account and group reports

### Reports and Insights

- Sales reports
- Purchase reports
- Inventory reports
- Low stock reports
- Accounting reports
- Invoice and print-related reports
- Dashboard access to important operational screens

### Security and Control

- User login system
- Permission-based access control
- Restricted access by module and report
- Role-based operational control

### Business Operations Support

- Branch-wise working environment
- Company and fiscal-year session context
- Session lock and inactivity handling
- Subscription/runtime monitoring support
- English and Arabic language behavior
- ZATCA-related integration points in the solution

## Solution Overview

### Main Projects

- `pos`  
  Main WinForms desktop application.

- `POS.Core`  
  Shared models, session values, and domain objects.

- `POS.BLL`  
  Business logic layer.

- `POS.DLL`  
  Data access layer and SQL Server integration.

- `BenchmarkSuite1`  
  BenchmarkDotNet-based performance project for selected forms and UI/theme scenarios.

## Architecture

Standard application flow:

- `Form (pos)` → `BLL` → `DLL` → `SQL Server`

### Notes

- Most legacy data operations use `DataTable`.
- Database work relies on SQL queries and stored procedures.
- Shared session/user context is maintained in `POS.Core`.
- Permissions are enforced through the security layer in the desktop app.

## Technology Stack

- .NET Framework 4.8
- Windows Forms
- SQL Server
- BenchmarkDotNet
- Crystal Reports
- DGVPrinterHelper

## Application Startup

Entry point:

- `pos/Program.cs`

Startup flow:

- `Program.cs` → `Login` → main application form

## Configuration

Database connection string file:

- `pos/App.config`

Connection string name:

- `cn`

Update the SQL Server connection string before running in a new environment.

## Build

### Build the main application
