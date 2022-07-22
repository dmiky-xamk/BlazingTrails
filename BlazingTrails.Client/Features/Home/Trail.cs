namespace BlazingTrails.Client.Features.Home;

public class Trail
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Desciption { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int TimeInMinutes { get; set; }
    public int Length { get; set; }
    public IEnumerable<RouteInstruction> Route { get; set; } = Array.Empty<RouteInstruction>();

    public string TimeFormatted => $"{TimeInMinutes / 60}h {TimeInMinutes % 60}m";
}

public class RouteInstruction
{
    public int Stage { get; set; }
    public string Description { get; set; } = string.Empty;
}
