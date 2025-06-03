
public class ReadMe
{
    //This class serves as a placeholder for documentation or metadata about the WebAPI project.
    #region Common
    /* This Project is a subscription based multi-tenant WebAPI project.
     * 
     * Clean Layered Architecture: This Project references to infrastructure which references to Application and which references to Domain.
     * 
     */
    #endregion
    #region webapi
    /*
     * 
     */
    #endregion

    #region infrastructure
    /*
     * Housing the External Dependencies , Database Context, Repositories, and Migrations.
     * What Db are we using etc
     * Identity: It is there in our project but we are extending it according to our use so we will inherit it.
     */
    #endregion

    #region domain
    /* 
     * Housing the Domain Entities(Representation of Database Tables), Domain Services, Domain Events, and Domain Value Objects.
     */
    #endregion

    #region application
    /*
     *  House CQRS Implementations, Application Services, DTOs, Mappings, and MediatR Handlers. 
     * 
     */
    #endregion
}
