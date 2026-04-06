using Microsoft.EntityFrameworkCore;

public class TrackerContext : DbContext
{
    //Create a public DbSet for Customers and Orders. Set them by type


	//Create a protected override void to set the Data Source to CustomerOrders.db

	//Use this potected override void for OnModelCreating
	//Pass in a ModelBuilder
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Use the ModelBuilder.Entity to ensure Customer has a unique email
		

		//Use the ModelBuilder.Entity to set a One to Many relationship between customer and orders
		//Enforce the Foreign Key with HasForeignKey
		//Use OnDelete to cascade delete orders for the deleted customer


		//Use ModelBuilder.Entity to ensure that Order has a Total Amount
		//Set Precision to (18, 2)

    }
}
