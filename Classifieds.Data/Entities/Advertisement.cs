﻿namespace Classifieds.Data.Entities;

public class Advertisement
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}