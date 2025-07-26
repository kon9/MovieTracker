namespace MovieTracker.Models;

public class Queue
{
    public int Id { get; set; }
    public string QueueGroupName { get; set; }
    LinkedList<int> FilmChooseQueue { get; set; } = new LinkedList<int>();
    public int CreatorId { get; set; }
}