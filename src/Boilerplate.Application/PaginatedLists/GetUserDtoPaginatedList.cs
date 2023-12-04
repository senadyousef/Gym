using Boilerplate.Application.DTOs.User;

namespace Boilerplate.Application.PaginatedLists;

public class GetUserDtoPaginatedList
{
    [Newtonsoft.Json.JsonProperty("currentPage", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public int CurrentPage { get; set; }

    [Newtonsoft.Json.JsonProperty("totalPages", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public int TotalPages { get; set; }

    [Newtonsoft.Json.JsonProperty("totalItems", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public int TotalItems { get; set; }

    [Newtonsoft.Json.JsonProperty("result", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public System.Collections.Generic.ICollection<GetUserExtendedDto> Result { get; set; }
}