using System;

namespace Catalog.Models
{

  public record Item
  {
    public Guid Id
    {
      get;
      init;
    }

    public string Name
    {
      get;
      init;
    }

    public double Price
    {
      get;
      init;
    }

    public DateTimeOffset CreatedDate
    {
      get;
      init;
    }
  }
}