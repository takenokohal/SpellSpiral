using System.Collections.Generic;

namespace Others.Dialog
{
    public class ChoiceDialogData
    {
        public string DescriptionTextKey { get; }

        public IReadOnlyList<string> ChoiceTextKeys { get; }

        public ChoiceDialogData(string descriptionTextKey, IReadOnlyList<string> choiceTextKeys)
        {
            DescriptionTextKey = descriptionTextKey;
            ChoiceTextKeys = choiceTextKeys;
        }

        public static ChoiceDialogData CreateYesNoDialog(string descriptionTextKey)
        {
            return new ChoiceDialogData(descriptionTextKey, new[]
            {
                "yes", "no"
            });
        }

        public static ChoiceDialogData CreateYesDialog(string descriptionTextKey)
        {
            return new ChoiceDialogData(descriptionTextKey, new[]
            {
                "yes"
            });
        }
    }
}