using System.ComponentModel.DataAnnotations;

namespace MAUI_Library.Models.Enums;

public enum EventTypeEnum
{
    [Display(Name = "All events")]
    AllEvents,
    [Display(Name ="Car crash")]
    CarCrash,
    [Display(Name = "Fire")]
    Fire,
    [Display(Name = "Fire")]
    Flood,
    [Display(Name = "Earthquake")]
    Earthquake,
    [Display(Name = "Party")]
    Party,
    [Display(Name = "Public event")]
    PublicEvent
}
