namespace Droits.Models.ViewModels;

public class LetterListView
{
    public LetterListView(List<LetterView> letters)
    {
        LetterList = letters;
    }


    public List<LetterView> LetterList { get; }
}