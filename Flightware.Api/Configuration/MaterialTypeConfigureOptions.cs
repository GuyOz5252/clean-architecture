using Flightware.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Flightware.Api.Configuration;

public class MaterialTypeConfigureOptions : IConfigureOptions<List<MaterialType>>,
    IOptionsChangeTokenSource<List<MaterialType>>
{
    private readonly IConfiguration _configuration;

    public MaterialTypeConfigureOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(List<MaterialType> options)
    {
        options.Clear();
        var section = _configuration.GetSection("MaterialTypes");
        options.AddRange(from materialSection in section.GetChildren()
            where materialSection is not null
            let name = materialSection.Key
            let orderParams = materialSection
                                  .GetSection("OrderParameters")
                                  .GetSection("OrderParameters")
                                  .Get<List<OrderParameter>>()
                              ?? throw new Exception()
            select new MaterialType { Name = name, OrderParameters = orderParams });
    }

    public IChangeToken GetChangeToken() => _configuration.GetReloadToken();

    public string Name => Options.DefaultName;
}
