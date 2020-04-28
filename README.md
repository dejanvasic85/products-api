# Recommendations

I believe the following still needs to be addressed to make a production ready app:

- API Versioning
- Performance - GetProducts should be including (pagination and filtering)
- Logging application level logs, http requests, latency and errors
- Monitoring / Metrics
- Dockerize (packaging)
- Integration Tests
- ORM choice - Dapper was the quickest in terms of performance and setup but possibly Entity Framework is better choice for code maintanability
- Error handling to log and not expose potential implementation detail to consumer
 

# Assumptions

- The POST methods should create a new ID on the server rather than the client
- Responses are not case sensitive for the client. The requirements display Pascal Casing but the project is defined as regular camelCasing (recommended)
- Another naming convention for models returned by controllers are Resources


# refactor-this
The attached project is a poorly written products API in C#.

Please evaluate and refactor areas where you think can be improved. 

Consider all aspects of good software engineering and show us how you'll make it #beautiful and make it a production ready code.

## Getting started for applicants

There should be these endpoints:

1. `GET /products` - gets all products.
2. `GET /products?name={name}` - finds all products matching the specified name.
3. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products` - creates a new product.
5. `PUT /products/{id}` - updates a product.
6. `DELETE /products/{id}` - deletes a product and its options.
7. `GET /products/{id}/options` - finds all options for a specified product.
8. `GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
9. `POST /products/{id}/options` - adds a new product option to the specified product.
10. `PUT /products/{id}/options/{optionId}` - updates the specified product option.
11. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

All models are specified in the `/Models` folder, but should conform to:

**Product:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description",
  "Price": 123.45,
  "DeliveryPrice": 12.34
}
```

**Products:**
```
{
  "Items": [
    {
      // product
    },
    {
      // product
    }
  ]
}
```

**Product Option:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description"
}
```

**Product Options:**
```
{
  "Items": [
    {
      // product option
    },
    {
      // product option
    }
  ]
}
```
