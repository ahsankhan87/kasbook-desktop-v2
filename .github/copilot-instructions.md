# Copilot Instructions

## General Guidelines
- First general instruction
- Second general instruction

## Design Consistency
- Ensure a single unified font/size scale across the app.
- Centralize UI design styling in AppTheme and call it from forms instead of per-form styling.
- Ensure Uimessages are consistent across the app, e.g. use the same button style for all buttons.
- Ensure busyui is consistent across the app, e.g. use the same busyui for all forms.
- Ensure performance is consistent across the app, e.g. use the same caching strategy for all forms.
- Use dark black font color for form labels across the UI (sales, purchases, purchase orders).

## Lock Screen Implementation
- Use a dedicated WinForms lock screen (separate form with designer files) instead of building lock UI inline inside `frm_main`.