using Boilerplate.Application.Interfaces;

namespace Boilerplate.Application.PaginatedLists;

public class GetOffersDtoPaginatedList
{
    [Newtonsoft.Json.JsonProperty("currentPage", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public int CurrentPage { get; set; }

    [Newtonsoft.Json.JsonProperty("totalPages", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public int TotalPages { get; set; }

    [Newtonsoft.Json.JsonProperty("totalItems", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public int TotalItems { get; set; } 
}