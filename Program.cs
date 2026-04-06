static async Task AddCustomerInteractive()
{
    //Code to get User Input:
    Console.Write("Customer name: ");
    var name = (Console.ReadLine() ?? "").Trim();

    Console.Write("Customer email: ");
    var email = (Console.ReadLine() ?? "").Trim();

    // Write an if statement to validate Name and Email input
    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
    {
        Console.WriteLine("Name and Email are required.");
        return;
    }

    //Create a new TrackerContext as a var
    using var ctx = new TrackerContext();

    //Create a var for a customer
    var customer = new Customer
    {
        Name = name,
        Email = email
    };

    //Then Add the new customer to Customers
    ctx.Customers.Add(customer);

    //Save Changes
    await ctx.SaveChangesAsync();

    //Report to end user that the customer was added
    Console.WriteLine($"Customer added: {customer.CustomerId} - {customer.Name} ({customer.Email})");
}

static async Task AddOrderInteractive()
{
    int customerId = ReadInt("Customer ID: ");
    double totalAmount = ReadDouble("Order Total Amount: ");

    // Validation
    if (totalAmount < 0)
    {
        Console.WriteLine("TotalAmount must be >= 0.");
        return;
    }

    using var ctx = new TrackerContext();

    // Create some validtion to prevent orphaned orders: verify customer exists
    var customer = await ctx.Customers.FindAsync(customerId);
    if (customer is null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    //Then create the new order and add it using the Tracker Context (ctx)
    var order = new Order
    {
        CustomerId = customerId,
        TotalAmount = totalAmount
    };
    ctx.Orders.Add(order);

    //After you add the order, use this to save the information:
    await ctx.SaveChangesAsync();

    //Then write an update to the console for the end user:
    Console.WriteLine($"Order added: {order.OrderId} for Customer {customer.Name} (ID {customer.CustomerId}) Amount: {order.TotalAmount:C}");
}

static async Task UpdateCustomerEmailInteractive()
{
    //Code to get some User Input and Validate the Email:
    int customerId = ReadInt("Customer ID: ");
    Console.Write("New email: ");
    var newEmail = (Console.ReadLine() ?? "").Trim();

    if (string.IsNullOrWhiteSpace(newEmail))
    {
        Console.WriteLine("Email is required.");
        return;
    }

    using var ctx = new TrackerContext();
    var customer = await ctx.Customers.FindAsync(customerId);

    //Create validaiton for customer not found (is null)
    if (customer is null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    //Set the customer Email to hte newEmail
    customer.Email = newEmail;

    //Save the changes
    await ctx.SaveChangesAsync();

    //Update the end user in the console
    Console.WriteLine($"Customer updated: {customer.CustomerId} - {customer.Name} ({customer.Email})");
}

static async Task DeleteCustomerInteractive()
{
    int customerId = ReadInt("Customer ID to delete: ");

    using var ctx = new TrackerContext();

    var customer = await ctx.Customers
        .Include(c => c.Orders)
        .FirstOrDefaultAsync(c => c.CustomerId == customerId);

    if (customer is null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    Console.WriteLine($"Deleting customer: {customer.CustomerId} - {customer.Name} ({customer.Email})");
    if (customer.Orders.Count > 0)
        Console.WriteLine($"NOTE: This customer has {customer.Orders.Count} order(s).");

    //Force the end user to write YES before we delete the customer
    Console.Write("Type YES to confirm deletion: ");
    var confirm = (Console.ReadLine() ?? "").Trim();
    if (!confirm.Equals("YES", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Deletion cancelled.");
        return;
    }

    // This will succeed if cascade delete is configured.
    // If not, you may need to delete orders first.
    ctx.Customers.Remove(customer);
    await ctx.SaveChangesAsync();

    Console.WriteLine("Customer deleted.");
}

static async Task DeleteOrderInteractive()
{
    int orderId = ReadInt("Order ID to delete: ");

    using var ctx = new TrackerContext();
    var order = await ctx.Orders.FindAsync(orderId);

    //Validate if the order is null and exit the oeration
    if (order is null)
    {
        Console.WriteLine("Order not found.");
        return;
    }

    //Remove from the Orders table and then save the changes
    ctx.Orders.Remove(order);
    await ctx.SaveChangesAsync();

    ///Report the result to the end user:
    Console.WriteLine($"Order deleted: {order.OrderId} Amount: {order.TotalAmount:C}");
}