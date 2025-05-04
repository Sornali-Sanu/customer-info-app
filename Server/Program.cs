using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DTOs;
using Server.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("con")));
var app = builder.Build();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Get_Customer_List

app.MapGet("api/customer", async ([FromServices] AppDbContext db) => 
{
    return await db.Customers.Include(x=>x.Addresses).ToListAsync();
}).WithOpenApi().Produces<Customer[]>(StatusCodes.Status200OK);

//Delete_Customer

app.MapDelete("api/customer/{id}", async ([FromRoute] int id, [FromServices] AppDbContext db) =>
{
    var customer= await db.Customers.Include(x => x.Addresses).FirstOrDefaultAsync(x=>x.Id==id);
    db.Customers.Remove(customer);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi().Produces<Customer>(StatusCodes.Status204NoContent);

//Create_New_Customer

app.MapPost("api/customer", async ([FromBody] CustomerDto customer, [FromServices] AppDbContext db) =>
{
    var image = string.IsNullOrEmpty(customer.ImageBase64) ? null : customer.ImageBase64;
    var addresses = string.IsNullOrWhiteSpace(customer.AddressJson) ? new List<AddressDto>() : JsonSerializer.Deserialize<List<AddressDto>>(customer.AddressJson);

    var customer1 = new Customer
    {
        Name = customer.Name,
        IsRegular = customer.IsRegular,
        ImageUrl = image,
        Email = customer.Email,
        MobileNo = customer.MobileNo,
        DateofBirth = customer.DateofBirth,
    };
    db.Customers.Add(customer1);
    await db.SaveChangesAsync();

    var newCustomer = await db.Customers.FirstOrDefaultAsync(x => x.Name == customer1.Name);
    if (newCustomer != null && addresses != null)
    {
        foreach (var address in addresses) {
            var add = new Address
            {
                City = address.City,
                Street = address.Street,
                CustomerId=newCustomer.Id,
            };
            db.Addresses.Add(add);
        }
        await db.SaveChangesAsync();
    }
    return newCustomer;
    
}).WithOpenApi().Produces<Customer>(StatusCodes.Status201Created);
//Get_Customer_By_ID
app.MapGet("api/customer/{id}", async ([FromRoute]int id,[FromServices] AppDbContext db) =>
{
var customer = await db.Customers.Include(x => x.Addresses).FirstOrDefaultAsync(x=>x.Id==id);
    return customer is null ? Results.NotFound() : Results.Ok(customer);
}).WithOpenApi().Produces<Customer[]>(StatusCodes.Status200OK);
//Update_Customer
app.MapPut("api/customer/{id}", async ([FromRoute] int id,[FromBody] CustomerDto customer, [FromServices] AppDbContext db) =>
{
    var exCustomer = await db.Customers.Include(x => x.Addresses).FirstOrDefaultAsync(x=>x.Id==id);
    if (exCustomer == null)
    {
        return Results.NotFound();
    }
    exCustomer.Id =id;
    exCustomer.Name= customer.Name;
    exCustomer.Email=customer.Email;
    exCustomer.DateofBirth=customer.DateofBirth;
    exCustomer.MobileNo=customer.MobileNo;
    exCustomer.IsRegular=customer.IsRegular;

    if (!string.IsNullOrEmpty(customer.ImageBase64))
    {
        exCustomer.ImageUrl = customer.ImageBase64; 
    }

   
    List<Address> addresses;
    if (!string.IsNullOrWhiteSpace(customer.AddressJson))
    {
        addresses = JsonSerializer.Deserialize<List<Address>>(customer.AddressJson);
    }
    else
    {
        addresses = new List<Address>();
    }

    var addressId= addresses.Select(a=>a.Id).ToList();
    foreach (var address in addresses)
    {
        if (address.Id != 0)
        {
            var exAddress = exCustomer.Addresses.FirstOrDefault(a => a.Id == address.Id);
            if (exAddress != null)
            {
                exAddress.City = address.City;
                exAddress.Street = address.Street;
            }

        }
        else {
            var newAddress = new Address { 
            Street  = address.Street,
            City = address.City,
            CustomerId=exCustomer.Id,
            };
            db.Addresses.Add(newAddress);

        }
    }

    var addressToDelete = exCustomer.Addresses.Where(a => !addressId.Contains(a.Id)).ToList();
    db.RemoveRange(addressToDelete);
    await db.SaveChangesAsync();
    return Results.NoContent();
    

}).WithOpenApi().Produces<Customer>(StatusCodes.Status201Created);



app.Run();

