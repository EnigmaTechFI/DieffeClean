﻿namespace DieffeClean.Presentation.Model.Staff;

public class NewStaffViewModel
{
    public Domain.Model.MyUser Staff { get; set; }
    
    public List<Domain.Model.Apartment> Apartaments { get; set; }
}