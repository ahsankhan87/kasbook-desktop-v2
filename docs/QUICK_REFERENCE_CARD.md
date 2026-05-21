# Stock Suppression - Quick Reference Card

## 🚀 Quick Start

### Open the Module
**Menu:** Inventory → Stock Suppression

### Basic Workflow
1. **Type** old part code → Wait 500ms → Product auto-populates
2. **Review** supersession status (green=good, orange=already linked)
3. **Type** new part code → Wait 500ms → Product auto-populates
4. **Click** Execute Supersession → Confirm → Done!

---

## ⌨️ Keyboard Shortcuts

| Key | Action |
|-----|--------|
| `Tab` | Move to next field |
| `Shift+Tab` | Move to previous field |
| `Alt+S` | Click Search button (Old Part) |
| `Alt+N` | Click Search button (New Part) |
| `Alt+C` | Click Select Company button |
| `Alt+X` | Click Execute Supersession |
| `Enter` | Confirm dialogs |
| `Esc` | Cancel/Close |

---

## 🎯 What Each Checkbox Does

| Option | Effect | Default |
|--------|--------|---------|
| **Transfer Stock** | Moves inventory from old → new item | ✅ ON |
| **Zero Demand** | Clears purchase/sales demand on old item | ✅ ON |
| **Transfer Description** | Copies description if new item empty | ⏳ OFF |
| **Reset Reorder** | Sets reorder level to 0 on new item | ✅ ON |

---

## 🎨 Color Guide

| Color | Meaning |
|-------|---------|
| 🟢 Green | Product found / Ready to proceed |
| 🔴 Red | Error / Product not found |
| 🟠 Orange | Warning / Already superseded |
| ⚫ Black | Normal / Neutral |

---

## 📊 Typical Times

| Action | Time |
|--------|------|
| Auto-search after typing | 500ms |
| Database lookup | 50-100ms |
| Supersession execution | 200-500ms |
| Form load | <200ms |
| Dialog open | <100ms |

---

## ❓ Common Issues & Fixes

| Issue | Fix |
|-------|-----|
| Search not triggering | Wait 500ms after typing stops |
| Product not found | Check spelling, try Search button |
| Already superseded error | Select different old part or click Help |
| Form won't close | Try again, check for error messages |
| Dialog won't open | Check network/database connection |

---

## 💡 Pro Tips

### ✨ Speed Tips
- Type **3+ characters** for faster matching
- Use **distinctive codes** (first 3 letters of name)
- Single results **auto-select** in dialog

### 🎯 Accuracy Tips
- **Always verify** product names match expectations
- **Check stock quantity** shown in dialog
- **Review** supersession status before proceeding
- **Start small** - test with 1 branch first

### 🔒 Safety Tips
- **Cannot undo** - review before executing
- **Creates link** - traceable history preserved
- **Transfers stock** - watch quantity changes
- **Audited** - all actions logged

---

## 📋 Pre-Execution Checklist

Before clicking Execute, verify:

- [ ] Old Part Code populated (with product name)
- [ ] New Part Code populated
- [ ] Status is green or acceptable
- [ ] At least 1 branch selected
- [ ] Checkboxes configured correctly
- [ ] Both parts are DIFFERENT (not same)

---

## 🆘 Need Help?

1. **In the Form:** Click "Help" button for detailed guide
2. **User Manual:** Ask admin for `STOCK_SUPPRESSION_USER_GUIDE.md`
3. **Report Issues:** Contact IT/System Administrator
4. **Quick Questions:** See this quick reference

---

## 🌍 Language

**Interface Supports:**
- English (EN)
- Arabic (العربية)

Switch language in application settings.

---

## 📞 Support Contacts

| Issue | Contact |
|-------|---------|
| Access/Permissions | System Administrator |
| Data Issues | Inventory Manager |
| Technical Bugs | IT Support |
| Training | Supervisor |

---

## 📅 Last Updated

**Date:** 2024
**Version:** 1.0
**Status:** Production Ready ✅

---

## 🎓 Learn More

### Available Documentation
- `STOCK_SUPPRESSION_USER_GUIDE.md` - Detailed user guide
- `STOCK_SUPPRESSION_DEBOUNCE_ENHANCEMENT.md` - Technical details
- `DEBOUNCE_IMPLEMENTATION_DETAILS.md` - Implementation reference

### In-App Help
- Click **Help** button in form for guided workflow
- Tooltips on each field for quick info
- Bilingual support (EN/العربية)

---

**Remember:** Type code → Wait 500ms → Done! 🎯
