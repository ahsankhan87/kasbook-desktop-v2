using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Core.POS;
using POS.DLL.POS;

namespace POS.BLL.POS
{
    public class DebitNoteBLL
    {

        private readonly DebitNoteDLL _repository = new DebitNoteDLL();

        public void CreateDebitNote(DebitNoteModal note)
        {
            // Validate business rules
            if (note.Amount <= 0)
                throw new ArgumentException("Amount must be positive.");

            if (string.IsNullOrWhiteSpace(note.Reason))
                throw new ArgumentException("Reason is required.");

            // Additional ZATCA validations as needed

            // Save to database
            _repository.AddDebitNote(note);

            // Optionally, generate ZATCA-compliant XML/UBL here
        }

        public DebitNoteModal GetDebitNote(int debitNoteId)
        {
            return _repository.GetDebitNote(debitNoteId);
        }

        public List<DebitNoteModal> GetAllDebitNotes()
        {
            return _repository.GetAllDebitNotes();
        }

        // Add other business methods as needed

    }
}
