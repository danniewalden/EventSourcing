namespace EventSourcing.Marten_Wolverine.Testing;

[CollectionDefinition("IntegrationTests")]
public class SharedTestCollection : ICollectionFixture<CustomWebApplicationFactory>, ICollectionFixture<PostgreSqlFixture>;