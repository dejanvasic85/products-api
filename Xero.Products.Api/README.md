# Recommendations

After running out of time, I believe the following still needs to be addressed to make a production ready app:

- Unit of Work pattern across repositories to ensure atomic transactions
- API Versioning
- Performance - GetProducts should be including (pagination and filtering)
- Logging application level logs, http requests, latency and errors
- Monitoring / Metrics
- Dockerize (packaging)
- Integration Tests
- ORM choice - Dapper was the quickest in terms of performance and setup but possibly Entity Framework is better choice for code maintability
- **Opinionated** -> Request and Response Models that would be mapped to business models
- Error handling to log and not expose potential implementation detail to consumer
 

# Assumptions

- Responses are not case sensitive for the client. The requirements display Pascal Casing but the project is defined as regular camelCasing (recommended)