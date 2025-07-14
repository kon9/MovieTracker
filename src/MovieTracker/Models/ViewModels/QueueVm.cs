namespace MovieTracker.Models.ViewModels;

public class QueueVm
{
    public int Id { get; set; }
    public string QueueGroupName { get; set; }
    LinkedList<int> FilmChooseQueue { get; set; } = new LinkedList<int>();
}