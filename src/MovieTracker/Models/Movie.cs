﻿namespace MovieTracker.Models;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public double AverageRating { get; set; }
    public ICollection<FavoriteMovie> FavoriteMovies { get; set; }
}