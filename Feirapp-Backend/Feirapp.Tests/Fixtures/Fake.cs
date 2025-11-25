using System;
using Bogus;
using Bogus.Extensions.Brazil;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;

namespace Feirapp.Tests.Fixtures;

public static class Fake
{
    public static GroceryItem GroceryItem(string? name = null, Guid? id = null)
    {
        var faker = new Faker<GroceryItem>()
            .RuleFor(g => g.Id, _ => Guid.NewGuid())
            .RuleFor(g => g.Name, f => f.Commerce.ProductName())
            .RuleFor(g => g.Barcode, f => f.Commerce.Ean13())
            .RuleFor(g => g.MeasureUnit, f => f.PickRandom<MeasureUnitEnum>());
        
        if (!string.IsNullOrWhiteSpace(name))
            faker.RuleFor(s => s.Name, _ => name);
        if (id != null)
            faker.RuleFor(s => s.Id, _ => id);
        
        return faker.Generate();
    }
    
    public static Store Store(string? name = null, Guid? id = null, string? cnpj = null, string? city = null, StatesEnum state = StatesEnum.Empty)
    {
        var faker = new Faker<Store>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.Name, f => f.Company.CompanyName())
            .RuleFor(s => s.Cnpj, f => f.Company.Cnpj())
            .RuleFor(s => s.State, f => f.PickRandomWithout(StatesEnum.Empty));
        
        if (!string.IsNullOrWhiteSpace(name))
            faker.RuleFor(s => s.Name, _ => name);
        if (!string.IsNullOrWhiteSpace(cnpj))
            faker.RuleFor(s => s.Cnpj, _ => cnpj);
        if (id != null)
            faker.RuleFor(s => s.Id, _ => id);
        if (!string.IsNullOrWhiteSpace(city))
            faker.RuleFor(s => s.CityName, _ => city);
        if(state != StatesEnum.Empty)
            faker.RuleFor(s => s.State, _ => state);
        
        return faker.Generate();
    }
    
    public static PriceLog PriceLog(Guid groceryItemId, string barcode, Guid storeId, DateTime? logDate = null)
    {
        var faker = new Faker<PriceLog>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.GroceryItemId, _ => groceryItemId)
            .RuleFor(p => p.StoreId, _ => storeId)
            .RuleFor(p => p.Barcode, _ => barcode)
            .RuleFor(p => p.Price, f => Math.Round(f.Random.Decimal(1M, 100M)))
            .RuleFor(p => p.LogDate, f => f.Date.Recent());
        
        if(logDate != null)
            faker.RuleFor(p => p.LogDate, _ => logDate);
        
        return faker.Generate();
    }

    public static User User(
        Guid? id = null,
        string? name = null,
        string? email = null,
        string? password = null,
        UserStatus status = UserStatus.Empty,
        DateTime? lastLogin = null,
        DateTime? createdAt = null)
    {
        var faker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.PasswordSalt, f => f.Internet.Password())
            .RuleFor(u => u.Status, f => f.PickRandom<UserStatus>())
            .RuleFor(u => u.CreatedAt, f => f.Date.Recent());

        if(id != null)
            faker.RuleFor(u => u.Id, _ => id);
        if (!string.IsNullOrWhiteSpace(name))
            faker.RuleFor(u => u.Name, _ => name);
        if (!string.IsNullOrWhiteSpace(email))
            faker.RuleFor(u => u.Email, _ => email);
        if (!string.IsNullOrWhiteSpace(password))
            faker.RuleFor(u => u.Password, _ => password);
        if (status != UserStatus.Empty)
            faker.RuleFor(u => u.Status, _ => status);
        if (createdAt != null)
            faker.RuleFor(u => u.CreatedAt, _ => createdAt);
        if (lastLogin != null)
            faker.RuleFor(u => u.LastLogin, _ => lastLogin);
        
        return faker.Generate();
    }
}